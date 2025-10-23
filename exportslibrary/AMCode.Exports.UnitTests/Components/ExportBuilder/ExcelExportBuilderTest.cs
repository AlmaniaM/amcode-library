using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.ExportBuilder;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.ExportBuilder.ExcelExportBuilderTests
{
    [TestFixture]
    public class ExcelExportBuilderTest
    {
        private ExcelBookBuilderConfig builderConfig;
        private IEnumerable<IExcelDataColumn> columns;
        private readonly IList<IExcelBookStyleAction> stylers = new List<IExcelBookStyleAction>();
        private ExcelExportBuilder exportBuilder;

        [SetUp]
        public void SetUp()
        {
            columns = new List<IExcelDataColumn>();
            builderConfig = new()
            {
                FetchDataAsync = (start, count, _) => Task.Run<IList<ExpandoObject>>(() => new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 10
            };
        }

        [TestCase(91, 10, 10)]
        [TestCase(90, 10, 9)]
        [TestCase(9, 10, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 1000000, 1)]
        [TestCase(1000000, 1000000, 1)]
        [TestCase(1000000, 1000001, 1)]
        public void ShouldCalculateNumberOfBooks(int numberOfRows, int maxRowsPerBook, int expectedNumberOfBooks)
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, stylers, maxRowsPerBook);
            Assert.That(exportBuilder.CalculateNumberOfBooks(numberOfRows), Is.EqualTo(expectedNumberOfBooks));
        }
    }
}