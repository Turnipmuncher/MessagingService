﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageService.Controllers;
using Microsoft.EntityFrameworkCore;

namespace MessageService.Model
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("Message");
        }
    }
}