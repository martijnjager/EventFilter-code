using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFilter.Contracts;
using System.Windows.Forms;

namespace EventFilter.Test
{
    [TestClass()]
    public class KeywordIndexTest
    {
        [TestMethod()]
        public void IndexKeywordsTest()
        {
            var events = Event.Instance;

            Bootstrap.Boot(events, new CheckedListBox());

            events.Keywords.Map();

            Assert.IsTrue(events.Keywords.KeywordsLoaded);
        }
    }
}