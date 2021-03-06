﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Moq;
using TimeManager.UI.Domain;
using TimeManager.UI.Controllers;
using TimeManager.UI.Domain.Entry;
using System.Web.Mvc;

namespace TimeManager.Tests
{
    [TestClass]
    public class DeleteEventTest
    {
        public EventsController InitializeController(ICollection<Event> events)
        {
            var repo = new Mock<IEventRepository>();
            repo.Setup(n => n.GetAll()).Returns(events.AsEnumerable());
            repo.Setup(n => n.Delete(It.IsAny<int>())).Callback<int>(id => events.Remove(events.First(n => n.Id == id)));

            return new EventsController(repo.Object);
        }

        [TestMethod]
        public void Delete_without_errors()
        {
            var events = new List<Event> { new Event { Id = 1 } };
            var target = InitializeController(events);

            var result = target.Delete(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), "wrong result type");

            var routeResult = (RedirectToRouteResult)result;
            Assert.AreEqual(0, events.Count, "the event was not deleted");
            Assert.AreEqual("GetAll", routeResult.RouteValues["action"], "wrong action name");
        }

        [TestMethod]
        public void Delete_with_error()
        {
            var events = new List<Event> { new Event { Id = 1 } };
            var target = InitializeController(null);

            var result = target.Delete(6);
            Assert.IsInstanceOfType(result, typeof(JsonResult), "the result has a wrong type");

            dynamic json = ((JsonResult)result).Data;
            var errors = json.errors;

            Assert.AreEqual("error", json.status, "the status property contains wrong value");
            Assert.AreEqual(1, errors.Count, "the errors property contains wrong count of elements");
        }
    }
}
