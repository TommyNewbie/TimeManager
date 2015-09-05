using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManager.UI.Controllers;
using TimeManager.UI.Domain;
using Moq;
using System.Collections.Generic;
using TimeManager.UI.Domain.Entry;
using TimeManager.UI.Models;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace TimeManager.Tests
{
    [TestClass]
    public class CreateEventTest
    {
        private EventsController InitializeController(ICollection<Event> events)
        {
            int o = 0;
            var repo = new Mock<IEventRepository>();
            repo.Setup(n => n.Add(It.IsAny<Event>())).Callback<Event>(e => events.Add(e));
            repo.Setup(n => n.GetAll()).Returns(events.AsEnumerable());

            return new EventsController(repo.Object);
        }

        [TestMethod]
        public void Create_valid_event()
        {
            var events = new List<Event>();
            var target = InitializeController(events);

            var result = target.Create(new CreateEventViewModel { BeginTime = DateTime.Now.AddDays(1) });
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), "wrong result type");

            var routeResult = (RedirectToRouteResult)result;
            Assert.AreEqual("GetAll", routeResult.RouteValues["action"], "wrong action name");
            Assert.AreEqual(1, events.Count, "event was not added");
        }

        [TestMethod]
        public void Create_invalid_events()
        {
            var firstError = "something goes wrong";
            var secondError = "one more thing goes wrong";

            var events = new List<Event>();
            var target = InitializeController(events);

            target.ModelState.AddModelError("", firstError);
            target.ModelState.AddModelError("", secondError);

            var result = target.Create(new CreateEventViewModel() { BeginTime = DateTime.Now.AddDays(1) });
            Assert.IsInstanceOfType(result, typeof(JsonResult));

            dynamic json = ((JsonResult)result).Data;
            var errors = json.errors.ToArray();

            Assert.AreEqual("error", json.status, "the status property contains wrong value");
            Assert.AreEqual(2, errors.Length, "the errors property has wrong length");
            Assert.AreEqual(firstError, errors[0], "errors[0] contains wrong value");
            Assert.AreEqual(secondError, errors[1], "errors[1] contains wrong value");
            Assert.AreEqual(0, events.Count, "event was added");
        }
    }
}
