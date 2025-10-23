using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.CsvBookBuilderTests
{
    [TestFixture]
    public class CsvBookBuilderExceptionsTest
    {
        private CsvBookBuilder csvBookBuilder;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;
        private string header;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            formatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns<string>(value => value.ToString());

            var csvBookFactoryMoq = new Mock<ICsvBookFactory>();
            csvBookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => new CsvBook());

            csvBookBuilder = new CsvBookBuilder(csvBookFactoryMoq.Object, new BookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 10,
            });

            header = ExceptionUtil.CreateExceptionHeader<int, int, IEnumerable<ICsvDataColumn>, CancellationToken, Task>(csvBookBuilder.BuildBookAsync);
        }

        [Test]
        public void ShouldThrowArgumentOutOfRangeExceptionWhenStartRowLessThanZero()
            => Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => csvBookBuilder.BuildBookAsync(-1, 1, new List<ICsvDataColumn>()),
                $"{header} Error: Starting row index cannot be less than zero.");

        [Test]
        public void ShouldThrowNullReferenceExceptionNullColumns()
            => Assert.ThrowsAsync<NullReferenceException>(
                () => csvBookBuilder.BuildBookAsync(0, 1, null),
                $"{header} Error: Columns parameter cannot be null.");

        [Test]
        public void ShouldThrowArgumentExceptionWhenColumnCountIsZero()
            => Assert.ThrowsAsync<ArgumentException>(
                () => csvBookBuilder.BuildBookAsync(0, 1, new List<ICsvDataColumn>()),
                $"{header} Error: Columns parameter cannot have zero columns.");
    }
}