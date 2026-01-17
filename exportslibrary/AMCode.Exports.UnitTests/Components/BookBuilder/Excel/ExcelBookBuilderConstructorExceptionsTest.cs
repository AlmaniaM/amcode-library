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

namespace AMCode.Exports.UnitTests.BookBuilder.ExcelBookBuilderTests
{
    [TestFixture]
    public class ExcelBookBuilderConstructorExceptionsTest
    {
        private ExcelBookBuilderConfig builderConfig;
        private Mock<IExcelBookFactory> bookFactoryMoq;
        private ExcelBookBuilder excelBookBuilder;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            bookFactoryMoq = new();

            builderConfig = new ExcelBookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 1,
            };

            excelBookBuilder = new ExcelBookBuilder(bookFactoryMoq.Object, builderConfig);
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenBookFactoryIsNull()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig>();

            var exception = Assert.Throws<NullReferenceException>(() => new ExcelBookBuilder(null, builderConfig));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IExcelBookFactory)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenBookBuilderConfigIsNull()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig>();

            var exception = Assert.Throws<NullReferenceException>(() => new ExcelBookBuilder(bookFactoryMoq.Object, null));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IBookBuilderConfig)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenFetchDataFunctionIsNull()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig>();

            builderConfig.FetchDataAsync = null;

            var exception = Assert.Throws<NullReferenceException>(() => new ExcelBookBuilder(bookFactoryMoq.Object, builderConfig));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(builderConfig.FetchDataAsync)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenMaxRowsPerDataFetchFileIsZero()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig>();

            builderConfig.MaxRowsPerDataFetch = -1;

            var exception = Assert.Throws<ArgumentException>(() => new ExcelBookBuilder(bookFactoryMoq.Object, builderConfig));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(builderConfig.MaxRowsPerDataFetch)}\" cannot be less than or equal to zero."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenIExportStreamDataSourceFactoryIsNull()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExportStreamDataSourceFactory>();

            var exception = Assert.Throws<NullReferenceException>(() => new ExcelBookBuilder(bookFactoryMoq.Object, builderConfig, (IExportStreamDataSourceFactory)null));

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IExportStreamDataSourceFactory)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenIExcelBookStylerIsNullFullConstructor()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExcelBookStyler, IExportStreamDataSourceFactory>();

            var exception = Assert.Throws<NullReferenceException>(
                () => new ExcelBookBuilder(bookFactoryMoq.Object, builderConfig, null, null)
            );

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IExcelBookStyler)}\" cannot be null."));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenIExportStreamDataSourceFactoryIsNullFullConstructor()
        {
            var expectedHeader = ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExcelBookStyler, IExportStreamDataSourceFactory>();

            var exception = Assert.Throws<NullReferenceException>(
                () => new ExcelBookBuilder(bookFactoryMoq.Object, builderConfig, new Mock<IExcelBookStyler>().Object, null)
            );

            Assert.That(exception.Message, Is.EqualTo($"{expectedHeader} Error: Parameter for \"{nameof(IExportStreamDataSourceFactory)}\" cannot be null."));
        }
    }
}