using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrSimonAcademy2.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    public class FeedbackController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Feedback
        [HttpGet]
        public ActionResult AddFeedbackForUser()
        {
            //User user = db.Users.Find(userId);
            //if (user.RoleName != "Student") 
            //{
            //    return RedirectToAction("Index", "Admin");
            //}
            //int speaking = 0;
            //int listening = 0;
            //int reading = 0;
            //int writing = 0;
            //if (user.hasFeedback) {
            //    var userFeeds = db.Feedbacks.Where(f => f.UserId == userId).ToList();
            //    List<Rating> ratingList = userFeeds[userFeeds.Count - 1].RatingList.ToList<Rating>();

            
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

            //}

            //ViewBag.speaking = speaking;
            //ViewBag.listening = listening;
            //ViewBag.reading = reading;
            //ViewBag.writing = writing;

            //List<Group> groups = user.UserGroups.ToList<Group>();
            //ViewBag.id = userId;
            //ViewBag.user = user.UserFName +" " +user.UserLName;
            //if (groups.Count !=0 )
            //    ViewBag.groupName = groups[0].GroupName;

            return View();
        }

        // GET: Feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeedbackForUser(Feedback model, string speaking, string writing, string reading, string listening)
        {
            //User user = db.Users.Find(model.UserId);
            //List<Rating> rating = new List<Rating>();

            //rating.Add(new Rating("Speaking", Convert.ToInt32(speaking)));
            //rating.Add(new Rating("Writing", Convert.ToInt32(writing)));
            //rating.Add(new Rating("Reading", Convert.ToInt32(reading)));
            //rating.Add(new Rating("Listening", Convert.ToInt32(listening)));

            //model.RatingList = rating;

            //user.FeedbackList.Add(model);
            //user.hasFeedback = true;
            //db.Feedbacks.Add(model);
            //db.SaveChanges();
            return RedirectToAction("Users", "Admin");
        }
    }
}