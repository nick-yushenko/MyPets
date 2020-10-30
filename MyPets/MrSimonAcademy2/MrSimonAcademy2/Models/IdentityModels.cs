using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MrSimonAcademy2.App_Start;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MrSimonAcademy2.Models
{
    // Модель пользователя
    public class User : IdentityUser
    {
        public string UserFName { get; set; }
        public string UserLName { get; set; }
        public DateTime Birthday { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Group> UserGroups { get; set; }
        public virtual ICollection<News> UserNews { get; set; }

        public virtual ICollection<Feedback> FeedbackList { get; set; }

        public bool hasFeedback { get; set; }

        public DateTime lastFeedback { get; set; }

        // Полное название ступени
        public string levelName { get; set; }

        public bool isActive { get; set; }
        public DateTime shutdownDate { get; set; }

        // Имеется ли фото у данного пользователя (если нет, то отображается стандартное фото)
        public bool withoutAvatar { get; set; }
        public string avatarName { get; set; }

        //public string ph { get; set; }

        public User()
        {
            UserGroups = new List<Group>();
            UserNews = new List<News>();
            FeedbackList = new List<Feedback>();
        }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
        UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in 
            //   CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this,
        DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName) { }

    }


    // Контекст даных для работы с пользователями   
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() :
            base("MrSimonDB", throwIfV1Schema: false)
        { }


        public DbSet<Group> Groups { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<MrSimonAcademy2.Models.TeacherModel> TeacherModels { get; set; }

    }


    // Менеджер для более удобной работы с пользователями
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store) 
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            ApplicationDbContext db = context.Get<ApplicationDbContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<User>(db));

            // Валидация пароля 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
            };


            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

          
            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

   
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore) : base(roleStore) { }
       
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var applicationRoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
            return applicationRoleManager;
        }
        
    }

   
}