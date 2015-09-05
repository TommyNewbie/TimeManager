using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TimeManager.UI.Domain;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.UI.DataAccess
{
    public class EfEventRepository : IEventRepository
    {
        private readonly EventDbContext _db = new EventDbContext();

        public IEnumerable<Event> GetAll()
        {
            return GetAll(n => true);
        }

        public IEnumerable<Event> GetAll(Expression<Func<Event, bool>> exp)
        {
            return _db.Events.Where(exp).ToList();
        }

        public void Add(Event entry)
        {
            _db.Entry(entry).State = EntityState.Added;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = new Event { Id = id };
            _db.Entry(entry).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public void Update(Event entry)
        {
            _db.Entry(entry).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public Event Find(int id)
        {
            return _db.Events.Find(id);
        }
    }
}