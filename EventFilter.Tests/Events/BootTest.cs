using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter;

// ReSharper disable CheckNamespace
namespace EventFilter.Test
{
    [TestClass()]
    public class BootstrapTest
    {
        [TestMethod()]
        public void LoadFilesTest()
        {
            //Actions.Form = new Form1();

            //var boot = Bootstrap.Boot();

            //Assert.IsTrue(boot.IsBooted);
        }

        [TestMethod()]
        public void FilesFoundTest()
        {
            Helper.Form = new Form1();
            Bootstrap.Boot();
            Assert.IsTrue(Bootstrap.FilesFound);
        }
    }
}