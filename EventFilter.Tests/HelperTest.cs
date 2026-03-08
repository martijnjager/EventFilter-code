using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventFilter;

namespace EventFilter.Test
{
    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void ToInt_ValidPositiveInteger_ReturnsInteger()
        {
            string input = "123";
            int expected = 123;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToInt_ValidNegativeInteger_ReturnsInteger()
        {
            string input = "-456";
            int expected = -456;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToInt_NonNumericString_ReturnsZero()
        {
            string input = "abc";
            int expected = 0;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToInt_EmptyString_ReturnsZero()
        {
            string input = "";
            int expected = 0;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToInt_NullString_ReturnsZero()
        {
            string input = null;
            int expected = 0;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToInt_IntegerOverflow_ReturnsZero()
        {
            string input = "2147483648";
            int expected = 0;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToInt_IntegerUnderflow_ReturnsZero()
        {
            string input = "-2147483649";
            int expected = 0;
            int actual = input.ToInt();
            Assert.AreEqual(expected, actual);
        }
    }
}
