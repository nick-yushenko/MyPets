using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrSimonAcademy2.Controllers
{
    [HandleError]
    public class ErrorController : Controller
    {
        public ActionResult Error400()
        {
            //Response.StatusCode = 400;
            return RedirectToAction("NotFound");
        }
        public ActionResult Error401()
        {
            //Response.StatusCode = 401;
            return RedirectToAction("NotFound");
        }
        public ActionResult Error402()
        {
            //Response.StatusCode = 402;
            return RedirectToAction("NotFound");
        }
        public ActionResult Error403()
        {
            //Response.StatusCode = 403;
            ViewBag.Title = "Opps... Похоже это очень важная страница";
            ViewBag.Subtitle = "Даже у Саймона нет к ней доступа";
            ViewBag.Primary = "Ну, ничего, зато у него есть косточка!";
            return View();
        }

        public ActionResult Error500()
        {
           // Response.StatusCode = 500;
            ViewBag.Title = "Opps... Похоже это очень важная страница";
            ViewBag.Subtitle = "Даже у Саймона нет к ней доступа";
            ViewBag.Primary = "Ну, ничего, зато у него есть косточка!";
            return View();
        }

        public ActionResult Error503()
        {
            //Response.StatusCode = 503;
            ViewBag.Title = "Opps... Саймон сильно устал";
            ViewBag.Subtitle = "Не волнуйтесь, Саймон догрызет косточку и вернется к работе";
            return RedirectToAction("NotFound");
        }

        public ActionResult NotFound()
        {
            ViewBag.Title =  "Opps... Похоже Саймон сгрыз Вашу страничку";
            ViewBag.Subtitle = "Не волнуйтесь, теперь у Саймона есть косточка, так что скоро все появится ";
           // Response.StatusCode = 404;
            return View();
        }
    }
}