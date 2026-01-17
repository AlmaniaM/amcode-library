using System;
using System.Linq;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.BookBuilder.Actions;
using AMCode.Exports.Common.Exceptions.Util;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.ActionTests
{
    [TestFixture]
    public class ApplyColumnWidthActionTest
    {
        private ApplyColumnWidthAction columnWidthAction;
        private Mock<IExcelBook> excelBookMoq;
        private string header;
        private Mock<IColumnStyleActionData> styleDataMoq;

        [SetUp]
        public void SetUp()
        {
            columnWidthAction = new ApplyColumnWidthAction();
            excelBookMoq = new();
            styleDataMoq = new();

            header = ExceptionUtil.CreateExceptionHeader<IExcelBook, IStyleActionData>(columnWidthAction.Style);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(100)]
        public void ShouldApplyColumnWidths(int columnCount)
        {
            styleDataMoq.Setup(moq => moq.ColumnStyles).Returns(() => Enumerable.Range(1, columnCount).Select(index => new ColumnStyle
            {
                Name = $"Column{index}",
                Width = index * 10.0
            }));

            columnWidthAction.Style(excelBookMoq.Object, styleDataMoq.Object);

            for (var i = 1; i <= columnCount; i++)
            {
                excelBookMoq.Verify(moq => moq.SetColumnWidthInPixelsAllSheets(It.Is<string>(columnName => columnName.Equals($"Column{i}")), It.Is<double>(width => width == i * 10)), Times.Once());
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenExcelBookIsNull()
            => Assert.Throws<NullReferenceException>(
                () => columnWidthAction.Style(null, styleDataMoq.Object),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "book"));

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStyleDataIsNull()
            => Assert.Throws<NullReferenceException>(
                () => columnWidthAction.Style(excelBookMoq.Object, null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "styleData"));
    }
}