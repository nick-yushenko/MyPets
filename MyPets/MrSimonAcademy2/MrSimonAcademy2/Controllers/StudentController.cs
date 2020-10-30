using Microsoft.AspNet.Identity;
using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrSimonAcademy2.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Student
        public ActionResult News()
        {

            string userId = User.Identity.GetUserId();
            User user = db.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Error500", "Error");

            List<News> news = user.UserNews.ToList();
            return View(news);
        }

        public ActionResult MyGroups(string userId)
        {
            User user = db.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Error500", "Error");
            else
            {

                List<string> teachers = new List<string>();

                List<Group> userGroups = user.UserGroups.ToList();

                foreach (Group g in userGroups)
                {
                    User teacher = db.Users.Find(g.GroupTeacherId);

                    if (teacher != null)
                    {
                        teachers.Add(teacher.UserFName + " " + teacher.UserLName);
                    }
                    else
                    {
                        teachers.Add("none");

                    }

                }

                ViewBag.Teachers = teachers;

                return View(userGroups);
            }
        }

        public ActionResult Themes()
        {
            string userId = User.Identity.GetUserId();
            User user = db.Users.Find(userId);

            if (user == null)
                return RedirectToAction("Error500", "Error");
            List<Group> groups = user.UserGroups.ToList();

            GroupComparer gc = new GroupComparer();
            groups.Sort(gc);
            return View(groups);


        }
        // Поиск темы или группы 
        [HttpPost]
        public ActionResult SearchTheme(string fragment)
        {

            string userId = User.Identity.GetUserId();
            User user = db.Users.Find(userId);

            if (user == null)
                return RedirectToAction("Error500", "Error");

            fragment = fragment.ToUpper();

            List<Group> groups = new List<Group>();


            foreach (Group g in user.UserGroups.ToList())
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


        public ActionResult GetTheme(int Id)
        {
            string userId = User.Identity.GetUserId();
            User user = db.Users.Find(userId);

            if (user == null)
                return RedirectToAction("Error500", "Error");

            Theme th = db.Themes.Find(Id);
            if (th == null)
                return RedirectToAction("Error500", "Error");

            Group g = th.group;

            if (g == null)
                g = db.Groups.Find(th.groupId);

            if (g == null)
                return RedirectToAction("Error500", "Error");

            List<Assignment> AudioFiles = th.AssignmentList.Where(a => a.type == "Audio").ToList();
            List<Assignment> DownloadFiles = th.AssignmentList.Where(a => a.type == "Download").ToList();
            List<Assignment> LinkFiles = th.AssignmentList.Where(a => a.type == "Link").ToList();



            int fileCount = AudioFiles.Count() + DownloadFiles.Count() + LinkFiles.Count();
            ViewBag.AudioFiles = AudioFiles;
            ViewBag.DownloadFiles = DownloadFiles;
            ViewBag.LinkFiles = LinkFiles;

            ViewBag.AudioCount = AudioFiles.Count();
            ViewBag.DownloadCount = DownloadFiles.Count();
            ViewBag.LinkCount = LinkFiles.Count();

            ViewBag.FileCount = fileCount;

            ViewBag.GroupName = g.GroupName;
            return View();
        }
    }
}