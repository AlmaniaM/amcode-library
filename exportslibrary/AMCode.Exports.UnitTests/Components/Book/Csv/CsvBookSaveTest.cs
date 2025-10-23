using System.Collections.Generic;
using System.IO;
using AMCode.Exports.Book;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Book.CsvBookTests
{
    [TestFixture]
    public class CsvBookSaveTest
    {
        private CsvBook csvBook;

        [SetUp]
        public void SetUp() => csvBook = new CsvBook();

        [TearDown]
        public void TearDown() => csvBook.Dispose();

        [Test]
        public void ShouldGetStreamWhenSaving()
        {
            csvBook.SetColumns(new List<string> { "Column1" });
            var stream = csvBook.Save();
            Assert.IsTrue(stream.Length > 0);
        }

        [Test]
        public void ShouldSaveAsProvidedStream()
        {
            csvBook.SetColumns(new List<string> { "Column1" });

            var stream = new MemoryStream();

            Assert.AreEqual(0, stream.Length);

            csvBook.SaveAs(stream);

            Assert.IsTrue(stream.Length > 0);
        }
    }
}