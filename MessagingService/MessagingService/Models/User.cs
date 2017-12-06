using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingService.Models
{
    public class User
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string userName { get; set; }
        public bool isStaff { get; set; }
        public bool isActive { get; set; }
    }
}
