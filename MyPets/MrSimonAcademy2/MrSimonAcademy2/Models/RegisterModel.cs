using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{
    // Модель регистрации. Здесь указаны поля, которые необходимо заполнить для регистрации
    public class RegisterModel
    {
        [Required]
        public string UserFName { get; set; }
        [Required]
        public string UserLName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }

        public bool withoutAvatar { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public string RoleName { get; set; }
    }
}