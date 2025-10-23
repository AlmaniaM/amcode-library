using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using DlExportsLibrary.UnitTests.BookBuilder.Mocks;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.BookBuilderCommonTests
{
    [TestFixture]
    public class BookBuilderCommonDataTest
    {
        private Mock<IBook<TestDataColumn>> bookMoq;
        private Mock<IBookBuilderConfig> builderConfigMoq;
        private readonly int columnCount = 2;
        private IList<TestDataColumn> columns;
        private BookBuilderCommon<TestDataColumn> commonBuilder;
        private Mock<ExportDataRangeFetch> fetchDataMoq;
        private readonly int maxRowsPerDataRequest = 10;
        private readonly Mock<IEqualityComparer<IList<TestDataColumn>>> columnComparerMoq = new();

        [SetUp]
        public void SetUp()
        {
            columns = Enumerable.Range(1, columnCount).Select(index => new TestDataColumn
            {
                WorksheetHeaderName = $"Column{index}"
            }).ToList();

            bookMoq = new();

            var bookFactoryMoq = new Mock<IBookFactory<TestDataColumn>>();
            bookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => bookMoq.Object);

            fetchDataMoq = new();
            fetchDataMoq.Setup(_ => _(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((int start, int count, CancellationToken cancellation) => Task.Run<IList<ExpandoObject>>(() =>
                {
                    var data = Enumerable.Range(start, count)
                    .Select(index =>
                    {
                        var row = new ExpandoObject();

                        columns.ForEach((column, i) => row.AddOrUpdatePropertyWithValue(column.WorksheetHeaderName, $"Value {i + 1}{index}"));

                        return row;
                    }).ToList();

                    return data;
                })
            );

            builderConfigMoq = new();
            builderConfigMoq.SetupGet(moq => moq.MaxRowsPerDataFetch).Returns(() => maxRowsPerDataRequest);
            builderConfigMoq.SetupGet(moq => moq.FetchDataAsync).Returns(() => fetchDataMoq.Object);

            commonBuilder = new BookBuilderCommon<TestDataColumn>(bookFactoryMoq.Object, builderConfigMoq.Object);

            columnComparerMoq.Setup(moq => moq.GetHashCode(It.IsAny<IList<TestDataColumn>>())).Returns<IList<TestDataColumn>>(columns => columns.GetHashCode());
            columnComparerMoq.Setup(
                moq => moq.Equals(
                    It.IsAny<IList<TestDataColumn>>(),
                    It.IsAny<IList<TestDataColumn>>()
                    )
                ).Returns<IList<TestDataColumn>, IList<TestDataColumn>>((c1, c2) => c1.Where(c => c2.Contains(c)).Count() == c1.Count);
        }

        [TestCase(0, 10, 1, 10)]
        [TestCase(10, 20, 2, 10)]
        [TestCase(100, 101, 11, 1)]
        [TestCase(100, 1, 1, 1)]
        [TestCase(0, 13, 2, 3)]
        public async Task ShouldCrateExcelBook(int start, int count, int expectedTimesCalledForData, int expectedLastCallCount)
        {
            var startingPoints = commonBuilder.CalculateStartingRows(start, count);
            // Make sure we have correct count of starting points at least
            Assert.AreEqual((int)Math.Ceiling((double)count / maxRowsPerDataRequest), startingPoints.Count);

            await commonBuilder.AddBookData(bookMoq.Object, startingPoints, count, columns, new CancellationToken());

            fetchDataMoq.Verify(_ => _(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(expectedTimesCalledForData));

            for (var i = 0; i < startingPoints.Count; i++)
            {
                if (i < startingPoints.Count - 1)
                {
                    fetchDataMoq.Verify(_ => _(It.Is<int>(start => start == startingPoints[i]), It.Is<int>(count => count == maxRowsPerDataRequest), It.IsAny<CancellationToken>()), Times.Once());
                    bookMoq.Verify(
                        moq => moq.AddData(
                            It.Is<IList<ExpandoObject>>(data => data.Count == maxRowsPerDataRequest),
                            It.Is(columns, columnComparerMoq.Object),
                            It.IsAny<CancellationToken>()));
                }
                else
                {
                    fetchDataMoq.Verify(_ => _(It.Is<int>(start => start == startingPoints[i]), It.Is<int>(count => count == expectedLastCallCount), It.IsAny<CancellationToken>()), Times.Once());
                    bookMoq.Verify(
                        moq => moq.AddData(
                            It.Is<IList<ExpandoObject>>(data => data.Count == expectedLastCallCount),
                            It.Is(columns, columnComparerMoq.Object),
                            It.IsAny<CancellationToken>()));
                }
            }
        }
    }
}