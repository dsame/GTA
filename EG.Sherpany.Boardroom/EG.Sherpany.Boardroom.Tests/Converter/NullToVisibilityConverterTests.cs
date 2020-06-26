using System;
using Windows.UI.Xaml;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class NullToVisibilityConverterTests
    {
        [TestMethod]
        public void ConvertWithNullReturnsVisible()
        {
            var converter = new NullToVisibilityConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.AreEqual(Visibility.Visible, actual);
        }
        [TestMethod]
        public void ConvertWithValueReturnsCollapsed()
        {
            var converter = new NullToVisibilityConverter();
            var actual = converter.Convert(1, typeof(bool), null, "");
            Assert.AreEqual(Visibility.Collapsed,actual);
        }

        [TestMethod]
        public void ConvertWithNullAndParameterReturnsCollapsed()
        {
            var converter = new NullToVisibilityConverter();
            var actual = converter.Convert(null, typeof(bool), 1, "");
            Assert.AreEqual(Visibility.Collapsed, actual);
        }

        [TestMethod]
        public void ConvertWithValueAndParameterReturnsVisible()
        {
            var converter = new NullToVisibilityConverter();
            var actual = converter.Convert(1, typeof(bool), 1, "");
            Assert.AreEqual(Visibility.Visible, actual);
        }

        [TestMethod]
        public void ConvertBackThrowsException()
        {
            var converter = new NullToVisibilityConverter();
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(true, typeof(bool), null, ""));
        }


    }
}
