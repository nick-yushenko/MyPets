using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Models
{

    public class IndexViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserFName { get; set; }
        public string UserLName { get; set; }
        public DateTime Birthday { get; set; }
        public string RoleName { get; set; }
        public bool HasFeedback { get; set;  }
        public string LevelName { get; set; }
    }
}