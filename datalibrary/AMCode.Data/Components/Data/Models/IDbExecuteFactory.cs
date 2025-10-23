namespace AMCode.Data
{
    /// <summary>
    /// An interface designed for creating instances of an <see cref="IDbExecute"/> object.
    /// </summary>
    public interface IDbExecuteFactory
    {
        /// <summary>
        /// Create an <see cref="IDbExecute"/> object.
        /// </summary>
        /// <returns>An <see cref="IDbExecute"/> object.</returns>
        IDbExecute Create();
    }
}