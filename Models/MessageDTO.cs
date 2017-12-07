using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingService.Models
{
    public class MessageDTO
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string subject { get; set; }

        public string message { get; set; }

        public string recipient { get; set; }

        public bool isDraft { get; set; }
    
    }
}
