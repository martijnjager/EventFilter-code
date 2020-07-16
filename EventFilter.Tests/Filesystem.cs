using EventFilter.Contracts;
using EventFilter.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventFilter.Test
{
    [TestClass()]
    public class Filesystem
    {
        [TestMethod()]
        public void ScanTest()
        {
            IEvent events = Event.GetInstance();
            Helper.Form = new Form1();
            Bootstrap.Boot();
            string eventLocation = null;
            Zip.ExtractZip("C:\\Users\\marti\\Desktop\\DESKTOP-NPASAR7 (2018-06-26 10 11).zip", ref eventLocation);

            Assert.IsNotNull(eventLocation);
        }
    }
}