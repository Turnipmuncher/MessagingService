using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingService.Models;

namespace MessagingService.Data
{
    public class MessageDbInitializer
    {
        public static void Initialize(MessageContext context)
        {
#if DEBUG
            context.Database.EnsureDeleted();
#endif
            context.Database.EnsureCreated();

#if DEBUG
            List<Message> testMessages = new List<Message>();
            List<Invoice> testInvoices = new List<Invoice>();
            List<Subject> testSubject = new List<Subject>();

            testSubject.Add(new Subject { subject = "Test 1" });
            testSubject.Add(new Subject { subject = "Test 2" });

            testMessages.Add(new Message { messageId = 2, subjectId = 2, subject = "Test 1", message = "Testing.", sender = "Test-sender", recipient = "Test-sender-staff", isDraft = true, datesent = DateTime.Now });
            testMessages.Add(new Message { messageId = 1, subjectId = 1, subject = "Test 2", message = "Item has not been deliverd.", sender = "Test-sender", recipient = "Test-sender-staff", isDraft = false, datesent = DateTime.Now }); 

            testInvoices.Add(new Invoice { orderId = 1, orderDetails = "Testing", sender = "Test-Sender", recipient = "Test-Recipient", datesent = DateTime.Now });

            context.Subject.AddRange(testSubject);
            context.Messages.AddRange(testMessages);
            context.Invoice.AddRange(testInvoices);

            context.SaveChanges();
#endif

        }
    }
}
