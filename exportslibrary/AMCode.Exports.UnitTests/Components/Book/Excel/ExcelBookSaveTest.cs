using System.Collections.Generic;
using System.IO;
using AMCode.Exports.Book;
using AMCode.Xlsx;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Book.ExcelBookTests
{
    [TestFixture]
    public class ExcelBookSaveTest
    {
        private IExcelApplication excelApplication;
        private ExcelBook excelBook;

        [SetUp]
        public void SetUp()
        {
            excelApplication = new ExcelApplication();
            excelBook = new ExcelBook(excelApplication);
        }

        [TearDown]
        public void TearDown() => excelApplication.Dispose();

        [Test]
        public void ShouldGetStreamWhenSaving()
        {
            excelBook.SetColumns(new List<string> { "Column1" });
            var stream = excelBook.Save();
            Assert.IsTrue(stream.Length > 0);
        }

        [Test]
        public void ShouldSaveAsProvidedStream()
        {
            excelBook.SetColumns(new List<string> { "Column1" });

            var stream = new MemoryStream();

            Assert.AreEqual(0, stream.Length);

            excelBook.SaveAs(stream);

            Assert.IsTrue(stream.Length > 0);
        }
    }
}