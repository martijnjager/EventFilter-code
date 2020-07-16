using EventFilter.Contracts;
using EventFilter.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
namespace EventFilter.Test
{
    [TestClass()]
    public class Index
    {
        [TestMethod()]
        public void IndexTest()
        {
            IEvent events = Event.GetInstance();
            Helper.Form = new Form1();
            Bootstrap.Boot();

            events.SetLocation("G:\\Documents\\Visual Studio 2015\\Projects\\EventFilter\\EventFilter\\bin\\Debug\\eventlog.txt");

            events.MapEvents();

            Assert.IsTrue(events.Eventlogs.Count > 0 && events.Events.Count > 0);
        }

        [TestMethod()]
        public void IndexExtvTest()
        {
            IEvent events = Event.GetInstance();
            Helper.Form = new Form1();
            Bootstrap.Boot();

            events.SetLocation("C:\\Users\\marti\\Desktop\\eventlogs.evtx");

            events.MapEvents();

            Assert.IsTrue(events.Eventlogs.Count > 0 && events.Events.Count > 0);
        }
    }
}