using System;
using System.Linq;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder.Actions
{
    /// <summary>
    /// A class designed to apply styles for a collection of columns.
    /// </summary>
    public class ApplyColumnStylesAction : IExcelBookStyleAction
    {
        /// <summary>
        /// Apply styles for a collection of <see cref="ColumnStyleParam"/>s.
        /// </summary>
        /// <exception cref="NullReferenceException">When either the <paramref name="book"/> or <paramref name="styleData"/> is null.</exception>
        /// <inheritdoc/>
        public void Style(IExcelBook book, IStyleActionData styleData)
        {
            ApplyStylesValidator.ValidateColumnStyleParameters(book, styleData, this, out var columnStyleData);

            var columnStyles = columnStyleData.ColumnStyles.Select(column => new ColumnStyleParam
            {
                Bold = column.Style?.Bold,
                BorderStyles = column.Style?.BorderStyles,
                Color = column.Style?.Color,
                ColorIndex = column.Style?.ColorIndex,
                ColumnName = column.Name,
                FillPattern = column.Style?.FillPattern,
                FontColor = column.Style?.FontColor,
                FontSize = column.Style?.FontSize,
                HorizontalAlignment = column.Style?.HorizontalAlignment,
                Italic = column.Style?.Italic,
                NumberFormat = column.Style?.NumberFormat,
                PatternColor = column.Style?.PatternColor,
            }).ToList<IColumnStyleParam>();

            book.SetColumnStylesByColumnNameAllSheets(columnStyles);
        }
    }
}