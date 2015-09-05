using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManager.UI.Controllers;
using System.Collections.Generic;
using TimeManager.UI.Domain.Entry;
using Moq;
using TimeManager.UI.Domain;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;

namespace TimeManager.Tests
{
    [TestClass]
    public class UpdateEventTest
    {
        private EventsController InitializeController(ICollection<Event> events)
        {
            var repo = new Mock<IEventRepository>();
            repo.Setup(n => n.Find(It.IsAny<int>())).Returns<int>(id => events.FirstOrDefault(e => e.Id == id));
            return new EventsController(repo.Object);
        }

        [TestMethod]
        public void GetExistingEvent()
        {
            var entry = new Event() { Id = 0, BeginDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Name = "n0" };
            var target = InitializeController(new List<Event>() { entry });

            dynamic json = target.Update(0).Data;

            Assert.AreEqual(json.status, "valid", "The isValid property has a wrong value");

            var jsonEntry = json.entry;
            Assert.AreEqual(entry.Id, jsonEntry.id, "The entry property has a wrong value");
            Assert.AreEqual(entry.BeginDate.ToString(CultureInfo.InvariantCulture), jsonEntry.beginTime, "The beginTime property has a wrong value");
            Assert.AreEqual(entry.EndDate.ToString(CultureInfo.InvariantCulture), jsonEntry.endTime, "The endTime property has a wrong value");
            Assert.AreEqual(entry.Name, jsonEntry.name, "The name property has a wrong value");
        }
    }
}
