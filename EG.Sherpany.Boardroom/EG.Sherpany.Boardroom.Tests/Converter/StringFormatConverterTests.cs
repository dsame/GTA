using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class StringFormatConverterTests
    {
        [TestMethod]
        public void ConvertWithNullReturnsNull()
        {
            var converter = new StringFormatConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.IsNull(actual);
        }
        [TestMethod]
        public void ConvertWithValueAndNoParameterReturnsValue()
        {
            var converter = new StringFormatConverter();
            var actual = converter.Convert(1, typeof(bool), null, "");
            Assert.AreEqual(1,actual);
        }

        [TestMethod]
        public void ConvertWithValueAndParameterReturnsFormatedValue()
        {
            var converter = new StringFormatConverter();
            var actual = converter.Convert(1.0, typeof(bool), "{0:0.0}", "");
            Assert.AreEqual("1.0", actual);
        }

        [TestMethod]
        public void ConvertWithDateAndParameterReturnsFormatedValue()
        {
            var converter = new StringFormatConverter();
            var actual = converter.Convert(new DateTime(2000,1,2,3,4,5,6), typeof(bool), "{0:dd/MM/yyyy hh:mm:ss}", "");
            Assert.AreEqual("02.01.2000 03:04:05", actual);
        }

        [TestMethod]
        public void ConvertBackThrowsException()
        {
            var converter = new StringFormatConverter();
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(true, typeof(bool), null, ""));
        }


    }
}
