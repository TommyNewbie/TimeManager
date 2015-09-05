using System.Collections.Generic;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.UI.Models.SessionModels
{
    public class IntersectedEvents
    {
        public bool IsEmpty { get; set; }

        public Event NewEvent { get; set; }

        public IEnumerable<Event> OldEvents { get; set; }
    }
}