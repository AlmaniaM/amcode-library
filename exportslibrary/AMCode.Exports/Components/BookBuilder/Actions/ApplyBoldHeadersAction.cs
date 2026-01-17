using System;
using AMCode.Exports.Book;
using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Exports.BookBuilder.Actions
{
    /// <summary>
    /// A class designed for setting the headers bold for an <see cref="IExcelBook"/>.
    /// </summary>
    public class ApplyBoldHeadersAction : IExcelBookStyleAction
    {
        /// <summary>
        /// Apply the header as bold.
        /// </summary>
        /// <exception cref="NullReferenceException">When either the <paramref name="book"/> or <paramref name="styleData"/> is null.</exception>
        /// <inheritdoc/>
        public void Style(IExcelBook book, IStyleActionData styleData)
        {
            ApplyStylesValidator.ValidateColumnStyleParameters(book, styleData, this, out var columnStyleData);

            book.SetRangeStyleAllSheets(1, 1, 1, columnStyleData.ColumnCount, new StyleParam { Bold = true });
        }
    }
}