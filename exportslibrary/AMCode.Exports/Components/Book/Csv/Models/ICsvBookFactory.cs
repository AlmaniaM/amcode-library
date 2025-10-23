namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface for creating <see cref="ICsvBook"/> objects.
    /// </summary>
    public interface ICsvBookFactory : IBookFactory<ICsvDataColumn> { }
}