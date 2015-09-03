using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManager.UI.Controllers;
using Moq;
using TimeManager.UI.Domain;
using TimeManager.UI.Domain.Entry;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace TimeManager.Tests
{
    [TestClass]
    public class GetAllEventsTest
    {
        private EventsController InitializeController(IEnumerable<Event> events)
        {
            var repo = new Mock<IEventRepository>();
            repo.Setup(n => n.GetAll()).Returns(events.AsEnumerable());
            return new EventsController(repo.Object);
        }

        [TestMethod]
        public void GetAll_with_exist_events()
        {
            var event0 = new Event() { Id = 0, Name = "name0", BeginDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(2) };
            var event1 = new Event() { Id = 1, Name = "name1", BeginDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3) };
            var event2 = new Event() { Id = 2, Name = "name2", BeginDate = new DateTime(2012, 10, 1), EndDate = new DateTime(2012, 10, 2) };
            var event3 = new Event() { Id = 3, Name = "name3", BeginDate = new DateTime(2001, 10, 10), EndDate = new DateTime(2002, 1, 1) };

            var target = InitializeController(new[] { event0, event1, event2, event3 });

            dynamic json = target.GetEvents().Data;
            var futureEvents = ((IEnumerable<dynamic>)json.futureEvents).ToArray();
            var pastEvents = ((IEnumerable<dynamic>)json.pastEvents).ToArray();

            Assert.AreEqual(true, json.isValid, "isValid property contains wrong value");

            Assert.AreEqual(0, futureEvents[0].id, "futureEvents[0].id contains wrong value");
            Assert.AreEqual(1, futureEvents[1].id, "futureEvents[1].id contains wrong value");
            Assert.AreEqual("name0", futureEvents[0].name, "futureEvents[0].name contains wrong value");
            Assert.AreEqual("name1", futureEvents[1].name, "futureEvents[1].name contains wrong value");
            Assert.IsTrue(string.Equals(event0.BeginDate.ToString(CultureInfo.InvariantCulture), futureEvents[0].beginDate, StringComparison.CurrentCulture), "futureEvents[0].beginDate contains wrong value");
            Assert.IsTrue(string.Equals(event1.BeginDate.ToString(CultureInfo.InvariantCulture), futureEvents[1].beginDate, StringComparison.CurrentCulture), "futureEvents[1].beginDate contains wrong value");
            Assert.IsTrue(string.Equals(event0.EndDate.ToString(CultureInfo.InvariantCulture), futureEvents[0].endDate, StringComparison.CurrentCulture), "futureEvents[0].endDate contains wrong value");
            Assert.IsTrue(string.Equals(event1.EndDate.ToString(CultureInfo.InvariantCulture), futureEvents[1].endDate, StringComparison.CurrentCulture), "futureEvents[1].endDate contains wrong value");

            Assert.AreEqual(3, pastEvents[0].id, "pastEvents[0].id contains wrong value");
            Assert.AreEqual(2, pastEvents[1].id, "pastEvents[1].id contains wrong value");
            Assert.AreEqual("name3", pastEvents[0].name, "pastEvents[0].name contains wrong value");
            Assert.AreEqual("name2", pastEvents[1].name, "pastEvents[1].name contains wrong value");
            Assert.IsTrue(string.Equals(event3.BeginDate.ToString(CultureInfo.InvariantCulture), pastEvents[0].beginDate, StringComparison.CurrentCulture), "pastEvents[0].beginDate contains wrong value");
            Assert.IsTrue(string.Equals(event2.BeginDate.ToString(CultureInfo.InvariantCulture), pastEvents[1].beginDate, StringComparison.CurrentCulture), "pastEvents[1].beginDate contains wrong value");
            Assert.IsTrue(string.Equals(event3.EndDate.ToString(CultureInfo.InvariantCulture), pastEvents[0].endDate, StringComparison.CurrentCulture), "pastEvents[0].endDate contains wrong value");
            Assert.IsTrue(string.Equals(event2.EndDate.ToString(CultureInfo.InvariantCulture), pastEvents[1].endDate, StringComparison.CurrentCulture), "pastEvents[1].endDate contains wrong value");
        }

        [TestMethod]
        public void GetAll_without_data()
        {
            var target = InitializeController(new Event[] { });

            dynamic json = target.GetEvents().Data;
            var futureEvents = ((IEnumerable<dynamic>)json.futureEvents).ToArray();
            var pastEvents = ((IEnumerable<dynamic>)json.pastEvents).ToArray();

            Assert.AreEqual(true, json.isValid, "isValid property has invalid value");
            Assert.AreEqual(0, futureEvents.Length, "futureEvents is not empty");
            Assert.AreEqual(0, pastEvents.Length, "pastEvents is not empty");
        }
    }
}
