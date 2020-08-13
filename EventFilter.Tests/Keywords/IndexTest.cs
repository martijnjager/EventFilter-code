using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventFilter.Test
{
    [TestClass()]
    public class KeywordIndexTest
    {
        [TestMethod()]
        public void IndexKeywordsTest()
        {
            Helper.Form = new Form1();
            Bootstrap.Boot();

            Keywords.Keyword.GetInstance().Map();

            Assert.IsTrue(Keywords.Keyword.GetInstance().KeywordsLoaded);
        }
    }
}