using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageService.Model;

namespace MessageService.Data
{
    public class MessageDbInitializer
    {
        public static void Initialize(MessageContext context)
        {

            context.Database.EnsureCreated();

            if (context.Messages.Any())
            {
                return;
            }

            var message = new Message
            {
                ID = 1,
                subject = "not delivered",
                message = "Item has not been deliverd.",
                sender = "Test-sender",
                recipient = "Test-sender-staff",
                isDraft = false
            };

            context.Messages.Add(message);
            context.SaveChanges();


        }
    }
}
