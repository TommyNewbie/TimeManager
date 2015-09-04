using System;
using System.Collections.Generic;
using System.Data.Entity;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.UI.DataAccess
{
    public class EventDbContext : DbContext
    {
        public EventDbContext() : base("name=TimeManager")
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(n => n.Name).HasMaxLength(50);
            base.OnModelCreating(modelBuilder);
        }
    }
}