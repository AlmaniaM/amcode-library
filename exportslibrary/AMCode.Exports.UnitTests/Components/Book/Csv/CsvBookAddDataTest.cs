using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions;
using AMCode.Exports.Common.Exceptions.Util;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Book.CsvBookTests
{
    [TestFixture]
    public class CsvBookAddDataTest
    {
        private CsvBook csvBook;
        private readonly Mock<IColumnValueFormatter<object, string>> formatterMoq = new();

        [SetUp]
        public void SetUp()
        {
            formatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns<string>(value => value.ToString());
            csvBook = new CsvBook();
        }

        [TearDown]
        public void TearDown() => csvBook.Dispose();

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSetDataWithoutColumns(bool withColumns)
        {
            var data = Enumerable.Range(0, 10).Select(index =>
                new ExpandoObject()
                    .AddOrUpdatePropertyWithValue("Column1", "1")
                    .AddOrUpdatePropertyWithValue("Column2", 2)
                    .AddOrUpdatePropertyWithValue("Column3", 3.3)
                    .AddOrUpdatePropertyWithValue("Column4", true)
            ).ToList();

            var columns = new List<ICsvDataColumn>
            {
                new CsvDataColumn { Formatter = formatterMoq.Object, DataFieldName = "Column1", WorksheetHeaderName = "Column 1" },
                new CsvDataColumn { Formatter = formatterMoq.Object, DataFieldName = "Column2", WorksheetHeaderName = "Column 2" },
                new CsvDataColumn { Formatter = formatterMoq.Object, DataFieldName = "Column3", WorksheetHeaderName = "Column 3" },
                new CsvDataColumn { Formatter = formatterMoq.Object, DataFieldName = "Column4", WorksheetHeaderName = "Column 4" }
            };

            if (withColumns)
            {
                csvBook.SetColumns(columns.Select(column => column.WorksheetHeaderName).ToList());
            }

            csvBook.AddData(data, columns, new CancellationToken());

            var csvData = new CSVReader(false, (index) => $"Column{index + 1}").GetExpandoList(csvBook.Save(), ",", true);

            var startRow = withColumns ? 1 : 0;
            var endRow = withColumns ? 10 : 9;

            var rowOne = csvData[startRow];
            Assert.AreEqual("1", rowOne.GetValue<string>("Column1"));
            Assert.AreEqual("1", csvData[endRow].GetValue<string>("Column1"));
            Assert.AreEqual(2, csvData[startRow].GetValue<int>("Column2"));
            Assert.AreEqual(2, csvData[endRow].GetValue<int>("Column2"));
            Assert.AreEqual(3.3, csvData[startRow].GetValue<double>("Column3"));
            Assert.AreEqual(3.3, csvData[endRow].GetValue<double>("Column3"));
            Assert.AreEqual(true, csvData[startRow].GetValue<bool>("Column4"));
            Assert.AreEqual(true, csvData[endRow].GetValue<bool>("Column4"));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenDataIsNull()
        {
            Assert.Throws<NullReferenceException>(
                () => csvBook.AddData(null, new List<ICsvDataColumn>(), new CancellationToken()),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<ICsvDataColumn>, CancellationToken>(csvBook.AddData), "dataList"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenDataIsEmpty()
        {
            Assert.Throws<EmptyCollectionException>(
                () => csvBook.AddData(new List<ExpandoObject>(), new List<ICsvDataColumn>(), new CancellationToken()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<ICsvDataColumn>, CancellationToken>(csvBook.AddData), "dataList"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenColumnsIsNull()
        {
            Assert.Throws<NullReferenceException>(
                () => csvBook.AddData(new List<ExpandoObject> { new ExpandoObject() }, null, new CancellationToken()),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<ICsvDataColumn>, CancellationToken>(csvBook.AddData), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenColumnsIsEmpty()
        {
            Assert.Throws<EmptyCollectionException>(
                () => csvBook.AddData(new List<ExpandoObject> { new ExpandoObject() }, new List<ICsvDataColumn>(), new CancellationToken()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<ICsvDataColumn>, CancellationToken>(csvBook.AddData), "columns"
                )
            );
        }

        [Test]
        public async Task ShouldCancelAddDataOperation()
        {
            var cancellationSource = new CancellationTokenSource();

            var data = Enumerable.Range(0, 20000).Select(index => new ExpandoObject().AddOrUpdatePropertyWithValue("Column1", "1")).ToList();

            var columns = new List<ICsvDataColumn>
            {
                new CsvDataColumn { Formatter = formatterMoq.Object, DataFieldName = "Column1", WorksheetHeaderName = "Column 1" }
            };

            var task = Task.Run(() =>
            {
                Thread.Sleep(5);
                cancellationSource.Cancel();
            });

            Assert.Throws<OperationCanceledException>(() => csvBook.AddData(data, columns, cancellationSource.Token));

            await task;
        }
    }
}