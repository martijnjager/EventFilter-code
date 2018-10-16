using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Events;
using System.Windows.Forms;
using System.Text;

// ReSharper disable CheckNamespace
namespace EventFilter.Test
{
    [TestClass]
    public class BootstrapTest
    {
        [TestMethod]
        public void LoadFilesTest()
        {
            var events = Event.Instance;

            var boot = Bootstrap.Boot(events, new CheckedListBox());

            Assert.IsTrue(boot.IsBooted);
        }

        [TestMethod()]
        public void FilesFoundTest()
        {
            Bootstrap.Boot(Event.Instance, new CheckedListBox());
            Form1 form = new Form1();
            Assert.IsTrue(Bootstrap.FilesFound());
        }
    }
}