using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Moq;
using TimeManager.UI.Domain;
using TimeManager.UI.Controllers;
using TimeManager.UI.Domain.Entry;

namespace TimeManager.Tests
{
    [TestClass]
    public class DeleteEventTest
    {
        public EventsController InitializeController()
        {
            var events = new List<Event>()
            {
                new Event { Id = 0, BeginDate = DateTime.Now.AddHours(2)},
                new Event { Id = 1, BeginDate = DateTime.Now.AddHours(2)},
                new Event { Id = 2, BeginDate = DateTime.Now.AddHours(2)},
            };

            var repo = new Mock<IEventRepository>();
            repo.Setup(n => n.GetAll()).Returns(events.AsEnumerable());
            repo.Setup(n => n.Delete(It.IsAny<int>())).Callback<int>(id => events.Remove(events.First(n => n.Id == id)));

            return new EventsController(repo.Object);
        }

        [TestMethod]
        public void Delete_without_errors()
        {
            var target = InitializeController();

            dynamic json = target.DeleteEvent(1).Data;
            var futureEvents = ((IEnumerable<dynamic>)json.futureEvents).ToArray();

            Assert.IsTrue(json.isValid, "isValid property contains wrong value");
            Assert.AreEqual(2, futureEvents.Length, "Wrong count of the delited elements");
            Assert.AreEqual(0, futureEvents[0].id, "Wrong element was deleted");
            Assert.AreEqual(2, futureEvents[1].id, "Wrong element was deleted");
        }

        [TestMethod]
        public void Delete_with_error()
        {
            var target = InitializeController();

            dynamic json = target.DeleteEvent(6).Data;
            var errors = json.errors;

            Assert.IsFalse(json.isValid, "isValid property constains wrong value");
            Assert.AreEqual(1, errors.Length, "errors property contains wrong count of elements");
            Assert.AreEqual("Oops, something went wrong.", errors[0], "error[0] contains wrong value");
        }
    }
}
