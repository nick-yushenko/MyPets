using Microsoft.AspNet.Identity;
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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private Service service = new Service();

        [ChildActionOnly]
        public ActionResult AcceptCookie() 
        {
            // Проверка: было ли дано согласие на обработку файлов Cookies 
            HttpCookie AcceptCookie = Request.Cookies["Accept"];
            if (AcceptCookie == null)
                ViewBag.Accepted = false;
            else
                ViewBag.Accepted = true;
            return PartialView();
        }

        [HttpPost]
        public ActionResult SetCookieAccept()
        {
            // Установка файлов cookie
            HttpContext.Response.Cookies["Accept"].Value = "true";
            HttpContext.Response.Cookies["Accept"].Expires = DateTime.Now.AddMonths(12);
            // для того, чтобы была плавность
            ViewBag.Accepted = false;
            return PartialView("AcceptCookie");
        }


        public ActionResult Index()
        {
            return View();
        }



        [ChildActionOnly]
        public ActionResult Simon_block1()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult About_block2()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Methods_block3()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Founder_block4()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Family_block5()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Steps_block6()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Courses_block7()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Flamp_block8()
        {
            return PartialView();
        }

        // Форма связи 
        [ChildActionOnly]
        public ActionResult Brif_block9()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Brif_block9(ContactViewModel model, List<string> language, string phone)
        {
            if (ModelState.IsValid)
            {
                model.CourseName = "Не выбран";
                model.Phone = phone;
                if (language.Count == 1)
                    model.Language = language[0].ToString();

                if (language.Count == 2)
                    model.Language = language[0].ToString() + ", " + language[1].ToString();


                service.SendEmail(model);

                return Redirect("Index");
            }
            return RedirectToAction("Error500", "Error");
            // TODO: Переделать на Ajax
        }

        // Модальное окно с формой связи
        [ChildActionOnly]
        public ActionResult BrifModal()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult BrifModal(ContactViewModel model, List<string> language, string phone, string course)
        {
            if (ModelState.IsValid)
            {
                model.CourseName = course;
                model.Phone = phone;
                if (language.Count == 1)
                    model.Language = language[0].ToString();

                if (language.Count == 2)
                    model.Language = language[0].ToString() + ", " + language[1].ToString();


                service.SendEmail(model);

                return Redirect("Index");
            }
            return RedirectToAction("Error500", "Error");

            // TODO: Переделать на Ajax
        }

        // Модальное окно политики
        [ChildActionOnly]
        public ActionResult PrivacyModal()
        {
            return PartialView();
        }

    }
}