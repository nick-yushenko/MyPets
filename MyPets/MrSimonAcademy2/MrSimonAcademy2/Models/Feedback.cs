using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Rating> RatingList { get; set; }

        public int LessonsCount { get; set; }
        public int LoseLessons { get; set; }

        public int HomeworkCount { get; set; }
        public int PassedHomework { get; set; }

        public int Activity { get; set; }
        public int Concentration { get; set; }

        public string AdventuresPlus { get; set; }
        public string AdventuresMinus { get; set; }

        public DateTime AddingData { get; set; }

        public Feedback()
        {
            RatingList = new List<Rating>();
        }
    }

    public class Rating
    {
        [Key]
        public int Id { get; set; }
        // Speaking, Writing, Listening or Reading
        public string AspectName { get; set; }

        // Оценка от 1 до 100
        [Range(1, 100)]
        public int Value { get; set; }

        public Rating()
        {
        }
        public Rating(string aspect, int value)
        {
            AspectName = aspect;
            Value = value;
        }
    }

}