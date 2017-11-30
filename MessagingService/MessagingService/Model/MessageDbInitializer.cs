using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingService.Model
{
    public class MessageDbInitializer
    {
        public static void Initialize(MessageContext context)
        {
            context.Database.EnsureCreated();

            context.Messages.Add(new Model.Message
            {
                id = 1,
                subject = "not delivered",
                message = "Item has not been deliverd.",
                sender = "Test-sender",
                recipient = "Test-sender-staff",
                isDraft = false
            });
            context.SaveChanges();

        }

    }
}
