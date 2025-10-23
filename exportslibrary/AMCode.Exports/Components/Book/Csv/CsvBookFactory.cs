namespace AMCode.Exports.Book
{
    /// <summary>
    /// A factory class designed to build <see cref="IBookFactory{TColumn}"/> of type <see cref="ICsvDataColumn"/> objects.
    /// </summary>
    public class CsvBookFactory : ICsvBookFactory
    {
        /// <summary>
        /// Create an <see cref="IBook{TColumn}"/> of type <see cref="ICsvDataColumn"/>.
        /// </summary>
        /// <returns>An <see cref="IBook{TColumn}"/> of type <see cref="ICsvDataColumn"/>.</returns>
        public IBook<ICsvDataColumn> CreateBook() => new CsvBook();
    }
}