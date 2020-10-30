using ImageResizer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MrSimonAcademy2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManger;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManger ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManger = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // Список отключенных пользователей 
        public ActionResult DisabledUsers()
        {
            List<User> users = new List<User>();

            foreach (User u in db.Users.ToList())
            {
                if (!u.isActive)
                    users.Add(u);

            }
            return View(users);
        }

        // Список пользователей 
        [ChildActionOnly]
        public ActionResult Users()
        {
            List<User> users = new List<User>();

            foreach (var u in db.Users.ToList())
            {
                UserManager.PasswordHasher.HashPassword("1234");
                if (u.isActive)
                    users.Add(u);
            }

            UserComparer uc = new UserComparer();
            users.Sort(uc);

            return PartialView(users);
        }


        // Поиск пользователей 
        [HttpPost]
        public ActionResult SearchUser(string name)
        {
            var student = new List<User>();

            name = name.ToUpper();

            student = db.Users.Where(st =>
                        (name.Contains(st.UserFName.ToUpper()) || name.Contains(st.UserLName.ToUpper()) || st.UserFName.ToUpper().Contains(name)
                        || st.UserLName.ToUpper().Contains(name) || name.ToUpper().Contains(st.RoleName.ToUpper()) || name.ToUpper().Contains(st.RoleName.ToUpper()) || st.RoleName.ToUpper().Contains(name) || st.RoleName.ToUpper().Contains(name)) && st.isActive).ToList();

            UserComparer uc = new UserComparer();
            student.Sort(uc);
            return PartialView(student);
        }

        // Добавление пользователя (редирект)
        public ActionResult AddUser()
        {
            return RedirectToActionPermanent("Register", "Account", new RegisterModel());
            //return Redirect("/Account/Register");
        }

        // Отключение пользователя 
        public ActionResult DisableUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
                return RedirectToAction("NotFound", "Error");

            user.isActive = false;
            // Удаление неактивного студента из его групп 
            if (user.RoleName != "Admin")
            {
                foreach (Group g in user.UserGroups.ToList())
                {
                    g.GroupStudents.Remove(user);
                    g.GroupCount--;
                    if (g.GroupCount == 0)
                    {
                        db.Groups.Remove(g);
                    }
                    if (g.GroupCount == 1)
                    {
                        g.isPersonal = true;
                    }
                }

                user.UserGroups.Clear();
            }
            user.shutdownDate = DateTime.Now;
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        // Восстановление пользователя 
        public ActionResult RestoreUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
                return RedirectToAction("NotFound", "Error");

            user.isActive = true;
            // При  восстановлении пользователя он не возвращается в группы 
            db.SaveChanges();

            return RedirectToAction("DisabledUsers");
        }

        // Просмотр статистики студента
        [AllowAnonymous]
        public ActionResult GetUser(string id)
        {
            if (id == "" || id == null)
                return RedirectToAction("NotFound", "Error");

            return RedirectToAction("Statistic", "Manage", new { userId = id });
        }

        // Редактирование пользователей
        public ActionResult EditUser(string id)
        {
            User u = db.Users.Find(id);
            if (u == null)
                return RedirectToAction("NotFound", "Error");

            return View(u);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(string id, string role, string UserFName, string UserLName, string level, string date, string withoutAvatar, HttpPostedFileBase photo)
        {

            User u = db.Users.Find(id);
            if (u == null)
                return RedirectToAction("NotFound", "Error");

            if (role != u.RoleName && role != "")
            {
                _ = await UserManager.AddToRoleAsync(u.Id, role);
                u.RoleName = role;

            }

            if (level != u.levelName && level != "")
                u.levelName = level;
            if (UserFName != u.UserFName && UserFName != "")
                u.UserFName = UserFName;
            if (UserLName != u.UserLName && UserLName != "")
                u.UserLName = UserLName;

            if (date != null && date != "")
            {
                string[] items = date.Split(new char[] { '-' });
                DateTime bd = new DateTime(Convert.ToInt32(items[0]), Convert.ToInt32(items[1]), Convert.ToInt32(items[2]));
                if (u.Birthday != bd)
                    u.Birthday = bd;
            }


            bool isWithoutAvatar = (withoutAvatar == "true") ? true : false;

            // Если заменили на стандартное фото
            if (!u.withoutAvatar && isWithoutAvatar)
            {
                // Удалить старое фото
                u.withoutAvatar = true;
                System.IO.File.Delete(Server.MapPath("~/u_photos/" + u.avatarName));
            }
            else if (!isWithoutAvatar && photo != null) // Если заменили на какое-то другое фото
            {
                if (!u.withoutAvatar)
                {
                    System.IO.File.Delete(Server.MapPath("~/u_photos/" + u.avatarName));
                }

                u.withoutAvatar = false;

                string extension = Path.GetExtension(photo.FileName);

                string fileName = u.Id + extension;


                var path = Server.MapPath("~/u_photos/");
                photo.InputStream.Seek(0, System.IO.SeekOrigin.Begin);

                ImageBuilder.Current.Build(
                    new ImageJob(
                        photo.InputStream,
                        path + fileName,
                        new Instructions("maxwidth=200&maxheight=200&format=jpg"),
                        false,
                        true));

                //// получаем имя файла


                //photo.SaveAs(Server.MapPath("~/u_photos/" + fileName + extension));


                u.avatarName = fileName + ".jpg";

            }

            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        // Список групп
        [ChildActionOnly]
        public ActionResult Groups()
        {
            List<Group> groups = db.Groups.ToList();

            List<int> fileCounts = new List<int>();
            List<Theme> lastThemes = new List<Theme>();
            List<DateTime> lastFiles = new List<DateTime>();
            List<Assignment> tempFileList = new List<Assignment>();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            List<string> teachers = new List<string>();

            foreach (Group g in groups)
            {
                // проверка активен ли учитель группы
                if (db.Users.Find(g.GroupTeacherId).isActive)
                {
                    string teacherId = g.GroupTeacherId;
                    string teacherName = db.Users.Find(teacherId).UserFName + " " + db.Users.Find(teacherId).UserLName;
                    teachers.Add(teacherName);
                }
                else
                {
                    teachers.Add("none");
                }

                // Проверка активны ли студенты
                // Подсчет активных студентов в группе 
                int activeCount = 0;
                foreach (User u in g.GroupStudents)
                {
                    if (u.isActive)
                        activeCount++;
                }
                // Если группа пуста (все студенты в ней не активны), то ее не нужно показывать 
                if (activeCount == 0)
                    groups.Remove(g);


                // Подсчет количества файлов, которые доступны группе
                if (g.ThemeList.Count() != 0)
                {
                    Theme lastTheme = g.ThemeList.ToList()[g.ThemeList.Count() - 1];
                    lastThemes.Add(lastTheme);

                }
                else
                {
                    lastThemes.Add(null);

                }

                int count = 0;
                foreach (Theme th in g.ThemeList)
                {
                    if (th.AssignmentList.Count() != 0)
                        tempFileList.AddRange(th.AssignmentList.ToList());
                    else
                        lastFiles.Add(new DateTime(1900, 01, 01));

                    count = count + th.AssignmentList.Count();
                }


                FileComparer fc = new FileComparer();
                tempFileList.Sort(fc);

                if (tempFileList.Count() != 0)
                    lastFiles.Add(tempFileList[0].added);

                fileCounts.Add(count);

            }


            ViewBag.Teachers = teachers;

            ViewBag.lastThemes = lastThemes;
            ViewBag.lastFiles = lastFiles;
            ViewBag.FileCounts = fileCounts;

            return PartialView(groups);
        }

        // Поиск групп
        [HttpPost]
        public ActionResult SearchGroup(string name)
        {
            var groups = new List<Group>();

            if (name == null || name == "")
                groups = db.Groups.ToList();
            else
            {
                name = name.ToUpper();
                groups = db.Groups.Where(g => (g.GroupName.ToUpper().Contains(name) || name.Contains(g.GroupName.ToUpper()))).ToList();
            }
            List<string> teachers = new List<string>();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            foreach (Group g in groups)
            {
                // проверка активен ли учитель группы
                if (db.Users.Find(g.GroupTeacherId).isActive)
                {
                    string teacherId = g.GroupTeacherId;
                    string teacherName = db.Users.Find(teacherId).UserFName + " " + db.Users.Find(teacherId).UserLName;
                    teachers.Add(teacherName);
                }
                else
                {
                    teachers.Add("none");
                }

            }



            ViewBag.Teachers = teachers;
            ViewBag.Count = groups.Count;
            return PartialView(groups);
        }

        // Добавление группы (GET)
        [ChildActionOnly]
        public ActionResult AddGroup()
        {

            Dictionary<string, string> students = new Dictionary<string, string>();
            Dictionary<string, string> teachers = new Dictionary<string, string>();


            foreach (var user in UserManager.Users.ToList())
            {

                if (user.RoleName == "Student" && user.isActive)
                    students.Add(user.Id, user.UserFName + " " + user.UserLName);
                if (user.RoleName == "Teacher" && user.isActive)
                    teachers.Add(user.Id, user.UserFName + " " + user.UserLName);

            }

            ViewBag.Students = students;
            ViewBag.Teachers = teachers;


            //ViewBag.Days = days;

            return PartialView();
        }

        // Добавление группы (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroup(Group group, List<string> students, List<string> teachers, string language, bool isPersonal)
        {
            if (ModelState.IsValid)
            {
                group.GroupCount = students.Count;
                group.Language = language;
                group.isPersonal = isPersonal;

                foreach (var s in db.Users.Where(student => students.Contains(student.Id)))
                    group.GroupStudents.Add(s);

                //foreach (var t in db.Users.Where(teacher => teacher.RoleName == "Teacher" && teachers.Contains(teacher.Id)))
                // TODO: Выбирать можно только 1го учителя
                group.GroupTeacherId = teachers[0];



                User teacher = db.Users.Find(teachers[0]);
                teacher.UserGroups.Add(group);

                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // Удаление группы 
        [HttpGet]
        public ActionResult DeleteGroup(int id)
        {
            Group g = db.Groups.Find(id);
            if (g == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            // Удаление всех файлов 


            List<Theme> themeList = g.ThemeList.ToList();
            foreach (Theme th in themeList)
            {

                if (th != null)
                {
                    List<Assignment> fileList = th.AssignmentList.ToList();
                    foreach (Assignment file in fileList)
                    {
                        if (file.type == "Link")
                            db.Assignments.Remove(file);
                        else
                        {
                            System.IO.File.Delete(Server.MapPath("~/th_files/" + file.AssignmentFileName + file.AssignmentFileExpansion));
                            db.Assignments.Remove(file);
                        }

                    }
                }

            }

            db.Groups.Remove(g);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // Исключение студента из группы
        [HttpGet]
        public ActionResult ExcludeStudent(string groupId, string userId)
        {
            Group g = db.Groups.Find(Convert.ToInt32(groupId));
            User u = db.Users.Find(userId);
            if (g == null || u == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (g.GroupCount == 1)
            {
                g.GroupStudents.Remove(u);
                db.Groups.Remove(g);
            }
            else
            {
                g.GroupStudents.Remove(u);
                g.GroupCount = g.GroupCount - 1;
                if (g.GroupCount == 1)
                {
                    g.isPersonal = true;
                }
            }


            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Редактирование группы (GET)
        [HttpGet]
        public ActionResult EditGroup(int? id)
        {
            Group g = db.Groups.Find(id);
            if (g == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            Dictionary<string, string> students = new Dictionary<string, string>();
            Dictionary<string, string> currentStudents = new Dictionary<string, string>();
            Dictionary<string, string> teachers = new Dictionary<string, string>();


            foreach (var user in UserManager.Users.ToList())
            {

                if (user.RoleName == "Student" && user.isActive)
                    students.Add(user.Id, user.UserFName + " " + user.UserLName);
                if (user.RoleName == "Teacher" && user.isActive)
                    teachers.Add(user.Id, user.UserFName + " " + user.UserLName);
            }

            foreach (User user in g.GroupStudents)
            {
                if (user.RoleName != "Teacher")
                    currentStudents.Add(user.Id, user.UserFName + " " + user.UserLName);
            }

            ViewBag.Students = students;
            ViewBag.CurrentStudents = currentStudents;
            ViewBag.Teachers = teachers;
            ViewBag.CurrentTeacherName = db.Users.Find(g.GroupTeacherId).UserFName + " " + db.Users.Find(g.GroupTeacherId).UserLName;

            return View(g);
        }

        // Редактирование группы (POST)
        [HttpPost]
        public ActionResult EditGroup(string GroupName, string GroupLevel, string days, string textbook, List<string> students, List<string> teachers, string language, bool isPersonal, int groupId)
        {

            Group g = db.Groups.Find(groupId);
            if (g == null)
                return RedirectToAction("NotFound", "Error");

            if (g.GroupName != GroupName && GroupName != "")
                g.GroupName = GroupName;

            if (g.GroupLevel != GroupLevel && GroupLevel != "")
                g.GroupLevel = GroupLevel;

            if (g.days != days && days != "")
                g.days = days;

            if (g.textbook != textbook && textbook != "")
                g.textbook = textbook;

            if (g.Language != language && language != "")
                g.Language = language;

            if (g.isPersonal != isPersonal)
                g.isPersonal = isPersonal;

            if (teachers.Count() != 0 && students.Count() != 0)
            {

                // Если учитель изменился 
                if (g.GroupTeacherId != teachers[0])
                {
                    User lastTeacher = db.Users.Find(g.GroupTeacherId);
                    User newTeacher = db.Users.Find(teachers[0]);

                    if (newTeacher == null || lastTeacher == null)
                        return RedirectToAction("NotFound", "Error");

                    lastTeacher.UserGroups.Remove(g);
                    newTeacher.UserGroups.Add(g);
                    g.GroupTeacherId = newTeacher.Id;
                }

                // Очистка списка студентов 
                foreach (User u in g.GroupStudents)
                {
                    u.UserGroups.Remove(g);
                }
                g.GroupStudents.Clear();

                foreach (string stId in students)
                {
                    User st = db.Users.Find(stId);

                    if (st == null)
                        return RedirectToAction("NotFound", "Error");
                    g.GroupStudents.Add(st);
                    st.UserGroups.Add(g);
                }

                g.GroupCount = students.Count;


                if (students.Count() == 1)
                    g.isPersonal = true;
                else
                    g.isPersonal = false;

                //// Удаление прошлых студентов и учителя из группы 
                //foreach (User u in g.GroupStudents)
                //{
                //    u.UserGroups.Remove(g);
                //}
                //g.GroupStudents.Clear();



                //// Добавление новых студентов
                //foreach (var s in db.Users.Where(student => students.Contains(student.Id)))
                //    g.GroupStudents.Add(s);

                //// Добавление нового учителя
                //g.GroupTeacherId = teachers[0];

                //User teacher = db.Users.Find(teachers[0]);
                //if (teacher == null)
                //    return RedirectToAction("NotFound", "Error");

                //teacher.UserGroups.Add(g);
            }
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // Добавление статистики студенту (GET)
        [ChildActionOnly]
        public ActionResult AddStatistic()
        {

            return PartialView();
        }

        // Добавление статистики студенту (POST)
        [HttpPost]
        public ActionResult AddStatistic(string userId, int lessonsCount = 0, int loseLessons = 0, int homeworkCount = 0, int passedHomework = 0,
            int Speaking = 0, int Listening = 0, int Writing = 0, int Reading = 0, int activity = 0, int concentration = 0, string adventuresPlus = "", string adventuresMinus = "")
        {
            User user = db.Users.Find(userId);

            Feedback model = new Feedback
            {
                User = user,
                UserId = userId,
                LessonsCount = lessonsCount,
                LoseLessons = loseLessons,
                HomeworkCount = homeworkCount,
                PassedHomework = passedHomework,
                Activity = activity,
                Concentration = concentration,
                AddingData = DateTime.Now,
            };


            // Добавление новости студенту о том, что ему добавили группу 
            News news = new News
            {
                title = "Узнайте свой прогресс",
                message = "У Вас появилась статистика. " +
                    "Ваша успеваемость, активность и концентрация на занятиях. Посмотрите ее, чтобы узнать, как улучшить знания иностранного.",
                fromId = "System",
                isFromSystem = true,
                added = DateTime.Now,
                isPinned = false,
                isForUser = true,
                isForAll = false,
                isForGroup = false,
                onlyAdmin = false,
                onlyStudent = false,
                onlyTeacher = false,
            };


            news.recipientList.Add(user);
            user.UserNews.Add(news);
            db.News.Add(news);

            List<Rating> rating = new List<Rating>();

            if (Speaking != 0)
                rating.Add(new Rating("Speaking", Speaking));
            if (Writing != 0)
                rating.Add(new Rating("Writing", Writing));
            if (Reading != 0)
                rating.Add(new Rating("Reading", Reading));
            if (Listening != 0)
                rating.Add(new Rating("Listening", Listening));

            if (rating.Count != 0)
                model.RatingList = rating;

            if (adventuresPlus != "")
                model.AdventuresPlus = adventuresPlus;
            if (adventuresMinus != "")
                model.AdventuresMinus = adventuresMinus;

            user.FeedbackList.Add(model);
            user.hasFeedback = true;
            user.lastFeedback = model.AddingData;
            db.Feedbacks.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        // GET: Admin/News
        public ActionResult News()
        {
            return View();
        }

        // Список всех сообщений 
        [ChildActionOnly]
        public ActionResult AllMessages()
        {
            NewsComparer nc = new NewsComparer();

            List<News> news = db.News.Where(n => !n.isFromSystem).ToList();
            news.Sort(nc);

            List<string> senders = new List<string>();
            List<string> recipients = new List<string>();

            foreach (News n in news)
            {
                User userFrom = db.Users.Find(n.fromId);
                // from 
                if (n.isFromSystem)
                    senders.Add("Система");
                else
                {
                    if (userFrom == null)
                        if (n.isFromSystem)
                            senders.Add("Система");
                        else
                            senders.Add("none");
                    else
                        senders.Add(userFrom.UserFName + " " + userFrom.UserLName);
                }

                // to
                if (n.isForAll)
                {
                    if (n.onlyStudent)
                        recipients.Add("Только студенты");
                    if (n.onlyTeacher)
                        recipients.Add("Только учителя");
                    if (n.onlyAdmin)
                        recipients.Add("Только администраторы");
                    if (!n.onlyTeacher && !n.onlyStudent && !n.onlyAdmin)
                        recipients.Add("Все пользователи");

                }



                if (n.isForUser)
                {
                    if (n.recipientList.Count != 0)
                    {
                        int count = n.recipientList.Count() - 1;
                        recipients.Add(n.recipientList.ToList()[count].UserFName + " " + n.recipientList.ToList()[count].UserLName);
                    }
                    else
                        recipients.Add("Error");

                }

                if (n.isForGroup)
                {
                    if (n.recipientList.Count != 0)
                    {
                        int count = n.recipientList.Count() - 1;
                        recipients.Add(n.groupName);
                    }
                    else
                        recipients.Add("Error");

                }



            }


            ViewBag.Senders = senders;
            ViewBag.Recipients = recipients;
            return PartialView(news);
        }

        // Поиск пользователей 
        [HttpPost]
        public ActionResult SearchMessages(string fragment)
        {

            fragment = fragment.ToUpper();

            NewsComparer nc = new NewsComparer();

            List<News> news = new List<News>();
            news = db.News.Where(n => (fragment.Contains(n.title.ToUpper()) || fragment.Contains(n.message.ToUpper()) || n.message.ToUpper().Contains(fragment)
                        || n.title.ToUpper().Contains(fragment))).ToList();

            news.Sort(nc);

            List<string> senders = new List<string>();
            List<string> recipients = new List<string>();

            foreach (News n in news)
            {
                User user = db.Users.Find(n.fromId);
                // from 
                if (n.isFromSystem)
                    senders.Add("Система");
                else
                {
                    if (user == null)
                        if (n.isFromSystem)
                            senders.Add("Система");
                        else
                            senders.Add("none");
                    else
                        senders.Add(user.UserFName + " " + user.UserLName);
                }

                // to
                if (n.isForAll)
                {
                    if (n.onlyStudent)
                        recipients.Add("Только студенты");
                    if (n.onlyTeacher)
                        recipients.Add("Только учителя");
                    if (n.onlyAdmin)
                        recipients.Add("Только администраторы");
                    if (!n.onlyTeacher && !n.onlyStudent && !n.onlyAdmin)
                        recipients.Add("Все пользователи");

                }

                if (n.isForUser)
                    recipients.Add(n.recipientList.Last().UserFName + " " + n.recipientList.Last().UserLName);

            }
            ViewBag.Senders = senders;
            ViewBag.Recipients = recipients;


            return PartialView(news);
        }

        // Добавление сообщения студенту (GET)
        [ChildActionOnly]
        public ActionResult AddMessage()
        {
            return PartialView();
        }

        // Добавление сообщения студенту (POST)
        [HttpPost]
        public ActionResult AddMessage(string targetId, string targetType, string msgText, string msgTitle)
        {
            List<User> recipientList = new List<User>();
            News model = new News
            {
                message = msgText,
                title = msgTitle,
                fromId = User.Identity.GetUserId(),
                isFromSystem = false,
                added = DateTime.Now,
                isPinned = false,
                onlyAdmin = false,
                onlyStudent = false,
                onlyTeacher = false,
                groupName = "none",
                groupId = 0,
            };

            if (targetType == "USER")
            {
                model.isForUser = true;
                model.isForAll = false;
                model.isForGroup = false;
                User user = db.Users.Find(targetId);
                model.recipientList.Add(user);
                user.UserNews.Add(model);

            }
            else
            {
                return RedirectToAction("NotFound", "Error");

            }


            db.News.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        // Удаление сообщения
        [HttpGet]
        public ActionResult DeleteMessage(int id)
        {
            News news = db.News.Find(id);

            if (news == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            foreach (User u in news.recipientList)
            {
                u.UserNews.Remove(news);
            }

            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("News", "Admin");
        }

        // Добавление сообщения всем студентам (GET)
        [ChildActionOnly]
        public ActionResult AddMessageForAll()
        {
            return PartialView();
        }

        // Добавление сообщения студенту (POST)
        [HttpPost]
        public ActionResult AddMessageForAll(string msgText, string msgTitle, string targetType)
        {

            News model = new News
            {
                message = msgText,
                title = msgTitle,
                fromId = User.Identity.GetUserId(),
                isFromSystem = false,
                isForAll = true,
                isForGroup = false,
                isForUser = false,
                onlyAdmin = false,
                onlyStudent = false,
                onlyTeacher = false,
                isPinned = false,
                added = DateTime.Now,
                groupId = 0,
                groupName = "none",
            };

            foreach (User u in db.Users.ToList())
            {

                if (targetType == "All" || targetType == null)
                {
                    model.recipientList.Add(u);
                    u.UserNews.Add(model);
                }
                else
                {
                    if (targetType == "Admin" && u.RoleName == "Admin")
                    {
                        model.recipientList.Add(u);
                        model.onlyAdmin = true;
                        u.UserNews.Add(model);

                    }
                    if (targetType == "Teacher" && u.RoleName == "Teacher")
                    {
                        model.recipientList.Add(u);
                        model.onlyTeacher = true;
                        u.UserNews.Add(model);
                    }

                    if (targetType == "Student" && u.RoleName == "Student")
                    {
                        model.recipientList.Add(u);
                        model.onlyStudent = true;
                        u.UserNews.Add(model);
                    }
                }



            }


            db.News.Add(model);
            db.SaveChanges();
            return RedirectToAction("News", "Admin");
        }

        // Добавление сообщения всем студентам (GET)
        public ActionResult AddMessageForGroup(int id)
        {
            Group g = db.Groups.Find(id);
            if (g == null)
                return RedirectToAction("NotFound", "Error");

            return View(g);
        }

        // Добавление сообщения студенту (POST)
        [HttpPost]
        public ActionResult AddMessageForGroup(int id, string msgText, string msgTitle)
        {

            Group g = db.Groups.Find(id);
            if (g == null)
                return RedirectToAction("NotFound", "Error");


            News model = new News
            {
                message = msgText,
                title = msgTitle,
                fromId = User.Identity.GetUserId(),
                isFromSystem = false,
                isForAll = false,
                isForGroup = true,
                isForUser = false,
                onlyAdmin = false,
                onlyStudent = false,
                onlyTeacher = false,
                isPinned = false,
                added = DateTime.Now,
                groupId = id,
                groupName = g.GroupName,

            };

            foreach (User u in g.GroupStudents)
            {
                model.recipientList.Add(u);
                u.UserNews.Add(model);


            }


            db.News.Add(model);
            db.SaveChanges();
            return RedirectToAction("News", "Admin");
        }



        // Страница файлов и заданий
        public ActionResult Themes()
        {
            return View();
        }

        // Список всех тем (групп и тем для них)
        [ChildActionOnly]
        public ActionResult AllThemes()
        {
            List<Group> groups = db.Groups.ToList();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            return PartialView(groups);
        }

        // Поиск темы или группы 
        [HttpPost]
        public ActionResult SearchTheme(string fragment)
        {

            fragment = fragment.ToUpper();

            List<Group> groups = new List<Group>();


            foreach (Group g in db.Groups.ToList())
            {
                if (g.GroupName.ToUpper().Contains(fragment) || fragment.Contains(g.GroupName.ToUpper()))
                {
                    groups.Add(g);
                }
                else
                {
                    bool isAdded = false;
                    foreach (Theme th in g.ThemeList)
                    {
                        if (isAdded)
                            break;

                        if (th.themeName.ToUpper().Contains(fragment) || fragment.Contains(th.themeName.ToUpper()))
                        {
                            groups.Add(g);
                            isAdded = true;
                        }

                    }
                }



            }
            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            return PartialView(groups);
        }

        // Список файлов и заданий в теме 
        [HttpGet]
        public ActionResult GetTheme(int Id)
        {
            Theme th = db.Themes.Find(Id);
            if (th == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            Group g = db.Groups.Find(th.groupId);
            if (g == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            List<Assignment> AudioFiles = th.AssignmentList.Where(a => a.type == "Audio").ToList();
            List<Assignment> VideoFiles = th.AssignmentList.Where(a => a.type == "Video").ToList();
            List<Assignment> DownloadFiles = th.AssignmentList.Where(a => a.type == "Download").ToList();
            List<Assignment> LinkFiles = th.AssignmentList.Where(a => a.type == "Link").ToList();
            List<Assignment> PictureFiles = th.AssignmentList.Where(a => a.type == "Picture").ToList();

            int fileCount = AudioFiles.Count() + VideoFiles.Count() + DownloadFiles.Count() + LinkFiles.Count() + PictureFiles.Count();
            ViewBag.AudioFiles = AudioFiles;
            ViewBag.VideoFiles = VideoFiles;
            ViewBag.DownloadFiles = DownloadFiles;
            ViewBag.LinkFiles = LinkFiles;
            ViewBag.PictureFiles = PictureFiles;

            ViewBag.AudioCount = AudioFiles.Count();
            ViewBag.VideoCount = VideoFiles.Count();
            ViewBag.DownloadCount = DownloadFiles.Count();
            ViewBag.LinkCount = LinkFiles.Count();
            ViewBag.PictureCount = PictureFiles.Count();

            ViewBag.FileCount = fileCount;
            ViewBag.GroupName = g.GroupName;
            return View(th);
        }

        // Скачивание любого файла 
        public FileResult GetFile(string AssignmentFileName, string expansion, string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/th_files/" + AssignmentFileName + expansion));
            string name = fileName.Replace(' ', '-') + expansion;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }

        // Добавление файла в тему (GET)
        [ChildActionOnly]
        public ActionResult AddFile(int Id)
        {
            ViewBag.themeId = Id;
            return PartialView();
        }

        // Добавление файла в тему(POST)
        [HttpPost]
        public ActionResult AddFile(int themeId, string name, string type, string task, string link, HttpPostedFileBase file)
        {
            Theme th = db.Themes.Find(themeId);

            if (th == null)
                return RedirectToAction("Error500");

            Assignment model = new Assignment
            {
                type = type,
                AssignmentTask = task,
                theme = th,
                themeId = th.Id,
                link = (type == "Link") ? link : "none",
                fileName = (type != "Link") ? name : "none",
                added = DateTime.Now,
            };
            if (type == "Link")
            {
                model.AssignmentFileName = "none";
                model.AssignmentFileExpansion = "none";
            }
            else
            {

                if (file == null)
                    return RedirectToAction("Error500", "Error");
                else
                {
                    // Разрешены любые форматы файлов 
                    // получаем имя файла
                    string extension = Path.GetExtension(file.FileName);
                    string now = DateTime.Now.ToString().Replace(':', '-');
                    now = now.Replace("\\", "-");
                    now = now.Replace(" ", "-");
                    now = now.Replace(".", "-");
                    string fileName = "theme" + th.Id + "-type" + type.ToUpper() + "-date" + now;


                    //string fileName = type + "_" + file.FileName + "-Date" + now + "-Theme" + th.Id;
                    // сохраняем файл в папку Files в проекте
                    file.SaveAs(Server.MapPath("~/th_files/") + fileName + extension);

                    model.AssignmentFileExpansion = extension;
                    model.AssignmentFileName = fileName;

                }

            }

            th.AssignmentList.Add(model);
            db.SaveChanges();
            return RedirectToActionPermanent("GetTheme", "Admin", new { Id = themeId.ToString() });

        }

        // Удаление файла
        [HttpGet]
        public ActionResult DeleteFile(int Id)
        {

            Assignment file = db.Assignments.Find(Id);

            if (file == null)
                return RedirectToAction("NotFound", "Error");

            Theme th = file.theme;

            if (th == null)
                th = db.Themes.Find(file.themeId);

            if (th == null)
                return RedirectToAction("NotFound", "Error");

            th.AssignmentList.Remove(file);

            if (file.type != "Link")

                System.IO.File.Delete(Server.MapPath("~/th_files/" + file.AssignmentFileName + file.AssignmentFileExpansion));

            db.Assignments.Remove(file);
            db.SaveChanges();
            return RedirectToActionPermanent("GetTheme", "Admin", new { Id = th.Id.ToString() });


        }

        // Добавление темы для группы (GET)
        [ChildActionOnly]
        public ActionResult AddTheme()
        {
            return PartialView();
        }
        // Добавление темы для группы (POST)
        [HttpPost]
        public ActionResult AddTheme(string themeName, string groupId, string color, string returnUrl)
        {
            Group group = db.Groups.Find(Convert.ToInt32(groupId));
            if (group == null)
                return RedirectToAction("Error500", "Error");

            // Цвет по умолчанию
            if (color != "Blue" && color != "Pink" && color != "Green" && color != "Purpure")
                color = "Blue";
            Theme theme = new Theme
            {
                colorTheme = color,
                themeName = themeName,
                added = DateTime.Now,
                group = group,
                groupId = Convert.ToInt32(groupId),
            };
            group.ThemeList.Add(theme);
            db.Themes.Add(theme);
            db.SaveChanges();

            if (returnUrl == "Theme")
                return RedirectToAction("Themes");

            return RedirectToAction("Index");
        }
    }
}