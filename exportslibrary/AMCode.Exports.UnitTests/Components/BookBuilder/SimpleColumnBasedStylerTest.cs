using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Xlsx;
using AMCode.Xlsx.Drawing;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.SimpleColumnBasedStylerTests
{
    [TestFixture]
    public class SimpleColumnBasedStylerTest
    {
        private readonly int columnCount = 3;
        private IColumnStyleActionData columnStyleData;
        private Mock<IExcelBook> excelBookMoq;
        private IList<Mock<IExcelBookStyleAction>> styleActionMoqs;
        private SimpleColumnBasedStyler styler;
        private string header;

        [SetUp]
        public void SetUp()
        {
            excelBookMoq = new();

            styleActionMoqs = new List<Mock<IExcelBookStyleAction>> { new(), new(), new() };

            styler = new SimpleColumnBasedStyler(styleActionMoqs.Select(moq => moq.Object).ToList());

            columnStyleData = new ColumnStyleActionData
            {
                ColumnStyles = Enumerable.Range(1, columnCount).Select(index => new ColumnStyle
                {
                    Name = $"Column{index}",
                    Style = new StyleParam
                    {
                        Bold = false,
                        Color = Color.Green,
                        Italic = true,
                    },
                    Width = index * 10
                }).ToList<IColumnStyle>()
            };

            header = ExceptionUtil.CreateExceptionHeader<IExcelBook, IColumnStyleActionData>(styler.ApplyStyles);
        }

        [Test]
        public void ShouldRunnStylesActions()
        {
            styler.ApplyStyles(excelBookMoq.Object, columnStyleData);

            styleActionMoqs.ForEach((actionMoq, index) => actionMoq.Verify(
                    moq => moq.Style(
                        It.Is<IExcelBook>(book => book == excelBookMoq.Object),
                        It.Is<IColumnStyleActionData>(style => style == columnStyleData)),
                    Times.Once()));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenExcelBookIsNull()
            => Assert.Throws<NullReferenceException>(
                () => styler.ApplyStyles(null, columnStyleData),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "excelBook"));

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylesAreNull()
            => Assert.Throws<NullReferenceException>(
                () => styler.ApplyStyles(excelBookMoq.Object, null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "columnStyles"));
    }
}