using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter.Keywords.Concerns;
using System.Linq;
using KeywordTypeEnum = EventFilter.Keywords.Concerns.Type;

namespace EventFilter.Tests.Keywords.Concerns
{
    [TestClass]
    public class KeywordTypeTests
    {
        [TestMethod]
        public void Add_ShouldAddKeywordToKeywordsList()
        {
            // Arrange
            var keywordType = new KeywordType(KeywordTypeEnum.Piracy, true);
            string keyword = "test-keyword";

            // Act
            keywordType.Add(keyword);

            // Assert
            Assert.IsTrue(keywordType.Keywords.Contains(keyword));
            Assert.AreEqual(1, keywordType.Keywords.Count);
        }

        [TestMethod]
        public void Add_ShouldAllowMultipleKeywords()
        {
            // Arrange
            var keywordType = new KeywordType(KeywordTypeEnum.Items, false);
            string keyword1 = "keyword1";
            string keyword2 = "keyword2";

            // Act
            keywordType.Add(keyword1);
            keywordType.Add(keyword2);

            // Assert
            Assert.IsTrue(keywordType.Keywords.Contains(keyword1));
            Assert.IsTrue(keywordType.Keywords.Contains(keyword2));
            Assert.AreEqual(2, keywordType.Keywords.Count);
        }
    }
}
