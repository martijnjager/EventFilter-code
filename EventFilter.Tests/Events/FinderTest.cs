using System.Collections.Generic;
using EventFilter.Contracts;
using EventFilter.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventFilter.Test
{
    [TestClass]
    public class FinderTest
    {
        [TestMethod]
        public void Has_MatchesKeyword_ReturnsTrue()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("The system has rebooted without cleanly shutting down first.");
            var keywords = new List<string> { "rebooted", "error" };

            // Act
            bool result = ev.Has(keywords);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Has_NoKeywordMatch_ReturnsFalse()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("The system has rebooted without cleanly shutting down first.");
            var keywords = new List<string> { "disk", "paging" };

            // Act
            bool result = ev.Has(keywords);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasNot_NoKeywordMatch_ReturnsTrue()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("The system has rebooted without cleanly shutting down first.");
            var keywords = new List<string> { "disk", "paging" };

            // Act
            bool result = ev.HasNot(keywords);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasNot_MatchesKeyword_ReturnsFalse()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("The system has rebooted without cleanly shutting down first.");
            var keywords = new List<string> { "rebooted", "error" };

            // Act
            bool result = ev.HasNot(keywords);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Has_EmptyKeywords_ReturnsFalse()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("Something happened");
            var keywords = new List<string>();

            // Act
            bool result = ev.Has(keywords);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasNot_EmptyKeywords_ReturnsTrue()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("Something happened");
            var keywords = new List<string>();

            // Act
            bool result = ev.HasNot(keywords);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Has_EmptyEvent_ReturnsFalse()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("");
            var keywords = new List<string> { "keyword" };

            // Act
            bool result = ev.Has(keywords);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasNot_EmptyEvent_ReturnsTrue()
        {
            // Arrange
            IEvent ev = Event.GetInstance();
            ev.With("");
            var keywords = new List<string> { "keyword" };

            // Act
            bool result = ev.HasNot(keywords);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
