using AMCode.Exports;
using AMCode.Exports.Extensions;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.Extensions.FileTypeExtensionTests
{
    [TestFixture]
    public class FileTypeExtensionTest
    {
        [Test]
        public void ShouldCreateCsvFileExtension()
            => Assert.AreEqual(".csv", FileType.Csv.CreateFileExtension());

        [Test]
        public void ShouldCreateXlsxFileExtension()
            => Assert.AreEqual(".xlsx", FileType.Xlsx.CreateFileExtension());

        [Test]
        public void ShouldCreateZipFileExtension()
            => Assert.AreEqual(".zip", FileType.Zip.CreateFileExtension());
    }
}