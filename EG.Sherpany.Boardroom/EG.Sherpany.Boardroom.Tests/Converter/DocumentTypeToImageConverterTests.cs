using System;
using System.Collections.Generic;
using System.Globalization;
using EG.Sherpany.Boardroom.Converter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Converter
{
    [TestClass]
    public class DocumentTypeToImageConverterTests
    {
        [TestMethod]
        public void ConvertWithNullReturnsNull()
        {
            var converter = new DocumentTypeToImageConverter();
            var actual = converter.Convert(null, typeof(bool), null, "");
            Assert.IsNull(actual);
        }
        [TestMethod]
        public void ConvertWithNoFormatReturnsNull()
        {
            var converter = new DocumentTypeToImageConverter();
            var actual = converter.Convert("", typeof(bool), null, "");
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ConvertWithFormatReturnsImageFile()
        {
            var formats = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pdf", "pdf 24x24"),
                new KeyValuePair<string, string>("doc", "word 24x24"),
                new KeyValuePair<string, string>("docx", "word 24x24"),
                new KeyValuePair<string, string>("html", "html 24x24"),
                new KeyValuePair<string, string>("xls", "excel 24x24"),
                new KeyValuePair<string, string>("xlsx", "excel 24x24"),
                new KeyValuePair<string, string>("txt", "txt 24x24"),
                new KeyValuePair<string, string>("png", "png 24x24"),
                new KeyValuePair<string, string>("gif", "gif 24x24"),
                new KeyValuePair<string, string>("bmp", "bmp 24x24"),
                new KeyValuePair<string, string>("jpg", "jpg 24x24"),
                new KeyValuePair<string, string>("meeting", "meeting 24x24"),
                new KeyValuePair<string, string>("pptx", "powerpoint 24x24"),
                new KeyValuePair<string, string>("xxx", "file 24x24"),
            };
            var converter = new DocumentTypeToImageConverter();
            foreach (KeyValuePair<string, string> format in formats)
            {
                var actual = converter.Convert(format.Key, typeof(bool), null, "");
                Assert.IsTrue(((string)actual).ToLower().Contains(format.Value));
            }
        }

        [TestMethod]
        public void ConvertBackThrowsException()
        {
            var converter = new DocumentTypeToImageConverter();
            Assert.ThrowsException<NotImplementedException>(() => converter.ConvertBack(true, typeof(bool), null, ""));
        }


    }
}
