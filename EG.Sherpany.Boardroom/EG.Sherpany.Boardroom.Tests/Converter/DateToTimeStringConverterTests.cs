using System;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class DateToTimeStringConverterTests
    {
        [TestMethod]
        public void ConvertWithNullReturnsEmptyString()
        {
            var converter = new DateToTimeStringConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.AreEqual("", actual);
        }
        [TestMethod]
        public void ConvertWithNonDateTimeReturnsEmptyString()
        {
            var converter = new DateToTimeStringConverter();
            var actual = converter.Convert(true, typeof(bool), null, "");
            Assert.AreEqual("", actual);
        }

        [TestMethod]
        public void ConvertWithDateTimeReturnsString()
        {
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "de-CH";
            var converter = new DateToTimeStringConverter();
            var actual = converter.Convert(new DateTime(2001,1,2,3,4,5,6), typeof(bool), null, "de-CH");
            Assert.AreEqual("03:04",actual);
        }

        [TestMethod]
        public void ConvertBackThrowsException()
        {
            var converter = new DateToTimeStringConverter();
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(true, typeof(bool), null, ""));
        }


    }
}
