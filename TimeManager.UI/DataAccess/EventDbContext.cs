using System;
using System.Collections.Generic;
using System.Data.Entity;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.UI.DataAccess
{
    public class EventDbContext : DbContext
    {
        static EventDbContext()
        {
            Database.SetInitializer(new TimeManagerDatabaseInitializer());
        }

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

    class TimeManagerDatabaseInitializer : DropCreateDatabaseIfModelChanges<EventDbContext>
    {
        protected override void Seed(EventDbContext context)
        {
            context.Events.AddRange
                (
                    new List<Event>()
                    {
                        new Event
                        {
                            Name = "first event",
                            BeginDate = new DateTime(2015, 9, 7, 14, 10, 0),
                            EndDate = new DateTime(2015, 9, 7, 15, 20, 0)
                        },
                        new Event
                        {
                            Name = "second event",
                            BeginDate = new DateTime(2015, 9, 7, 15, 30, 0),
                            EndDate = new DateTime(2015, 9, 7, 15, 50, 0)
                        },
                        new Event
                        {
                            Name = "third event",
                            BeginDate = new DateTime(2015, 9, 8, 14, 10, 0),
                            EndDate = new DateTime(2015, 9, 8, 15, 10, 0)
                        },
                        new Event
                        {
                            Name = "fourth event",
                            BeginDate = new DateTime(2015, 9, 11, 14, 10, 0),
                            EndDate = new DateTime(2015, 9, 12, 12, 20, 0)
                        },
                        new Event
                        {
                            Name = "fifth event",
                            BeginDate = new DateTime(2015, 8, 11, 14, 10, 0),
                            EndDate = new DateTime(2015, 8, 12, 12, 20, 0)
                        },
                        new Event
                        {
                            Name = "sixth event",
                            BeginDate = new DateTime(2015, 7, 11, 14, 10, 0),
                            EndDate = new DateTime(2015, 7, 12, 12, 20, 0)
                        },

                    }
                );
            base.Seed(context);
        }
    }
}