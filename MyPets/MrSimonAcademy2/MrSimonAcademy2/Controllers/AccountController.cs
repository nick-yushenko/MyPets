using ImageResizer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MrSimonAcademy2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly Service service = new Service();
        readonly ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManger;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
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


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        // Возвращает форму входа
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        // Проверяет введенные данные. Получает пользователя из БД 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {


                // поиск пользователя в БД
                User user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    if (user.isActive == false)
                    {
                        ModelState.AddModelError("", "Ваш аккаунт был заблокирован. Пожалуйста, обратитесь к администратору");
                    }
                    else
                    {
                        if (user.EmailConfirmed == true)
                        {
                            ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                               DefaultAuthenticationTypes.ApplicationCookie);
                            // Удаление ранее определенных куки в браузере
                            AuthenticationManager.SignOut();
                            // Добавление новых куки в браузере
                            AuthenticationManager.SignIn(new AuthenticationProperties
                            {
                                IsPersistent = true
                            }, claim);
                            if (user.RoleName == "Student")
                            {
                                ViewBag.userId = user.Id;
                                return RedirectToAction("News", "Student");
                            }
                            if (user.RoleName == "Admin")
                            {
                                ViewBag.userId = user.Id;

                                return RedirectToAction("Index", "Admin");
                            }
                            if (user.RoleName == "Teacher")
                            {
                                ViewBag.userId = user.Id;
                                return RedirectToAction("Users", "Teacher");

                            }
                            //if (String.IsNullOrEmpty(returnUrl))

                            //    return RedirectToAction("Index", "Home");
                            //return Redirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Чтобы войти в личный кабинет, необходимо подтвердить Ваш E-mail. Пожалуйста, перейдите по ссылке в письме, которое мы отправили Вам на почту");

                        }

                    }



                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }


        // Возвращает форму регистрации
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Register()
        {
            return PartialView();
        }

        // Получает данные нового пользователя создает по ним объект класса User и добавляет его в БД
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterModel model, string role, string level, string date, HttpPostedFileBase photo)
        {

            if (ModelState.IsValid)
            {


                if (UserManager.FindByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("", "Пользльзователь с таким E-mail уже существует.");
                }
                else
                {

                    string[] items = date.Split(new char[] { '-' });
                    var user = new User
                    {
                        RoleName = role,
                        UserName = model.Email,
                        Email = model.Email,
                        UserFName = model.UserFName,
                        UserLName = model.UserLName,
                        Birthday = new DateTime(Convert.ToInt32(items[0]), Convert.ToInt32(items[1]), Convert.ToInt32(items[2])),
                        hasFeedback = false,
                        levelName = level,
                        lastFeedback = new DateTime(1900, 01, 01),
                        isActive = true,
                        shutdownDate = new DateTime(1900, 01, 01),
                        withoutAvatar = model.withoutAvatar,

                    };
                    if (!user.withoutAvatar)
                    {
                        if (photo != null)
                        {
                            string extension = Path.GetExtension(photo.FileName);

                            string fileName = user.Id + extension;


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


                            user.avatarName = fileName + ".jpg";
                        }
                    }
                    else
                    {
                        user.avatarName = "";
                    }

                    
                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        _ = await UserManager.AddToRoleAsync(user.Id, role);

                        // Добавление новости студенту
                        News news = new News
                        {
                            title = "Добро пожаловать в личный кабинет Mr. Simon Academy.",
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
                            groupName = "none", 
                            groupId = 0
                        };
                        if (user.RoleName == "Teacher")
                        {

                            news.message = "Личный кабинет сделает Вашу работу со студентами более удобной. " +
                                "Здесь Вы сможете посмотреть список своих групп и студентов, а так же информацию о них. Не забудьте добавить своим ученикам статистику," +
                                " чтобы они смогли увидеть свой прогресс";
                        }
                        if (user.RoleName == "Student")
                        {

                            news.message = "Личный кабинет поможет Вам в изучении языка. " +
                                "Здесь Вы сможете получить актуальную информацию об уроках, посмотреть свой прогресс и многое другое!";
                        }
                        User u = db.Users.Find(user.Id);
                        news.recipientList.Add(u);
                        u.UserNews.Add(news);
                        db.News.Add(news);
                        db.SaveChanges();



                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);

                        await service.SendVerifyCodeAsync(user.Email, "Регистрация Mr. Simon Academy", callbackUrl);



                        // ViewBag.Link = callbackUrl;   // Used only for initial demo.
                        return RedirectToAction("Index", "Admin");
                    }
                    AddErrors(result);

                }

            }


            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Произошла ошибка.");

            return RedirectToAction("Error503", "Error");

        }


        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null)
            {
                return RedirectToAction("NotFound", "Error"); ;
            }


            User user = db.Users.Find(userId);

            user.EmailConfirmed = true;

            db.SaveChanges();
            return RedirectToAction("Login", "Account");
            
            //var result = await UserManager.ConfirmEmailAsync(userId, code);
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            //    return RedirectToAction("Error500", "Error");
            //}
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }



        //// GET: Возвращает форму восстановления
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}


        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByEmailAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        var callbackUrl = Url.Action("ResetPassword", "Account",
        //            new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id, "Сброс пароля",
        //            "Для сброса пароля, перейдите по ссылке <a href=\"" + callbackUrl + "\">сбросить</a>");
        //        return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }
        //    return View(model);
        //}


        //// GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}


        //
        // GET: /Account/ResetPassword
        [Authorize(Roles = "Admin")]
        public ActionResult ResetPassword(string userId)
        {
            if (userId == null)
                return RedirectToAction("NotFound", "Error");

            User user = db.Users.Find(userId);
            if (user == null)
                return RedirectToAction("NotFound", "Error");

            ViewBag.UserName = user.UserFName + " " + user.UserLName;
            return View();
        }


        // POST: /Account/ResetPassword
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("NotFound", "Error");
            }
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View();
            }
            else
            {
        
                return RedirectToAction("Index", "Admin");
            }
    
        }

    }
}