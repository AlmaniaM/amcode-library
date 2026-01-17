using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.DataSource;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.CsvBookBuilderTests
{
    [TestFixture]
    public class CsvBookBuilderConstructorExceptionsTest
    {
        private BookBuilderConfig builderConfig;
        private Mock<ICsvBookFactory> bookFactoryMoq;
        private CsvBookBuilder csvBookBuilder;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;
        private string expectedHeader;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            bookFactoryMoq = new();

            builderConfig = new BookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 1,
            };

            csvBookBuilder = new CsvBookBuilder(bookFactoryMoq.Object, builderConfig);

            expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<CsvBookBuilder, ICsvBookFactory, IBookBuilderConfig>();
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenBookFactoryIsNull()
        {
            var exception = Assert.Throws<NullReferenceException>(() => new CsvBookBuilder(null, builderConfig));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IExcelBookFactory)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenBookBuilderConfigIsNull()
        {
            var exception = Assert.Throws<NullReferenceException>(() => new CsvBookBuilder(bookFactoryMoq.Object, null));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IBookBuilderConfig)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenFetchDataFunctionIsNull()
        {
            builderConfig.FetchDataAsync = null;

            var exception = Assert.Throws<NullReferenceException>(() => new CsvBookBuilder(bookFactoryMoq.Object, builderConfig));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(builderConfig.FetchDataAsync)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenMaxRowsPerDataFetchFileIsZero()
        {
            builderConfig.MaxRowsPerDataFetch = -1;

            var exception = Assert.Throws<ArgumentException>(() => new CsvBookBuilder(bookFactoryMoq.Object, builderConfig));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(builderConfig.MaxRowsPerDataFetch)}\" cannot be less than or equal to zero."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenIExportStreamDataSourceFactoryIsNull()
        {
            expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<CsvBookBuilder, ICsvBookFactory, IBookBuilderConfig, IExportStreamDataSourceFactory>();

            var exception = Assert.Throws<NullReferenceException>(() => new CsvBookBuilder(bookFactoryMoq.Object, builderConfig, null));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IExportStreamDataSourceFactory)}\" cannot be null."));
        }
    }
}