using System;
using AMCode.Exports;
using AMCode.Exports.Extensions;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.Extensions.ExportResultExtensionTests
{
    [TestFixture]
    public class ExportResultExtensionTest
    {
        [Test]
        public void ShouldCreateFileNameWithExtension()
        {
            IExportResult result = new InMemoryExportResult
            {
                Name = "TestFile",
                FileType = FileType.Xlsx
            };

            Assert.AreEqual("TestFile.xlsx", result.CreateFileName());
        }

        [Test]
        public void ShouldCreateFileNameWithExtensionWhenExtensionAlreadyExists()
        {
            IExportResult result = new InMemoryExportResult
            {
                Name = "TestFile.xlsx",
                FileType = FileType.Xlsx
            };

            Assert.AreEqual("TestFile.xlsx", result.CreateFileName());
        }

        [Test]
        public void ShouldCreateFileNameWithExtensionWithAppendedText()
        {
            var uuid = Guid.NewGuid().ToString();

            IExportResult result = new InMemoryExportResult
            {
                Name = "TestFile",
                FileType = FileType.Xlsx
            };

            Assert.AreEqual($"TestFile{uuid}.xlsx", result.CreateFileName(uuid));
        }
    }
}