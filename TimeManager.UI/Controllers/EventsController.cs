using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeManager.UI.Domain;
using TimeManager.UI.Domain.Entry;
using TimeManager.UI.Models;

namespace TimeManager.UI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _db;

        public EventsController(IEventRepository db)
        {
            _db = db;
        }

        public ActionResult GetEvents()
        {
            return View(_db.GetAll().OrderByDescending(n => n.BeginDate));
        }

        public PartialViewResult CreateEvent(CreateEventViewModel model)
        {
            _db.Add(new Event() { Name = model.Name, BeginDate = model.BeginTime, EndDate = model.EndTime });
            return PartialView("DisplayEvents", _db.GetAll().OrderByDescending(n => n.BeginDate));
        }
    }
}