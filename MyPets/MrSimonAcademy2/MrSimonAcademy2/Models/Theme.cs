using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{


    //Модель темы.Связь один ко многим (одна тема - много файлов) и один ко многим(одна группа - много тем)
    public class Theme
    {
        // Дата создания: 10.10.2020
        // Дата последних изменений: 10.10.2020


        public int Id { get; set; }

        // Название темы 
        public string themeName { get; set; }

        // Группа, для которой эта тема
        public Group group { get; set; }

        // Id группы, для которой эта тема
        public int groupId { get; set; }

        // Цвет маркера для темы (Для каждой темы учитель (админ) выбирает 1 из 4 цветов: синий, зеленый, розовый, фиолетовый). 
        // Не несет функциональности. Исключительно для дизайна.
        // Цвет по умолчанию - синий
        public string colorTheme { get; set; }

        // Дата добавления темы
        public DateTime added { get; set; }

        // Список заданий
        public virtual ICollection<Assignment> AssignmentList { get; set; }

        public Theme()
        {
            AssignmentList = new List<Assignment>();
        }
    }

    //Модель задания в теме
    public class Assignment
    {

        public int Id { get; set; }

        // Тема, для которой это задание 
        public Theme theme { get; set; }

        // Id темы, для которой это задание 
        public int themeId { get; set; }

        // Тип задания (Аудио, видео, текст, ссылка, фото, для скачивания)
        public string type { get; set; }

        // Имя файла. Хранится для отображения в аудио или в видео. Очень кратко описвает задание
        public string fileName { get; set; }

        // Требования к заданию от преподавателя
        public string AssignmentTask { get; set; }

        // Если задание имеет файл - название файла и его расширение
        public string AssignmentFileName { get; set; }
        public string AssignmentFileExpansion { get; set; }
        // Если задание имеет файл - название файла и его расширение
        public string link { get; set;  }

        //// Если это текстовое задание - Текст самого задания
        //public string AssignmentText { get; set; }

        // Дата добавления задания
        public DateTime added { get; set; }

    }


}