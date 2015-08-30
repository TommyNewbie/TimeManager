using System;

namespace TimeManager.UI.Models
{
    public class CreateEventViewModel
    {
        public string Name { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}