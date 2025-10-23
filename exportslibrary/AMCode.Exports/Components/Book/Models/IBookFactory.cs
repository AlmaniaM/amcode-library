namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface for creating <see cref="IBook{TColumn}"/> objects.
    /// </summary>
    public interface IBookFactory<TColumn> where TColumn : IBookDataColumn
    {
        /// <summary>
        /// Create an <see cref="IBook{TColumn}"/> of type <typeparamref name="TColumn"/> object.
        /// </summary>
        /// <returns>An <see cref="IBook{TColumn}"/> of type <typeparamref name="TColumn"/>.</returns>
        IBook<TColumn> CreateBook();
    }
}