using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.ExportBuilder;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.ExportBuilder.CsvExportBuilderTests
{
    [TestFixture]
    public class CsvExportBuilderTest
    {
        private IBookBuilderConfig builderConfig;
        private IEnumerable<ICsvDataColumn> columns;
        private CsvExportBuilder exportBuilder;

        [SetUp]
        public void SetUp()
        {
            var valueFormatterMoq = new Mock<IColumnValueFormatter<object, string>>();
            valueFormatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns((object obj) => obj.ToString());

            columns = new List<ICsvDataColumn>();

            builderConfig = new BookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.Run<IList<ExpandoObject>>(() => new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 10
            };
        }

        [TestCase(100, 1)]
        [TestCase(1, 1)]
        [TestCase(1000000, 1)]
        [TestCase(2000000, 1)]
        public void ShouldCalculateNumberOfBooks(int numberOfRows, int expectedNumberOfBooks)
        {
            exportBuilder = new CsvExportBuilder(builderConfig);
            Assert.That(exportBuilder.CalculateNumberOfBooks(numberOfRows), Is.EqualTo(expectedNumberOfBooks));
        }
    }
}