using System;
using AMCode.Common.Extensions.Objects;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions.Util;

namespace AMCode.Exports.BookBuilder.Actions
{
    /// <summary>
    /// A class designed for setting the headers bold for an <see cref="IExcelBook"/>.
    /// </summary>
    internal class ApplyStylesValidator
    {
        /// <summary>
        /// Validate parameters for the <see cref="IExcelBookStyleAction.Style(IExcelBook, IStyleActionData)"/> method.
        /// </summary>
        /// <param name="book">The <see cref="IExcelBook"/> object parameter.</param>
        /// <param name="styleData">The <see cref="IStyleActionData"/> object parameter.</param>
        /// <param name="action">The <see cref="IExcelBookStyleAction"/> object you are checking.</param>
        /// <param name="columnStyleData">An <c>out</c> parameter that will be assigned as a <see cref="IColumnStyleActionData"/> if it can
        /// be cast to it.</param>
        /// <exception cref="NullReferenceException">When either the <paramref name="book"/> or <paramref name="styleData"/> is null.</exception>
        public static void ValidateColumnStyleParameters(IExcelBook book, IStyleActionData styleData, IExcelBookStyleAction action, out IColumnStyleActionData columnStyleData)
        {
            IColumnStyleActionData columnData = null;

            if (book is null || styleData is null || !styleData.Is(out columnData))
            {
                var header = ExceptionUtil.CreateExceptionHeader<IExcelBook, IStyleActionData>(action.Style);

                if (book is null)
                {
                    throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(book)));
                }

                if (styleData is null)
                {
                    throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(styleData)));
                }
            }

            columnStyleData = columnData;
        }
    }
}