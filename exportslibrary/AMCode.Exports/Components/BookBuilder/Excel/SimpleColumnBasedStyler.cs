using System;
using System.Collections.Generic;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions.Util;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A class designed to apply simple column based styles
    /// </summary>
    public class SimpleColumnBasedStyler : IExcelBookStyler
    {
        private readonly IList<IExcelBookStyleAction> bookStyleActions;

        /// <summary>
        /// Create an instance of the <see cref="SimpleColumnBasedStyler"/> class.
        /// </summary>
        /// <param name="bookStyleActions">An <see cref="IList{T}"/> collection of <see cref="IExcelBookStyleAction"/>s.</param>
        public SimpleColumnBasedStyler(IList<IExcelBookStyleAction> bookStyleActions)
        {
            this.bookStyleActions = bookStyleActions;
        }

        /// <summary>
        /// Apply styles to a book.
        /// </summary>
        /// <param name="excelBook">The <see cref="IExcelBook"/> to apply styles to.</param>
        /// <param name="styles">The styles object to apply.</param>
        public void ApplyStyles(IExcelBook excelBook, IColumnStyleActionData styles)
        {
            if (excelBook is null || styles is null)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IExcelBook, IColumnStyleActionData>(ApplyStyles);
                var parameterName = excelBook is null ? nameof(excelBook) : nameof(styles);
                throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, parameterName));
            }

            foreach (var styleAction in bookStyleActions)
            {
                styleAction.Style(excelBook, styles);
            }
        }
    }
}