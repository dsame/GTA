using EG.Sherpany.Boardroom.Helper;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace EG.Sherpany.Boardroom.Tests.Helper
{
    [TestClass]
    public class ByteHelperTests
    {
        [TestMethod]
        public void ByteTextZeroReturnsZeroBytes()
        {
            Assert.AreEqual("0 bytes", 0L.GetByteText());
        }

        [TestMethod]
        public void ByteText1ReturnsOneBytes()
        {
            Assert.AreEqual("1 bytes", 1L.GetByteText());
        }

        [TestMethod]
        public void ByteText1024MultiplesReturnsCorrectSuffix()
        {
            string[] suffixes = { "KB", "MB", "GB", "TB", "PB", "EB"};
            var value = 1024L;
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual("1.0 "+suffixes[i], value.GetByteText());
                value *= 1024L;
            }
            
        }

        [TestMethod]
        public void ByteText1512MultplesReturnsCorrectSuffix()
        {
            string[] suffixes = { "KB", "MB", "GB", "TB", "PB", "EB" };
            var value = 1512L;
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual("1.5 " + suffixes[i], value.GetByteText());
                value *= 1024L;
            }

        }
    }
}
