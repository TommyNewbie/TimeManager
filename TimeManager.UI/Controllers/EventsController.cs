using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using TimeManager.UI.Domain;
using TimeManager.UI.Domain.Entry;
using TimeManager.UI.Models;

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

        public JsonResult GetEvents()
        {
            return Json(GetAllEvents(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public ActionResult AddEvent(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.Add(new Event { Name = model.Name, BeginDate = model.BeginTime, EndDate = model.EndTime });
                return RedirectToAction("GetEvents");
            }
            return Json(new { isValid = false, errors = GetErrors(ModelState) }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost()]
        public ActionResult DeleteEvent(int id)
        {
            try
            {
                _db.Delete(id);
                return RedirectToAction("GetEvents");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Oops, something went wrong");
            }

            return Json(new { isValid = false, errors = GetErrors(ModelState) });
        }

        public JsonResult GetEvent(int id)
        {
            var entry = _db.Find(id);
            if (entry != null)
            {
                return Json(new
                {
                    isValid = true,
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

            return Json(new { isValid = false, errors = GetErrors(ModelState) });
        }

        [HttpPost]
        public ActionResult EditEvent(EditEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(new Event
                    {
                        Id = model.Id,
                        Name = model.Name,
                        BeginDate = model.BeginTime,
                        EndDate = model.EndTime
                    });

                    return RedirectToAction("GetEvents");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Oops something went wrong");
                }
            }
            return Json(new { isValid = false, errors = GetErrors(ModelState) }, JsonRequestBehavior.DenyGet);
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
                    isValid = true,
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