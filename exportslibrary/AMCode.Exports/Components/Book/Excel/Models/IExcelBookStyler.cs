using AMCode.Exports.BookBuilder;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to style an <see cref="IExcelBook"/> object.
    /// </summary>
    public interface IExcelBookStyler
    {
        /// <summary>
        /// Apply styles to a book.
        /// </summary>
        /// <param name="excelBook">The <see cref="IExcelBook"/> to apply styles to.</param>
        /// <param name="styles">The styles object to apply.</param>
        void ApplyStyles(IExcelBook excelBook, IColumnStyleActionData styles);
    }
}