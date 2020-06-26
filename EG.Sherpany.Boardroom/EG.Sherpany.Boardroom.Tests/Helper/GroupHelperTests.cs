using System;
using EG.Sherpany.Boardroom.Enums;
using EG.Sherpany.Boardroom.Helper;
using EG.Sherpany.Boardroom.Tests.Fakes;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Helper
{
    [TestClass]
    public class GroupHelperTests
    {
        [TestMethod]
        public void GetDateGroupIndexesGivenOlderDateReturnOlderGroup()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 1),
                new FakeDateTimeProvider(new DateTime(2002, 1, 1)));
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.Older));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTodayDateReturnThreeGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.Today));
            Assert.IsTrue(actual.Contains(DateGroup.ThisMonth));
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenYesterdayDateReturnThreeGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1,2)));
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.Yesterday));
            Assert.IsTrue(actual.Contains(DateGroup.ThisMonth));
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenYesterdayEndOfMonthDateReturnThreeGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 31),
                new FakeDateTimeProvider(new DateTime(2000, 2, 1)));
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.Yesterday));
            Assert.IsTrue(actual.Contains(DateGroup.LastMonth));
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenYesterdayEndOfYearDateReturnThreeGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(1999, 12, 31),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.Yesterday));
            Assert.IsTrue(actual.Contains(DateGroup.LastMonth));
            Assert.IsTrue(actual.Contains(DateGroup.LastYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoDaysAgoReturnTwoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 3)));
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.ThisMonth));
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoDaysAgoEndOfMonthReturnTwoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 29),
                new FakeDateTimeProvider(new DateTime(2000, 2, 1)));
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.LastMonth));
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoDaysAgoEndOfYearReturnTwoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(1999, 12, 29),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.LastMonth));
            Assert.IsTrue(actual.Contains(DateGroup.LastYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenLastMonthReturnTwoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(1999, 12, 20),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.LastMonth));
            Assert.IsTrue(actual.Contains(DateGroup.LastYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoMonthsAgoReturnOneGroup()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(1999, 10, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.LastYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoMonthsAgoThisYearReturnOneGroup()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 1),
                new FakeDateTimeProvider(new DateTime(2000, 5, 1)));
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenNextMonthThisYearReturnTwoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 2, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
            Assert.IsTrue(actual.Contains(DateGroup.NextMonth));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenNextMonthNextYearReturnTwoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 1, 1),
                new FakeDateTimeProvider(new DateTime(1999, 12, 31)));
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.NextYear));
            Assert.IsTrue(actual.Contains(DateGroup.NextMonth));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoMonthsFromNowNextYearReturnOneGroup()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 2, 1),
                new FakeDateTimeProvider(new DateTime(1999, 12, 31)));
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.NextYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenTwoMonthsFromNowThisYearReturnOneGroup()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2000, 3, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(DateGroup.ThisYear));
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenFutureReturnNoGroups()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2002, 3, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetDateGroupIndexesGivenFutureReturnNoGroups2()
        {
            var actual = GroupHelper.GetDateGroupIndexes(new DateTime(2002, 2, 1),
                new FakeDateTimeProvider(new DateTime(2000, 1, 1)));
            Assert.AreEqual(0, actual.Count);
        }

        
    }
}
