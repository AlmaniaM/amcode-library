using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using DlExportsLibrary.UnitTests.BookBuilder.Mocks;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.BookBuilderCommonTests
{
    [TestFixture]
    public class BookBuilderCommonHelpersTest
    {
        private BookBuilderCommon<TestDataColumn> exportBuilder;
        private BookBuilderConfig builderConfig;

        [SetUp]
        public void SetUp()
        {
            var bookFactoryMoq = new Mock<IBookFactory<TestDataColumn>>();
            bookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => null);

            builderConfig = new BookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
                MaxRowsPerDataFetch = 10,
            };

            exportBuilder = new BookBuilderCommon<TestDataColumn>(bookFactoryMoq.Object, builderConfig);
        }

        [TestCase(10, 0, 100, 10)]
        [TestCase(100, 100, 100, 1)]
        [TestCase(100000, 0, 1000000, 10)]
        [TestCase(100000, 0, 1048576, 11)]
        public void ShouldGetCorrectStartingPointsCount(int rowsPerRequest, int startRow, int totalRows, int expectedCount)
        {
            builderConfig.MaxRowsPerDataFetch = rowsPerRequest;

            var startingPoints = exportBuilder.CalculateStartingRows(startRow, totalRows);
            Assert.AreEqual(expectedCount, startingPoints.Count);
        }

        [TestCase(10, 0, 100, 10)]
        [TestCase(100, 100, 100, 1)]
        [TestCase(100000, 0, 1000000, 10)]
        [TestCase(100000, 0, 1048576, 11)]
        public void ShouldGetCorrectStartingPointsValues(int rowsPerRequest, int startRow, int totalRows, int expectedCount)
        {
            builderConfig.MaxRowsPerDataFetch = rowsPerRequest;

            var startingPoints = exportBuilder.CalculateStartingRows(startRow, totalRows);

            Enumerable.Range(0, expectedCount).ForEach(index => Assert.AreEqual(startRow + (rowsPerRequest * index), startingPoints[index]));
        }
    }
}