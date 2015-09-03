using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManager.UI.Controllers;
using TimeManager.UI.Domain;
using Moq;
using System.Collections.Generic;
using TimeManager.UI.Domain.Entry;
using TimeManager.UI.Models;
using System.Linq;

namespace TimeManager.Tests
{
    [TestClass]
    public class AddEventTest
    {
        private EventsController InitializeController()
        {
            var events = new List<Event>
            {
                new Event() { BeginDate = DateTime.Now },
                new Event() { BeginDate = DateTime.Now },
                new Event() { BeginDate = DateTime.Now }
            };

            var repo = new Mock<IEventRepository>();
            repo.Setup(n => n.Add(It.IsAny<Event>())).Callback<Event>(e => events.Add(e));
            repo.Setup(n => n.GetAll()).Returns(events.AsEnumerable());

            return new EventsController(repo.Object);
        }

        [TestMethod]
        public void Add_valid_event()
        {
            var target = InitializeController();

            dynamic json = target.AddEvent(new CreateEventViewModel { BeginTime = DateTime.Now.AddDays(1) }).Data;
            var futureEvents = ((IEnumerable<dynamic>)json.futureEvents).ToArray();
            var pastEvents = ((IEnumerable<dynamic>)json.pastEvents).ToArray();

            Assert.IsTrue(json.isValid, "isValid property contains wrong value");
            Assert.AreEqual(3, pastEvents.Length, "pastEvents array has wrong length");
            Assert.AreEqual(1, futureEvents.Length, "futureEvents array has wrong length");
        }

        [TestMethod]
        public void Add_invalid_events()
        {
            var firstError = "something goes wrong";
            var secondError = "one more thng goes wrong";
            var target = InitializeController();
            target.ModelState.AddModelError("", firstError);
            target.ModelState.AddModelError("", secondError);

            dynamic json = target.AddEvent(new CreateEventViewModel() { BeginTime = DateTime.Now.AddDays(1) }).Data;
            var errors = json.errors.ToArray();

            Assert.IsFalse(json.isValid, "isValid property contains wrong value");
            Assert.AreEqual(2, errors.Length, "errors property has wrong length");
            Assert.AreEqual(firstError, errors[0], "errors[0] constains wrong value");
            Assert.AreEqual(secondError, errors[1], "errors[1] constains wrong value");
        }
    }
}
