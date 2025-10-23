namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    ///  A factory interface designed for creating <see cref="IBookBuilder"/> objects.
    /// </summary>
    public interface IBookBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="IBookBuilder"/> object.
        /// </summary>
        /// <param name="fileType">The type of <see cref="FileType"/> you want an <see cref="IBookBuilder"/> built for.</param>
        /// <returns>An <see cref="IBookBuilder"/> object.</returns>
        IBookBuilder CreateBuilder(FileType fileType);
    }
}