using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Util;
using AMCode.Common.Xlsx;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.BookBuilder.Actions;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Xlsx;
using AMCode.Xlsx.Common;
using AMCode.Xlsx.Drawing;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.ActionTests
{
    [TestFixture]
    public class ApplyColumnStylesActionTest
    {
        private ApplyColumnStylesAction columnStylesAction;
        private Mock<IExcelBook> excelBookMoq;
        private string header;
        private Mock<IColumnStyleActionData> styleDataMoq;

        [SetUp]
        public void SetUp()
        {
            columnStylesAction = new ApplyColumnStylesAction();
            excelBookMoq = new();
            styleDataMoq = new();

            header = ExceptionUtil.CreateExceptionHeader<IExcelBook, IStyleActionData>(columnStylesAction.Style);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(100)]
        public void ShouldApplyColumnStyles(int columnCount)
        {
            styleDataMoq.Setup(moq => moq.ColumnStyles).Returns(() => Enumerable.Range(1, columnCount).Select(index => new ColumnStyle
            {
                Name = $"Column{index}",
                Style = new StyleParam
                {
                    Bold = true,
                    Color = Color.Blue,
                    HorizontalAlignment = ExcelHAlign.HAlignCenter,
                }
            }));

            columnStylesAction.Style(excelBookMoq.Object, styleDataMoq.Object);

            for (var i = 1; i <= columnCount; i++)
            {
                excelBookMoq.Verify(
                    moq => moq.SetColumnStylesByColumnNameAllSheets(
                        It.Is<IList<IColumnStyleParam>>(
                            styles => styles.Where(
                                style =>
                                    style.Bold == true
                                    && style.Color == Color.Blue
                                    && style.HorizontalAlignment == ExcelHAlign.HAlignCenter
                                ).Count() == columnCount
                            )
                        ),
                    Times.Once());
            }
        }

        [Test]
        public void ShouldApplyAllColumnStyles()
        {
            var columnCount = 100;

            static BorderStyle greenBorder() => new() { Color = Color.Green, LineStyle = ExcelLineStyle.Thick };

            styleDataMoq.SetupGet(moq => moq.ColumnStyles).Returns(() => Enumerable.Range(1, columnCount).Select(index => new ColumnStyle
            {
                Name = $"Column{index}",
                Style = new StyleParam
                {
                    Bold = true,
                    BorderStyles = new()
                    {
                        [ExcelBordersIndex.EdgeTop] = greenBorder(),
                        [ExcelBordersIndex.EdgeRight] = greenBorder(),
                        [ExcelBordersIndex.EdgeBottom] = greenBorder(),
                        [ExcelBordersIndex.EdgeLeft] = greenBorder(),
                    },
                    Color = Color.Blue,
                    ColorIndex = ExcelKnownColors.Blue,
                    HorizontalAlignment = ExcelHAlign.HAlignCenter,
                    FillPattern = ExcelPattern.Angle,
                    FontColor = Color.Red,
                    FontSize = 24,
                    Italic = true,
                    NumberFormat = "#,###",
                    PatternColor = Color.Gray
                }
            }));

            columnStylesAction.Style(excelBookMoq.Object, styleDataMoq.Object);

            for (var i = 1; i <= columnCount; i++)
            {
                excelBookMoq.Verify(
                    moq => moq.SetColumnStylesByColumnNameAllSheets(
                        It.Is<IList<IColumnStyleParam>>(
                            styles => styles.Where(
                                style =>
                                    style.Bold == true
                                    && style.BorderStyles[ExcelBordersIndex.EdgeTop].Color == Color.Green
                                    && style.BorderStyles[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.Thick
                                    && style.BorderStyles[ExcelBordersIndex.EdgeRight].Color == Color.Green
                                    && style.BorderStyles[ExcelBordersIndex.EdgeRight].LineStyle == ExcelLineStyle.Thick
                                    && style.BorderStyles[ExcelBordersIndex.EdgeBottom].Color == Color.Green
                                    && style.BorderStyles[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.Thick
                                    && style.BorderStyles[ExcelBordersIndex.EdgeLeft].Color == Color.Green
                                    && style.BorderStyles[ExcelBordersIndex.EdgeLeft].LineStyle == ExcelLineStyle.Thick
                                    && style.Color == Color.Blue
                                    && style.ColorIndex == ExcelKnownColors.Blue
                                    && style.HorizontalAlignment == ExcelHAlign.HAlignCenter
                                    && style.FillPattern == ExcelPattern.Angle
                                    && style.FontColor == Color.Red
                                    && style.FontSize == 24
                                    && style.Italic == true
                                    && style.NumberFormat.Equals("#,###")
                                    && style.PatternColor == Color.Gray
                                ).Count() == columnCount
                            )
                        ),
                    Times.Once());
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenExcelBookIsNull()
            => Assert.Throws<NullReferenceException>(
                () => columnStylesAction.Style(null, styleDataMoq.Object),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "book"));

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStyleDataIsNull()
            => Assert.Throws<NullReferenceException>(
                () => columnStylesAction.Style(excelBookMoq.Object, null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, "styleData"));
    }
}