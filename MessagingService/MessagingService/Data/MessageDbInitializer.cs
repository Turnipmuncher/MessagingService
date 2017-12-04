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

            context.Subject.AddRange(testSubject);
            context.SaveChanges();

            testMessages.Add(new Message { messageId = 2,subject = "Test 1", subjectId = context.Subject.SingleOrDefault(s => s.subject == "Test 1").id , message = "Testing.", sender = "Test-sender1", recipient = "Test-sender-staff1", isDraft = true, datesent = DateTime.Now });
            testMessages.Add(new Message { messageId = 1, subject = "Test 2", subjectId = context.Subject.SingleOrDefault(s => s.subject == "Test 1").id, message = "Item has not been deliverd.", sender = "Test-sender1", recipient = "Test-sender-staff2", isDraft = false, datesent = DateTime.Now });

            context.Messages.AddRange(testMessages);
            context.SaveChanges();

            testInvoices.Add(new Invoice { orderId = 1, orderDetails = "Testing", sender = "Test-Sender", recipient = "Test-Recipient", datesent = DateTime.Now });

            context.Invoice.AddRange(testInvoices);
            context.SaveChanges();
#endif

        }
    }
}
