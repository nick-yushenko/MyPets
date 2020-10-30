using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string Language { get; set; }

        public string CourseName { get; set; }

    }
}