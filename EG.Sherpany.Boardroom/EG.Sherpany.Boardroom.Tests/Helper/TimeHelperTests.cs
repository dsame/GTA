using System;
using EG.Sherpany.Boardroom.Helper;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Helper
{
    [TestClass]
    public class TimeHelperTests
    {
        [TestMethod]
        public void ToQuarterStringReturnsQ1OnFirstQuarter()
        {
            for (int i = 1; i <= 3; i++)
            {
                Assert.AreEqual("Q1 - 2000", new DateTime(2000, i, 1).ToQuarterString());
            }
        }

        [TestMethod]
        public void ToQuarterStringReturnsQ2OnSecondQuarter()
        {
            for (int i = 4; i <= 6; i++)
            {
                Assert.AreEqual("Q2 - 2000", new DateTime(2000, i, 1).ToQuarterString());
            }
        }

        [TestMethod]
        public void ToQuarterStringReturns3OnThirdQuarter()
        {
            for (int i = 7; i <= 9; i++)
            {
                Assert.AreEqual("Q3 - 2000", new DateTime(2000, i, 1).ToQuarterString());
            }
        }

        [TestMethod]
        public void ToQuarterStringReturnsQ4OnFourthQuarter()
        {
            for (int i = 10; i <= 12; i++)
            {
                Assert.AreEqual("Q4 - 2000", new DateTime(2000, i, 1).ToQuarterString());
            }
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairTodayReturnsToday()
        {
            var actual = new DateTime(2000,1,2,3,4,5).ToTimeIndexTextKeyValuePair(new DateTime(2000,1,2,5,4,3));
                Assert.AreEqual("Today", actual.Value);
            Assert.AreEqual(0, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairTomorrowReturnsTomorrow()
        {
            var actual = new DateTime(2000, 1, 2, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 1, 5, 4, 3));
            Assert.AreEqual("tomorrow", actual.Value);
            Assert.AreEqual(1, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairYesterdayTomorrowReturnsYesterday()
        {
            var actual = new DateTime(2000, 1, 1, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 2, 5, 4, 3));
            Assert.AreEqual("yesterday", actual.Value);
            Assert.AreEqual(-1, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairSameMonthReturnsThisMonth()
        {
            var actual = new DateTime(2000, 1, 2, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 31, 5, 4, 3));
            Assert.AreEqual("previous this month", actual.Value);
            Assert.AreEqual(-2, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairSameMonthPastReturnsThisMonth()
        {
            var actual = new DateTime(2000, 1, 31, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 2, 5, 4, 3));
            Assert.AreEqual("upcoming this month", actual.Value);
            Assert.AreEqual(2, actual.Key);
        }


        [TestMethod]
        public void ToTimeIndexTextKeyValuePairLastMonthReturnsLastMonth()
        {
            var actual = new DateTime(2000, 1, 2, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 2, 28, 5, 4, 3));
            Assert.AreEqual("last month", actual.Value);
            Assert.AreEqual(-3, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairNextMonthReturnsNextMonth()
        {
            var actual = new DateTime(2000, 2, 28, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 1, 5, 4, 3));
            Assert.AreEqual("next month", actual.Value);
            Assert.AreEqual(3, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairThreeMontsPastReturnsThisYear()
        {
            var actual = new DateTime(2000, 1, 2, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 4, 28, 5, 4, 3));
            Assert.AreEqual("previous this year", actual.Value);
            Assert.AreEqual(-4, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairThreeMontsReturnsThisYear()
        {
            var actual = new DateTime(2000, 4, 28, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 1, 5, 4, 3));
            Assert.AreEqual("upcoming this year", actual.Value);
            Assert.AreEqual(4, actual.Key);
        }
        [TestMethod]
        public void ToTimeIndexTextKeyValuePairLastYearReturnsLastYear()
        {
            var actual = new DateTime(1999, 1, 2, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 2, 28, 5, 4, 3));
            Assert.AreEqual("last year", actual.Value);
            Assert.AreEqual(-5, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairNextYearPastReturnsNextYear()
        {
            var actual = new DateTime(2001, 2, 28, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 1, 1, 5, 4, 3));
            Assert.AreEqual("next year", actual.Value);
            Assert.AreEqual(5, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairTwoYearsReturnsEarlier()
        {
            var actual = new DateTime(1998, 1, 2, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(2000, 2, 28, 5, 4, 3));
            Assert.AreEqual("earlier", actual.Value);
            Assert.AreEqual(-6, actual.Key);
        }

        [TestMethod]
        public void ToTimeIndexTextKeyValuePairTwoYearsFromNowReturnsLater()
        {
            var actual = new DateTime(2000, 2, 28, 3, 4, 5).ToTimeIndexTextKeyValuePair(new DateTime(1998, 1, 1, 5, 4, 3));
            Assert.AreEqual("later", actual.Value);
            Assert.AreEqual(6, actual.Key);
        }
    }
}
