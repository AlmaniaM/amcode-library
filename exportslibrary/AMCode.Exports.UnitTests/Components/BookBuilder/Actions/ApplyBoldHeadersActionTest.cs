using System;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.BookBuilder.Actions;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Documents.Xlsx;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.ActionTests
{
    [TestFixture]
    public class ApplyBoldHeadersActionTest
    {
        private ApplyBoldHeadersAction boldHeaderStyleAction;
        private Mock<IExcelBook> excelBookMoq;
        private string header;
        private Mock<IColumnStyleActionData> styleDataMoq;

        [SetUp]
        public void SetUp()
        {
            boldHeaderStyleAction = new ApplyBoldHeadersAction();
            excelBookMoq = new();

            styleDataMoq = new();
            styleDataMoq.Setup(moq => moq.ColumnCount).Returns(() => 3);

            header = ExceptionUtil.CreateExceptionHeader<IExcelBook, IStyleActionData>(boldHeaderStyleAction.Style);
        }

        [Test]
        public void ShouldApplyBoldHeaderStyle()
        {
            boldHeaderStyleAction.Style(excelBookMoq.Object, styleDataMoq.Object);

            excelBookMoq.Verify(moq => moq.SetRangeStyleAllSheets(
                It.Is<int>(row => row == 1),
                It.Is<int>(column => column == 1),
                It.Is<int>(lastRow => lastRow == 1),
                It.Is<int>(lastColumn => lastColumn == 3),
                It.Is<IStyleParam>(styleParam => styleParam.Bold == true)), Times.Once());
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenExcelBookIsNull()
            => Assert.Throws<NullReferenceException>(
                () => boldHeaderStyleAction.Style(null, styleDataMoq.Object),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "book"));

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStyleDataIsNull()
            => Assert.Throws<NullReferenceException>(
                () => boldHeaderStyleAction.Style(excelBookMoq.Object, null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "styleData"));
    }
}