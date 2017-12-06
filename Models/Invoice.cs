using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingService.Models
{
    public class Invoice
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string orderDetails { get; set; }
        public string recipient { get; set; }
        public DateTime invoiceDate { get; set; }
        
    }
}
