using System.Collections.Generic;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.UI.Domain
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAll();

        void Add(Event entry);

        void Delete(int id);

        void Update(Event entry);
    }
}
