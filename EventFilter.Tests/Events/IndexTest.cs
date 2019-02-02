using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Events;
using System.Windows.Forms;
using EventFilter.Contracts;
using System.IO;

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
            Actions.Form = new Form1();
            Bootstrap.Boot();

            events.SetLocation(new FileInfo("G:\\Documents\\Visual Studio 2015\\Projects\\EventFilter\\EventFilter\\bin\\Debug\\eventlog.txt"));

            events.MapEvents();

            Assert.IsTrue(events.Eventlogs.Count > 0 && events.Events.Count > 0);
        }

        [TestMethod()]
        public void IndexExtvTest()
        {
            IEvent events = Event.Instance;
            Actions.Form = new Form1();
            Bootstrap.Boot();

            events.SetLocation(new FileInfo("C:\\Users\\marti\\Desktop\\eventlogs.evtx"));

            events.MapEvents();

            Assert.IsTrue(events.Eventlogs.Count > 0 && events.Events.Count > 0);
        }
    }
}