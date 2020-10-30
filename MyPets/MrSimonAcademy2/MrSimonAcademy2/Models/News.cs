using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{
    public class News
    {
        // Дата создания: 30.09.2020
        // Дата последних изменений: 09.10.2020

        [Key]
        public int id { get; set; }

        // Id пользователя, который отправил сообщение (учитель или админ)
        public string fromId { get; set; }

        // Список получателей
        public virtual ICollection<User> recipientList { get; set; }

        // Имя группы и ее Id, если сообщение для группы
        public string groupName { get; set; }
        public int groupId { get; set; }

        // Закреплено ли это сообщением 
        public bool isPinned{ get; set; }

        // Является ли это сообщением для отдельного пользователя
        public bool isForUser { get; set; }

        // Является ли это сообщением для целой группы
        public bool isForGroup { get; set; }

        // Парамаетры рассылки (если сразу для нескольких пользователей)
        public bool onlyStudent { get; set; }
        public bool onlyTeacher { get; set; }
        public bool onlyAdmin { get; set; }

        // Является ли это сообщением для всех пользователей
        public bool isForAll { get; set; }
        // Является ли это сообщением системным
        public bool isFromSystem { get; set; }

        

        // Текст сообщения (новости, объявления)
        public string message { get; set; }

        // Заголовок сообщения (новости, объявления)
        public string title { get; set; }

        // Дата добавления сообщения (новости, объявления)
        public DateTime added { get; set; }

        public News()
        {
            recipientList = new List<User>();
        }
    }
}