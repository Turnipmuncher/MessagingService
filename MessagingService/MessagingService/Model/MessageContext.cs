using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingService.Controllers;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Model
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
        }

        public DbSet<MessageItem> MessageItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageItem>().ToTable("Message");
        }
    }
}