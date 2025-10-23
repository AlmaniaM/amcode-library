using System;
using System.Collections.Generic;
using AMCode.Common.IO.CSV;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions;
using AMCode.Exports.Common.Exceptions.Util;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Book.CsvBookTests
{
    [TestFixture]
    public class CsvBookColumnsTest
    {
        private CsvBook csvBook;

        [SetUp]
        public void SetUp() => csvBook = new CsvBook();

        [TearDown]
        public void TearDown() => csvBook.Dispose();

        [Test]
        public void ShouldSetColumns()
        {
            csvBook.SetColumns(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });

            var csvHeaders = new CSVReader().GetHeaders(csvBook.Save());

            Assert.AreEqual("Column1", csvHeaders[0]);
            Assert.AreEqual("Column2", csvHeaders[1]);
            Assert.AreEqual("Column3", csvHeaders[2]);
            Assert.AreEqual("Column4", csvHeaders[3]);
            Assert.AreEqual("Column5", csvHeaders[4]);
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylesIsNullForSetColumns()
        {
            Assert.Throws<NullReferenceException>(
                () => csvBook.SetColumns(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(csvBook.SetColumns), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenCollectionIsEmptyForSetColumns()
        {
            Assert.Throws<EmptyCollectionException>(
                () => csvBook.SetColumns(new List<string>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(csvBook.SetColumns), "columns"
                )
            );
        }
    }
}