using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An interface designed provide small styling actions.
    /// </summary>
    public interface IExcelBookStyleAction
    {
        /// <summary>
        /// Style an <see cref="IExcelBook"/>.
        /// </summary>
        /// <param name="book">The <see cref="IExcelBook"/> to apply styles to.</param>
        /// <param name="styleData">The style data.</param>
        void Style(IExcelBook book, IStyleActionData styleData);
    }
}