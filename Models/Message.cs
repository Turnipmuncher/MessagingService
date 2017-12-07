using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingService.Models
{
    public class Message
    {
        
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime datesent { get; set; }
        public bool isDraft { get; set; }
        public bool isActive { get; set; }
        public string senderID { get; set; }
        public string recipientID { get; set; }
        public string recipient { get; set; }


        public User User { get; set; }

    }
}
