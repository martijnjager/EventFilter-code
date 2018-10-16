using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Events;
using System.Windows.Forms;
using EventFilter.Contracts;

// ReSharper disable CheckNamespace
namespace EventFilter.Test
{
    [TestClass()]
    public class Index
    {
        [TestMethod()]
        public void IndexTest()
        {
            IEvent events = Event.Instance;

            Bootstrap.Boot(events, new CheckedListBox());

            events.IndexEvents();

            Assert.IsTrue(events.Eventlogs.Length > 0 && events.Events.Count > 0);
        }
    }
}