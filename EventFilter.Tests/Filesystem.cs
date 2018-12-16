using EventFilter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Contracts;
using EventFilter.Events;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFilter.Test
{
    [TestClass()]
    public class Filesystem
    {
        [TestMethod()]
        public void ScanTest()
        {
            IEvent events = Event.Instance;
            Bootstrap.Boot(events, new CheckedListBox());
            string eventLocation = null;
            Zip.ExtractZip("C:\\Users\\marti\\Desktop\\DESKTOP-NPASAR7 (2018-06-26 10 11).zip", ref eventLocation);

            Assert.IsNotNull(eventLocation);
        }
    }
}