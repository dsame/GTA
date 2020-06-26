using System;
using Windows.UI;
using EG.Sherpany.Boardroom.Helper;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Helper
{
    [TestClass]
    public class ColorHelperTests
    {
        [TestMethod]
        public void FromHexEmptyThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ColorHelpers.FromHex(""));
        }

        [TestMethod]
        public void FromHexNoHashThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ColorHelpers.FromHex("12345678"));
        }

        [TestMethod]
        public void FromHexCorrectCodeReturnsColor()
        {
            var color = ColorHelpers.FromHex("#FFFFFFFF");
            Assert.AreEqual(Colors.White,color);
        }
    }
}
