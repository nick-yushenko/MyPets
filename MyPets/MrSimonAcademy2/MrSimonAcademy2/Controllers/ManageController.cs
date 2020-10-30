using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MrSimonAcademy2.Controllers
{
    // Данный контроллер доступен только для авторизованных пользователей
    //[Authorize]
    public class ManageController : Controller
    {
        // Менеджер пользователей для взаимодействия с конкретным пользователем
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // Конструктор
        public ManageController()
        {
        }

        // Конструктор по менеджеру пользователей
        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        // Получение менеджера пользователей
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

        // Блок с именем в шапке
        [ChildActionOnly]
        public ActionResult HeaderProfile(string userId)
        {
            var user = db.Users.Find(userId);

            if (user != null)
            {
                ViewBag.RoleName = user.RoleName;
                ViewBag.UserName = user.UserFName + " " + user.UserLName;
                ViewBag.withoutAvatar = user.withoutAvatar;
                ViewBag.avatarName = user.avatarName;
            }

            return PartialView();
        }

        // Блок с именем в шапке
        [ChildActionOnly]
        public ActionResult MenuProfile(string userId)
        {
            var user = db.Users.Find(userId);

            if (user != null)
            {
                ViewBag.RoleName = user.RoleName;
                ViewBag.UserName = user.UserFName + " " + user.UserLName;
                ViewBag.withoutAvatar = user.withoutAvatar;
                ViewBag.avatarName = user.avatarName;
            }

            return PartialView();
        }

        public ActionResult Statistic(string userId)
        {
            // Если статистику пытается открыть не авторизованный пользователь
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Если это не статистика текущего пользователя
            if (User.Identity.GetUserId() != userId)
            {
                User u = db.Users.Find(User.Identity.GetUserId());

                // Если статистику пытается посмотреть другой студент
                if (u.RoleName == "Student")
                {
                    return RedirectToAction("Login", "Account");
                }
                // Если статистику пытается посмотреть некий учитель
                if (u.RoleName == "Teacher")
                {

                    // Разрешен ли доступ для учителя к этому студенту
                    bool result = false;

                    // Перебор всех групп (и студентов в них) для данного учителя, чтобы проверить, обучается ли нужный студент у этого учителя
                    foreach (Group g in u.UserGroups)
                    {
                        List<User> sts = g.GroupStudents.Where(st => st.Id == userId).ToList();

                        if (sts.Count() != 0)
                            result = true;
                    }

                    if (!result)
                        return RedirectToAction("Login", "Account");
                }

            }
            bool isMyProfile = false;
            if (userId == null || userId == User.Identity.GetUserId())
            {
                userId = User.Identity.GetUserId();
                isMyProfile = true;
            }
            ViewBag.isMyProfile = isMyProfile;
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var model = new IndexViewModel
            {
                Id = userId,
                //UserFName = user.UserFName,
                //UserLName = user.UserLName,
                //Birthday = user.Birthday,
                //Email = user.Email,
                RoleName = user.RoleName,
                HasFeedback = user.hasFeedback,
                LevelName = user.levelName,

            };
            ViewBag.hasFeedback = user.hasFeedback;

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Attendance(string userId)
        {
            User user = db.Users.Find(userId);
            if (user.hasFeedback)
            {
                List<Feedback> feedbacks = user.FeedbackList.ToList<Feedback>();
                //List<Rating> ratingList = feedback[feedback.Count - 1].RatingList.ToList<Rating>();

                int totalAttendance = 0;
                int missedAttendance = 0;


                foreach (Feedback f in feedbacks)
                {
                    totalAttendance += f.LessonsCount;
                    missedAttendance += f.LoseLessons;
                }
                if (totalAttendance != 0)
                    ViewBag.hasAttendance = true;
                else
                    ViewBag.hasAttendance = false;


                ViewBag.total = totalAttendance;
                ViewBag.visited = totalAttendance - missedAttendance;
                ViewBag.percentEmoji = ((totalAttendance - missedAttendance) * 1.0) / totalAttendance * 100;

            }
            else
            {
                ViewBag.hasAttendance = false;
            }

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Homework(string userId)
        {
            User user = db.Users.Find(userId);
            if (user.hasFeedback)
            {
                List<Feedback> feedbacks = user.FeedbackList.ToList<Feedback>();
                //List<Rating> ratingList = feedback[feedback.Count - 1].RatingList.ToList<Rating>();

                int totalHomework = 0;
                int passedHomework = 0;


                foreach (Feedback f in feedbacks)
                {
                    totalHomework += f.HomeworkCount;
                    passedHomework += f.PassedHomework;
                }
                if (totalHomework != 0)
                    ViewBag.hasHomework = true;
                else
                    ViewBag.hasHomework = false;

                ViewBag.total = totalHomework;
                ViewBag.passed = passedHomework;
                ViewBag.percentEmoji = passedHomework * 1.0 / totalHomework * 100;
            }
            else
            {
                ViewBag.hasHomework = false;
            }
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Activity(string userId)
        {
            User user = db.Users.Find(userId);
            if (user.hasFeedback)
            {
                List<Feedback> feedbacks = user.FeedbackList.ToList<Feedback>();
                //List<Rating> ratingList = feedback[feedback.Count - 1].RatingList.ToList<Rating>();

                int activity = -1;
                //int activityCount = 0;
                int concentration = -1;
                //int concentrationCount = 0;


                for (int i = feedbacks.Count - 1; i >= 0; i--)
                {
                    if (activity == -1)
                        activity += feedbacks[i].Activity;
                    if (concentration == -1)
                        concentration += feedbacks[i].Concentration;
                    if (activity != -1 && concentration != -1)
                        break;
                }
                //foreach (Feedback f in feedbacks)
                //{
                //    activity += f.Activity;
                //    concentration += f.Concentration;
                //    //if (f.Activity != 0)
                //    //    activityCount++;
                //    //if (f.Concentration != 0)
                //    //    concentrationCount++;
                //}


                if (activity == -1 && concentration == -1)
                    ViewBag.hasActivity = false;
                else
                    ViewBag.hasActivity = true;

                activity++;
                //activity = activity / activityCount;
                concentration++;
                //concentration = concentration / concentrationCount;


                if (concentration == 0)
                    concentration = 5;
                ViewBag.Activity = activity;
                ViewBag.Concentration = concentration;
            }
            else
            {
                ViewBag.hasActivity = false;
            }
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Hours(string userId)
        {
            ViewBag.hasHours = false;
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Adventures(string userId)
        {
            User user = db.Users.Find(userId);
            if (user.hasFeedback)
            {
                List<Feedback> feedbacks = user.FeedbackList.ToList<Feedback>();

                string minus = "";
                string plus = "";

                foreach (Feedback f in feedbacks)
                {
                    if (f.AdventuresMinus != "" && f.AdventuresMinus != null)
                        minus = f.AdventuresMinus;
                    if (f.AdventuresPlus != "" && f.AdventuresPlus != null)
                        plus = f.AdventuresPlus;
                }

                ViewBag.Minus = minus;
                ViewBag.Plus = plus;
            }
            return PartialView();
        }



        [ChildActionOnly]
        public ActionResult Skills(string userId)
        {
            User user = db.Users.Find(userId);
            if (user.hasFeedback)
            {
                List<Feedback> feedbacks = user.FeedbackList.ToList<Feedback>();
                //List<Rating> ratingList = feedbacks[feedbacks.Count - 1].RatingList.ToList<Rating>();

                string speaking = "0";
                string listening = "0";
                string reading = "0";
                string writing = "0";

                foreach (Feedback f in feedbacks)
                {

                    List<Rating> ratings = f.RatingList.ToList();
                    foreach (Rating rating in ratings)
                    {
                        if (rating.AspectName == "Speaking" && rating.Value != 0)
                            speaking = rating.Value.ToString();
                        if (rating.AspectName == "Listening" && rating.Value != 0)
                            listening = rating.Value.ToString();
                        if (rating.AspectName == "Reading" && rating.Value != 0)
                            reading = rating.Value.ToString();
                        if (rating.AspectName == "Writing" && rating.Value != 0)
                            writing = rating.Value.ToString();
                    }
                }

                if (writing == "0" && speaking == "0" && listening == "0" && reading == "0")
                    ViewBag.hasSkills = false;
                else
                    ViewBag.hasSkills = true;



                //foreach (Rating rating in ratingList)
                //{
                //    if (rating.AspectName == "Speaking")
                //        speaking = rating.Value.ToString();
                //    if (rating.AspectName == "Listening")
                //        listening = rating.Value.ToString();
                //    if (rating.AspectName == "Reading")
                //        reading = rating.Value.ToString();
                //    if (rating.AspectName == "Writing")
                //        writing = rating.Value.ToString();
                //}

                ViewBag.Speaking = speaking;
                ViewBag.Listening = listening;
                ViewBag.Reading = reading;
                ViewBag.Writing = writing;
                ViewBag.Level = user.levelName;
            }
            else
            {
                ViewBag.hasSkills = false;
            }



            //if (user.levelName.ToUpper() == "STARTER")
            //    ViewBag.NextLevelName = "Beginner";
            //if (user.levelName.ToUpper() == "BEGINNER")
            //    ViewBag.NextLevelName = "Elementary";
            //if (user.levelName.ToUpper() == "ELEMENTARY")
            //    ViewBag.NextLevelName = "Pre-intermediate";
            //if (user.levelName.ToUpper() == "PRE-INTERMEDIATE")
            //    ViewBag.NextLevelName = "Intermediate";
            //if (user.levelName.ToUpper() == "INTERMEDIATE")
            //    ViewBag.NextLevelName = "Upper-intermediate";
            //if (user.levelName.ToUpper() == "UPPER-INTERMEDIATE")
            //    ViewBag.NextLevelName = "Advanced";
            //if (user.levelName.ToUpper() == "ADVANCED")
            //    ViewBag.NextLevelName = "Proficiency";
            //if (user.levelName.ToUpper() == "PROFICIENCY")
            //    ViewBag.NextLevelName = "null";

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Level(string userId)
        {
            User user = db.Users.Find(userId);
            if (user.hasFeedback)
            {
                List<Feedback> feedbacks = user.FeedbackList.ToList<Feedback>();
                //List<Rating> ratingList = feedbacks[feedbacks.Count - 1].RatingList.ToList<Rating>();

                int speaking = 0;
                int listening = 0;
                int reading = 0;
                int writing = 0;

                int lastSpeaking = 0;
                int lastListening = 0;
                int lastReading = 0;
                int lastWriting = 0;
                DateTime lastPointDate = new DateTime();

                foreach (Feedback f in feedbacks)
                {

                    List<Rating> ratings = f.RatingList.ToList<Rating>();
                    foreach (Rating rating in ratings)
                    {
                        if (rating.AspectName == "Speaking" && rating.Value != 0)
                        {
                            if (speaking != 0)
                            {
                                lastSpeaking = speaking;
                            }
                            speaking = (rating.Value == 0) ? rating.Value + 1 : rating.Value;
                        }
                        if (rating.AspectName == "Listening" && rating.Value != 0)
                        {
                            if (listening != 0)
                            {
                                lastListening = listening;
                            }
                            listening = (rating.Value == 0) ? rating.Value + 1 : rating.Value;
                        }
                        if (rating.AspectName == "Reading" && rating.Value != 0)
                        {
                            if (reading != 0)
                            {
                                lastReading = reading;
                            }
                            reading = (rating.Value == 0) ? rating.Value + 1 : rating.Value;
                        }
                        if (rating.AspectName == "Writing" && rating.Value != 0)
                        {
                            if (writing != 0)
                            {
                                lastWriting = writing;
                            }
                            writing = (rating.Value == 0) ? rating.Value + 1 : rating.Value;
                        }

                        if (lastSpeaking != 0 && lastReading != 0 && lastWriting != 0 && lastListening != 0)
                            lastPointDate = f.AddingData;
                    }
                }

                //if (writing == "0" && speaking == "0" && listening == "0" && reading == "0")
                //    ViewBag.hasLevel = false;
                //else
                //    ViewBag.hasLevel = true;

                string currentLevel = user.levelName;

                int levelPossition = (speaking + listening + reading + writing) / 4;
                int lastLevelPossition = (lastSpeaking + lastListening + lastReading + lastWriting) / 4;

                ViewBag.Level = currentLevel;
                ViewBag.Pos = levelPossition;
                ViewBag.lastPos = lastLevelPossition;
                ViewBag.lastPosDate = lastPointDate;





            }
            else
            {
                ViewBag.hasLevel = false;
            }



            //if (user.levelName.ToUpper() == "STARTER")
            //    ViewBag.NextLevelName = "Beginner";
            //if (user.levelName.ToUpper() == "BEGINNER")
            //    ViewBag.NextLevelName = "Elementary";
            //if (user.levelName.ToUpper() == "ELEMENTARY")
            //    ViewBag.NextLevelName = "Pre-intermediate";
            //if (user.levelName.ToUpper() == "PRE-INTERMEDIATE")
            //    ViewBag.NextLevelName = "Intermediate";
            //if (user.levelName.ToUpper() == "INTERMEDIATE")
            //    ViewBag.NextLevelName = "Upper-intermediate";
            //if (user.levelName.ToUpper() == "UPPER-INTERMEDIATE")
            //    ViewBag.NextLevelName = "Advanced";
            //if (user.levelName.ToUpper() == "ADVANCED")
            //    ViewBag.NextLevelName = "Proficiency";
            //if (user.levelName.ToUpper() == "PROFICIENCY")
            //    ViewBag.NextLevelName = "null";

            return PartialView();
        }



        [ChildActionOnly]
        public ActionResult MyGroups(string userId)
        {
            User user = db.Users.Find(userId);
            List<Group> groups = user.UserGroups.ToList<Group>();
            List<string> teachers = new List<string>();
            foreach (var g in groups)
            {
                teachers.Add(db.Users.Find(g.GroupTeacherId).UserFName + " " + db.Users.Find(g.GroupTeacherId).UserLName);

            }
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.Teachers = teachers;
            return PartialView(groups);
        }



        //[ChildActionOnly]
        //public ActionResult GetFeedbackForLevel(string userId)
        //{
        //    User user = db.Users.Find(userId);
        //    List<Feedback> feedback = user.FeedbackList.ToList<Feedback>();
        //    List<Rating> ratingList = feedback[feedback.Count - 1].RatingList.ToList<Rating>();

        //    int speaking = 4;
        //    int listening = 4;
        //    int reading = 4;
        //    int writing = 4;
        //    foreach (Rating rating in ratingList)
        //    {
        //        if (rating.AspectName == "Speaking")
        //            speaking = rating.Value;
        //        if (rating.AspectName == "Listening")
        //            listening = rating.Value;
        //        if (rating.AspectName == "Reading")
        //            reading = rating.Value;
        //        if (rating.AspectName == "Writing")
        //            writing = rating.Value;
        //    }

        //    int progress = (speaking + listening + reading + writing) / 4;
        //    progress = (progress > 90) ? progress - 10 : progress;
        //    progress = (progress < 10) ? progress + 10 : progress;
        //    ViewBag.LevelName = user.levelName;
        //    ViewBag.LevelProgress = progress;
        //    return PartialView();
        //}


        //[HttpPost]
        //public ActionResult AddDeclarationForGroup(string groupId, string declaration)
        //{
        //    int id = Convert.ToInt32(groupId);
        //    Group group = db.Groups.Find(id);
        //    group.declaration = declaration;
        //    UpdateModel(group);
        //    db.SaveChanges();
        //    ViewBag.groupId = id;
        //    return PartialView(group);
        //}

        //public async Task<ActionResult> Edit(string id)
        //{
        //    if (id == User.Identity.GetUserId())
        //    {
        //        User user = await UserManager.FindByIdAsync(id);
        //        if (user != null)
        //        {
        //            EditModel model = new EditModel { UserFName = user.UserFName, UserLName = user.UserLName, Email = user.Email };
        //            return View(model);
        //        }
        //    }
        //    return RedirectToAction("Login", "Account");
        //}
        //[HttpPost]
        //public async Task<ActionResult> Edit(EditModel model, string id)
        //{
        //    if (id == User.Identity.GetUserId())
        //    {
        //        User user = await UserManager.FindByIdAsync(id);
        //        if (user != null)
        //        {
        //            user.Email = model.Email;
        //            user.UserName = model.Email;
        //            IdentityResult result = await UserManager.UpdateAsync(user);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Manage");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Что-то пошло не так");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Пользователь не найден");
        //        }

        //        return View(model);
        //    }
        //    return RedirectToAction("Login", "Account");

        //}

    }
}