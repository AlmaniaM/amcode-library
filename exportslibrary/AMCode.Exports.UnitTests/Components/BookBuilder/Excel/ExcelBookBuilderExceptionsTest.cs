using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Documents.Xlsx;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.ExcelBookBuilderTests
{
    [TestFixture]
    public class ExcelBookBuilderExceptionsTest
    {
        private ExcelBookBuilderConfig builderConfig;
        private ExcelBookBuilder excelBookBuilder;
        private Mock<IExcelBookFactory> excelBookFactoryMoq;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;
        private string header;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            formatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns<string>(value => value.ToString());

            excelBookFactoryMoq = new Mock<IExcelBookFactory>();
            excelBookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => new ExcelBook(new ExcelApplication()));

            builderConfig = new ExcelBookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 1,
            };

            excelBookBuilder = new ExcelBookBuilder(excelBookFactoryMoq.Object, builderConfig);

            header = ExceptionUtil.CreateExceptionHeader<int, int, IEnumerable<IExcelDataColumn>, CancellationToken, Task>(excelBookBuilder.BuildBookAsync);
        }

        [Test]
        public void ShouldThrowArgumentOutOfRangeExceptionWhenStartRowLessThanZero()
            => Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => excelBookBuilder.BuildBookAsync(-1, 1, new List<IExcelDataColumn>()),
                $"{header} Error: Starting row index cannot be less than zero.");

        [Test]
        public void ShouldThrowNullReferenceExceptionNullColumns()
            => Assert.ThrowsAsync<NullReferenceException>(
                () => excelBookBuilder.BuildBookAsync(0, 1, null),
                $"{header} Error: Columns parameter cannot be null.");

        [Test]
        public void ShouldThrowArgumentExceptionWhenColumnCountIsZero()
            => Assert.ThrowsAsync<ArgumentException>(
                () => excelBookBuilder.BuildBookAsync(0, 1, new List<IExcelDataColumn>()),
                $"{header} Error: Columns parameter cannot have zero columns.");

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylersAreNull()
            => Assert.Throws<NullReferenceException>(
                () => new ExcelBookBuilder(excelBookFactoryMoq.Object, builderConfig, (IExcelBookStyler)null),
                $"{header} Error: Parameter for \"{nameof(IExcelBookStyler)}\" cannot be null.");
    }
}