using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class BoolToBrushConverterTests
    {
        [UITestMethod]
        public void ConvertWithNullReturnsFalseBrush()
        {
            var converter = CreateConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.IsInstanceOfType(actual, typeof(Brush));
            Assert.AreEqual(converter.FalseBrush, actual);
        }

        private static BoolToBrushConverter CreateConverter()
        {
            var converter = new BoolToBrushConverter
            {
                FalseBrush = new SolidColorBrush(Colors.Red),
                TrueBrush = new SolidColorBrush(Colors.Green)
            };
            return converter;
        }

        [UITestMethod]
        public void ConvertWithTrueReturnsTrueBrush()
        {
            var converter = CreateConverter();
            var actual = converter.Convert(true, typeof(bool), null, "");
            Assert.IsInstanceOfType(actual, typeof(Brush));
            Assert.AreEqual(converter.TrueBrush, actual);
        }

        [UITestMethod]
        public void ConvertWithFalseReturnsFalseBrush()
        {
            var converter = CreateConverter();
            var actual = converter.Convert(false, typeof(bool), null, "");
            Assert.IsInstanceOfType(actual, typeof(Brush));
            Assert.AreEqual(converter.FalseBrush, actual);
        }

        [UITestMethod]
        public void ConvertBackThrowsException()
        {
            var converter = CreateConverter();
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(true, typeof(bool), null, ""));
        }


    }
}
