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
            List<User> testUsers = new List<User>();

            testUsers.Add(new User { userID = "Test1-Pls-ignore", userName = "BarryBob", isStaff = false, isActive = true });
            testUsers.Add(new User { userID = "Test1-Pls-ignore", userName = "BobBarry", isStaff = true, isActive = true });

            context.User.AddRange(testUsers);
            context.SaveChanges();

            testMessages.Add(new Message { subject = "Test 1", message = "Testing.", isDraft = true, datesent = DateTime.Now, isActive = true, senderID = testUsers.Single(s => s.userName == "BarryBob").userID, recipientID = testUsers.Single(s => s.userName == "BobBarry").userID, recipient ="BobBarry" });
            testMessages.Add(new Message { subject = "Test 2", message = "Item has not been deliverd.", isDraft = false, datesent = DateTime.Now, isActive = true, senderID = testUsers.Single(s => s.userName == "BobBarry").userID, recipientID = testUsers.Single(s => s.userName == "BarryBob").userID, recipient = "BarryBob" });

            context.Messages.AddRange(testMessages);
            context.SaveChanges();

            testInvoices.Add(new Invoice { orderDetails = "Testing", recipient = "Test-Recipient", invoiceDate = DateTime.Now });

            context.Invoice.AddRange(testInvoices);
            context.SaveChanges();
#endif

        }
    }
}
