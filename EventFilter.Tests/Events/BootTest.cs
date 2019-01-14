using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Events;
using System.Windows.Forms;

// ReSharper disable CheckNamespace
namespace EventFilter.Test
{
    [TestClass()]
    public class BootstrapTest
    {
        [TestMethod()]
        public void LoadFilesTest()
        {
            var events = Event.Instance;
            Actions.form = new Form1();

            var boot = Bootstrap.Boot(events);

            Assert.IsTrue(boot.IsBooted);
        }

        [TestMethod()]
        public void FilesFoundTest()
        {
            Actions.form = new Form1();
            Bootstrap.Boot(Event.Instance);
            Form1 form = new Form1();
            Assert.IsTrue(Bootstrap.FilesFound());
        }
    }
}