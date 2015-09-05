using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;
using TimeManager.UI.Domain;
using TimeManager.UI.Domain.Entry;
using TimeManager.UI.Models;
using TimeManager.UI.Models.SessionModels;

[assembly: InternalsVisibleTo("TimeManager.Tests")]
namespace TimeManager.UI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _db;

        public EventsController(IEventRepository db)
        {
            _db = db;
        }

        public ActionResult Events()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            return Json(GetAllEvents(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public ActionResult Create(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event { Id = -1, Name = model.Name, BeginDate = model.BeginTime, EndDate = model.EndTime };
                if (IsIntersected(newEvent))
                {
                    return RedirectToAction("Intersection");
                }
                _db.Add(newEvent);
                return RedirectToAction("GetAll");
            }
            return Json(new { status = "error", errors = GetErrors(ModelState) }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost()]
        public ActionResult Delete(int id)
        {
            try
            {
                _db.Delete(id);
                return RedirectToAction("GetAll");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Oops, something went wrong");
            }

            return Json(new { status = "error", errors = GetErrors(ModelState) });
        }

        public JsonResult Update(int id)
        {
            var entry = _db.Find(id);
            if (entry != null)
            {
                return Json(new
                {
                    status = "valid",
                    entry = new
                    {
                        id = entry.Id,
                        beginTime = entry.BeginDate.ToString(CultureInfo.InvariantCulture),
                        endTime = entry.EndDate.ToString(CultureInfo.InvariantCulture),
                        name = entry.Name
                    }
                }, JsonRequestBehavior.AllowGet);
            }

            ModelState.AddModelError("", "Event does not exist");

            return Json(new { status = "error", errors = GetErrors(ModelState) });
        }

        [HttpPost]
        public ActionResult Update(UpdateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    Id = model.Id,
                    Name = model.Name,
                    BeginDate = model.BeginTime,
                    EndDate = model.EndTime
                };

                if (IsIntersected(newEvent))
                {
                    return RedirectToAction("Intersection");
                }

                try
                {
                    _db.Update(newEvent);
                    return RedirectToAction("GetAll");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Oops something went wrong");
                }
            }
            return Json(new { status = "error", errors = GetErrors(ModelState) }, JsonRequestBehavior.DenyGet);
        }

        public JsonResult Intersection(IntersectedEvents model)
        {
            var sb = new StringBuilder("The event that you creates is intersects with: ");

            if (model.OldEvents.Count() <= 5)
            {
                foreach (var oldEvent in model.OldEvents)
                {
                    sb.Append(oldEvent.Name + ", ");
                }
                sb.Length = sb.Length - 2;
                sb.Append(".");
            }
            else
            {
                foreach (var oldEvent in model.OldEvents.Take(5))
                {
                    sb.Append(oldEvent.Name + ", ");
                }
                sb.Length = sb.Length - 2;
                sb.Append(" and others.");
            }
            return Json(new { status = "intersected", id = model.NewEvent.Id, name = model.NewEvent.Name, message = sb.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Intersection(IntersectionViewModel model, IntersectedEvents sessionModel)
        {
            if (!sessionModel.IsEmpty)
            {
                try
                {
                    var newEvent = sessionModel.NewEvent;
                    if (model.Id != -1 && model.Id == newEvent.Id)
                    {
                        _db.Update(newEvent);
                    }
                    else if (model.Id == -1 && newEvent.Id == -1 && model.Name.Equals(newEvent.Name, StringComparison.Ordinal))
                    {
                        _db.Add(newEvent);
                    }
                    return RedirectToAction("GetAll");
                }
                catch
                {
                    ModelState.AddModelError("", "Can't save changes.");
                    return Json(new { status = "error", errors = GetErrors(ModelState) }, JsonRequestBehavior.DenyGet);
                }
            }
            ModelState.AddModelError("", "Oops, something went wrong");
            return Json(new { status = "error", errors = GetErrors(ModelState) }, JsonRequestBehavior.DenyGet);
        }

        private bool IsIntersected(Event newEvent)
        {
            var events = _db.GetAll(old => newEvent.EndDate > old.BeginDate && old.EndDate > newEvent.BeginDate).Where(n => n.Id != newEvent.Id);
            var result = events.Count() != 0;

            if (result)
            {
                Session.Add("Intersection", new IntersectedEvents
                {
                    IsEmpty = false,
                    NewEvent = newEvent,
                    OldEvents = events
                });
            }
            return result;

        }

        private object GetErrors(ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            foreach (var value in modelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors;
        }

        #region GetAllEventsJson

        private object GetAllEvents()
        {
            var events = _db.GetAll().ToArray();
            var futureEvents = events.Where(n => n.EndDate >= DateTime.Now).ToArray();

            return
                new
                {
                    status = "valid",
                    futureEvents = FormatEvents(futureEvents),
                    pastEvents = FormatEvents(events.Except(futureEvents))
                };
        }

        private object FormatEvents(IEnumerable<Event> collection)
        {
            return
                collection.OrderBy(n => n.BeginDate)
                    .Select(n =>
                    new
                    {
                        id = n.Id,
                        name = n.Name,
                        beginDate = n.BeginDate.ToString(CultureInfo.InvariantCulture),
                        endDate = n.EndDate.ToString(CultureInfo.InvariantCulture)
                    });
        }
    }
    #endregion
}