using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MrSimonAcademy2.Models;

namespace MrSimonAcademy2.Controllers
{
    [Authorize(Roles = "Teacher")]
    //[Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult News()
        {
            NewsComparer nc = new NewsComparer();

            string userId = User.Identity.GetUserId();
            User user = db.Users.Find(userId);
            List<News> news = user.UserNews.ToList();

            news.Sort(nc);


            return PartialView(news);

            //return View(news);
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
            return RedirectToAction("Groups", "Teacher");
        }


        // Добавление сообщения студенту (GET)
        public ActionResult AddMessage(string id)
        {
            User u = db.Users.Find(id);
            if (u == null)
                return RedirectToAction("NotFound", "Error");

            return PartialView(u);
        }

        // Добавление сообщения студенту (POST)
        [HttpPost]
        public ActionResult AddMessage(string id, string msgText, string msgTitle)
        {
            User u = db.Users.Find(id);
            if (u == null)
                return RedirectToAction("NotFound", "Error");

            News model = new News
            {
                message = msgText,
                title = msgTitle,
                fromId = User.Identity.GetUserId(),
                isFromSystem = false,
                isForAll = false,
                isForGroup = false,
                isForUser = true,
                onlyAdmin = false,
                onlyStudent = false,
                onlyTeacher = false,
                isPinned = false,
                added = DateTime.Now,
                groupId = 0,
                groupName = "none",

            };

            model.recipientList.Add(u);
            u.UserNews.Add(model);

            // Добавление данного сообщения для учителя
            string userId = User.Identity.GetUserId();
            User teacher = db.Users.Find(userId);
            teacher.UserNews.Add(model);
            db.News.Add(model);
            db.SaveChanges();
            return RedirectToAction("Users", "Teacher");
        }

        // Поиск пользователей 
        [HttpPost]
        public ActionResult SearchMessages(string fragment)
        {
            fragment = fragment.ToUpper();

            NewsComparer nc = new NewsComparer();

            string userId = User.Identity.GetUserId();
            User user = db.Users.Find(userId);
            List<News> news = new List<News>();

            foreach (News n in user.UserNews.ToList())
            {
                if (fragment.Contains(n.title.ToUpper()) || fragment.Contains(n.message.ToUpper()) || n.message.ToUpper().Contains(fragment)
                            || n.title.ToUpper().Contains(fragment))
                    news.Add(n);
            }

            news.Sort(nc);


            return PartialView(news);
        }

        public ActionResult Users()
        {
            List<User> students = new List<User>();

            foreach (User u in db.Users.ToList())
            {
                foreach (Group g in u.UserGroups.ToList())
                {
                    if (u.RoleName == "Student")
                    {
                        // Проверка обучается ли студент u у преподавателя
                        if (g.GroupTeacherId == User.Identity.GetUserId())
                        {
                            students.Add(u);
                        }
                    }
                }
            }
            UserComparer uc = new UserComparer();
            students.Sort(uc);

            return View(students);
        }

        [HttpPost]
        public ActionResult SearchUser(string name)
        {
            List<User> students = new List<User>();

            var teacherId = User.Identity.GetUserId();
            name = name.ToUpper();

            foreach (User u in db.Users.ToList())
            {
                foreach (Group g in u.UserGroups.ToList())
                {
                    if (u.RoleName == "Student")
                    {
                        // Проверка обучается ли студент u у преподавателя
                        if (g.GroupTeacherId == teacherId)
                        {
                            // проверка, подходит ли этот студент к выборке 
                            if ((name.Contains(u.UserFName.ToUpper()) || name.Contains(u.UserLName.ToUpper()) || u.UserFName.ToUpper().Contains(name) || u.UserLName.ToUpper().Contains(name)))
                                students.Add(u);
                        }
                    }
                }
            }
            UserComparer uc = new UserComparer();
            students.Sort(uc);


            return PartialView(students);
        }

        public ActionResult Groups()
        {
            List<Group> groups = new List<Group>();

            List<int> fileCounts = new List<int>();
            List<Theme> lastThemes = new List<Theme>();
            List<DateTime> lastFiles = new List<DateTime>();
            List<Assignment> tempFileList = new List<Assignment>();


            User teacher = db.Users.Find(User.Identity.GetUserId());

            groups = teacher.UserGroups.ToList();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            foreach (Group g in groups)
            {
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
            ViewBag.lastThemes = lastThemes;
            ViewBag.lastFiles = lastFiles;
            ViewBag.FileCounts = fileCounts;
            return View(groups);
        }

        [HttpPost]
        public ActionResult SearchGroup(string name)
        {

            name = name.ToUpper();
            List<Group> groups = new List<Group>();

            List<int> fileCounts = new List<int>();
            List<Theme> lastThemes = new List<Theme>();
            List<DateTime> lastFiles = new List<DateTime>();
            List<Assignment> tempFileList = new List<Assignment>();


            User teacher = db.Users.Find(User.Identity.GetUserId());

            groups = teacher.UserGroups.Where(g => (g.GroupName.ToUpper().Contains(name) || name.Contains(g.GroupName.ToUpper()))).ToList();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            foreach (Group g in groups)
            {
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
            ViewBag.lastThemes = lastThemes;
            ViewBag.lastFiles = lastFiles;
            ViewBag.FileCounts = fileCounts;

            return PartialView(groups);
        }

        [ChildActionOnly]
        public ActionResult AddStatistic()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult AddStatistic(string userId, string returnUrl, int lessonsCount = 0, int loseLessons = 0, int homeworkCount = 0, int passedHomework = 0,
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
            if (returnUrl == "Users")
                return RedirectToAction("Users");
            if (returnUrl == "Groups")
                return RedirectToAction("Groups");

            return RedirectToAction("Users");

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
            User teacher = db.Users.Find(User.Identity.GetUserId());

            if (teacher == null)
                return RedirectToAction("NotFound", "Error");

            List<Group> groups = teacher.UserGroups.ToList();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);

            return PartialView(groups);
        }

        // Поиск темы или группы 
        [HttpPost]
        public ActionResult SearchTheme(string fragment)
        {

            User teacher = db.Users.Find(User.Identity.GetUserId());

            if (teacher == null)
                return RedirectToAction("NotFound", "Error");


            fragment = fragment.ToUpper();

            List<Group> groups = new List<Group>();


            foreach (Group g in teacher.UserGroups.ToList())
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

        // Список файлов и заданий в теме 
        [HttpGet]
        public ActionResult GetTheme(int Id)
        {

            User teacher = db.Users.Find(User.Identity.GetUserId());

            if (teacher == null)
                return RedirectToAction("NotFound", "Error");

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
            if (!teacher.UserGroups.Contains(g))
            {
                return RedirectToAction("Error403", "Error");
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
        [AllowAnonymous]
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
            return RedirectToActionPermanent("GetTheme", "Teacher", new { Id = themeId.ToString() });

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
            return RedirectToActionPermanent("GetTheme", "Teacher", new { Id = th.Id.ToString() });


        }


        // Поиск темы или группы 
        [HttpPost]
        public ActionResult SearchFile(string fragment, int Id)
        {

            fragment = fragment.ToUpper();

            User teacher = db.Users.Find(User.Identity.GetUserId());

            if (teacher == null)
                return RedirectToAction("NotFound", "Error");

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
            if (!teacher.UserGroups.Contains(g))
            {
                return RedirectToAction("Error403", "Error");
            }

            List<Assignment> AudioFiles = th.AssignmentList.Where(a =>
                (
                    a.type == "Audio" &&
                    (
                        a.fileName.ToUpper().Contains(fragment) || fragment.Contains(a.fileName.ToUpper()) ||
                        a.AssignmentTask.ToUpper().Contains(fragment) || fragment.Contains(a.AssignmentTask.ToUpper())
                    )
                )).ToList();

            List<Assignment> VideoFiles = th.AssignmentList.Where(a =>
                (
                    a.type == "Video" &&
                    (
                        a.fileName.ToUpper().Contains(fragment) || fragment.Contains(a.fileName.ToUpper()) ||
                        a.AssignmentTask.ToUpper().Contains(fragment) || fragment.Contains(a.AssignmentTask.ToUpper())
                    )
                )).ToList();

            List<Assignment> DownloadFiles = th.AssignmentList.Where(a =>
                (
                    a.type == "Download" &&
                    (
                        a.fileName.ToUpper().Contains(fragment) || fragment.Contains(a.fileName.ToUpper()) ||
                        a.AssignmentTask.ToUpper().Contains(fragment) || fragment.Contains(a.AssignmentTask.ToUpper())
                    )
                )).ToList();

            List<Assignment> LinkFiles = th.AssignmentList.Where(a =>
                (
                    a.type == "Link" &&
                    (
                        a.fileName.ToUpper().Contains(fragment) || fragment.Contains(a.fileName.ToUpper()) ||
                        a.AssignmentTask.ToUpper().Contains(fragment) || fragment.Contains(a.AssignmentTask.ToUpper())
                    )
                )).ToList();

            List<Assignment> PictureFiles = th.AssignmentList.Where(a =>
                (
                    a.type == "Picture" &&
                    (
                        a.fileName.ToUpper().Contains(fragment) || fragment.Contains(a.fileName.ToUpper()) ||
                        a.AssignmentTask.ToUpper().Contains(fragment) || fragment.Contains(a.AssignmentTask.ToUpper())
                    )
                )).ToList();


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

            return PartialView(th);
        }

    }
}
