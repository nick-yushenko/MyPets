using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string GroupName { get; set; }

        [MaxLength(2)]
        public string GroupLevel { get; set; }

        public int GroupCount { get; set; }

        public string Language { get; set; }

        public bool isPersonal { get; set; }

        public virtual ICollection<User> GroupStudents { get; set; }

        public string GroupTeacherId { get; set; }

        // TODO Переделать в лист дней. Создать класс для учебного дня, который харнит день недели, время и продолжительность занятий 
        // Пока что добавлять через запятую только дни (например: Пн, Вт)
        public string days { get; set; }

        public string textbook { get; set; }

        public virtual ICollection<Theme> ThemeList { get; set; }

        public Group()
        {
            GroupStudents = new List<User>();
            ThemeList = new List<Theme>();
        }

    }
}