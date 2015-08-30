using System;

namespace TimeManager.UI.Domain.Entry
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}