using System;
using Windows.UI.Xaml.Media.Imaging;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class ByteArrayToImageConverterTests
    {
        [TestMethod]
        public void ConvertWithNullReturnsNull()
        {
            var converter = new ByteArrayToImageConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.IsNull(actual);
        }
        [UITestMethod]
        public void ConvertWithValueReturnsImage()
        {
            var byteArray =
                Convert.FromBase64String(
                    "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAMCAgMCAgMDAwMEAwMEBQgFBQQEBQoHBwYIDAoMDAsKCwsNDhIQD" +
                    "Q4RDgsLEBYQERMUFRUVDA8XGBYUGBIUFRT/2wBDAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUF" +
                    "BQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wAARCAABAAEDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQE" +
                    "AAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEI" +
                    "I0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4e" +
                    "XqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6er" +
                    "x8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AA" +
                    "ECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU" +
                    "1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHy" +
                    "MnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDweiiivkj/AFFP/9k=");
            var converter = new ByteArrayToImageConverter();
            var actual = converter.Convert(byteArray, typeof(bool), null, "");
            Assert.IsInstanceOfType(actual, typeof(BitmapImage));
            Assert.AreEqual(1, ((BitmapImage)actual).PixelHeight);
            Assert.AreEqual(1, ((BitmapImage)actual).PixelWidth);
        }

        [TestMethod]
        public void ConvertBackThrowsException()
        {
            var converter = new ByteArrayToImageConverter();
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(true, typeof(bool), null, ""));
        }
    }
}
