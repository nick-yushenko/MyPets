using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MrSimonAcademy2.Models;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


// Запуск данного класса, при запуске OWIN-приложения (= запуск веб приложения)
[assembly: OwinStartup(typeof(MrSimonAcademy2.Startup))]

namespace MrSimonAcademy2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // настраиваем контекст и менеджер пользователей
            app.CreatePerOwinContext<ApplicationDbContext>(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // регистрация менеджера ролей
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}