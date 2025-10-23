using System;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder.Actions
{
    /// <summary>
    /// A class designed for setting the width for a collection of columns.
    /// </summary>
    public class ApplyColumnWidthAction : IExcelBookStyleAction
    {
        /// <summary>
        /// Apply the column width for a collection of <see cref="ColumnStyleParam"/>s.
        /// </summary>
        /// <exception cref="NullReferenceException">When either the <paramref name="book"/> or <paramref name="styleData"/> is null.</exception>
        /// <inheritdoc/>
        public void Style(IExcelBook book, IStyleActionData styleData)
        {
            ApplyStylesValidator.ValidateColumnStyleParameters(book, styleData, this, out var columnStyleData);

            foreach (var columnStyle in columnStyleData.ColumnStyles)
            {
                if (columnStyle.Width != null)
                {
                    book.SetColumnWidthInPixelsAllSheets(columnStyle.Name, columnStyle.Width.Value);
                }
            }
        }
    }
}