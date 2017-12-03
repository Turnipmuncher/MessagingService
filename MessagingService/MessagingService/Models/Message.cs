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
        public int messageId { get; set; }
        [ForeignKey("SubjectID")]
        public int subjectId { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string sender { get; set; }
        public string recipient { get; set; }
        public DateTime datesent { get; set; }
        public bool isDraft { get; set; }
    }
}
