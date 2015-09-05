using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.UI.Domain
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAll();

        IEnumerable<Event> GetAll(Expression<Func<Event, bool>> exp);

        void Add(Event entry);

        void Delete(int id);

        void Update(Event entry);

        Event Find(int id);
    }
}
