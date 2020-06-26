using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class ItemCountToEmptyLabelVisibilityVisibilityConverterTests
    {
        [TestMethod]
        public void ConvertWithNullReturnsCollapsed()
        {
            var converter = new ItemCountToEmptyLabelVisibilityConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.AreEqual(Visibility.Collapsed, actual);
        }
        [TestMethod]
        public void ConvertWithZeroReturnsVisible()
        {
            var converter = new ItemCountToEmptyLabelVisibilityConverter();
            var actual = converter.Convert(0, typeof(bool), null, "");
            Assert.AreEqual(Visibility.Visible,actual);
        }

        [TestMethod]
        public void ConvertWithValueReturnsCollapsed()
        {
            var converter = new ItemCountToEmptyLabelVisibilityConverter();
            var actual = converter.Convert(1, typeof(bool), null, "");
            Assert.AreEqual(Visibility.Collapsed, actual);
        }

        [TestMethod]
        public void ConvertWithNonIntReturnsCollapsed()
        {
            var converter = new ItemCountToEmptyLabelVisibilityConverter();
            var actual = converter.Convert("abc", typeof(bool), null, "");
            Assert.AreEqual(Visibility.Collapsed, actual);
        }

        [TestMethod]
        public void ConvertBackReturnsNull()
        {
            var converter = new ItemCountToEmptyLabelVisibilityConverter();
            var actual = converter.ConvertBack("abc", typeof(bool), null, "");
            Assert.IsNull(actual);
        }


    }
}
