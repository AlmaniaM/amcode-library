using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Util;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Exports.UnitTests.BookBuilder.Mocks;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.BookCompilerTests
{
    [TestFixture]
    public class BookCompilerExceptionsTest
    {
        private BookCompiler bookCompiler;
        private Mock<IBookBuilderFactory> bookFactoryMoq;
        private IEnumerable<TestDataColumn> columns;

        [SetUp]
        public void SetUp()
        {
            bookFactoryMoq = new Mock<IBookBuilderFactory>();

            columns = Enumerable.Range(1, 1).Select(index => new TestDataColumn { WorksheetHeaderName = $"Column{index}" });

            bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldThrowLessThanEqualToZeroExceptionWhenTotalRowsZeroOrLess(int totalRowCount)
        {
            Assert.ThrowsAsync<ArgumentException>(
                () => bookCompiler.CompileBookAsync("TestBook", totalRowCount, new List<TestDataColumn>(), FileType.Xlsx),
                ExportsExceptionUtil.CreateLessThanEqualToZeroExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<string, int, IEnumerable<IBookDataColumn>, FileType, CancellationToken, Task>(bookCompiler.CompileBookAsync),
                    "totalRowCount"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenColumnsAreNull()
        {
            Assert.ThrowsAsync<NullReferenceException>(
                () => bookCompiler.CompileBookAsync("TestBook", 1, null, FileType.Xlsx),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<string, int, IEnumerable<IBookDataColumn>, FileType, CancellationToken, Task>(bookCompiler.CompileBookAsync),
                    "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenColumnCountIsZero()
        {
            Assert.ThrowsAsync<ArgumentException>(
                () => bookCompiler.CompileBookAsync("TestBook", 1, new List<TestDataColumn>(), FileType.Xlsx),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<string, int, IEnumerable<IBookDataColumn>, FileType, CancellationToken, Task>(bookCompiler.CompileBookAsync),
                    "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenFileTypeIsNotValid()
        {
            var header = ExceptionUtil.CreateExceptionHeader<string, int, IEnumerable<IBookDataColumn>, FileType, CancellationToken, Task>(bookCompiler.CompileBookAsync);
            Assert.ThrowsAsync<ArgumentException>(
                () => bookCompiler.CompileBookAsync("TestBook", 1, new List<TestDataColumn> { null }, (FileType)(-1)),
                $"{header} Error: Parameter \"fileType\" has a value of \"-1\" which does not exit in the type {typeof(FileType)}"
            );
        }
    }
}