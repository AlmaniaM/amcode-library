namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a factory which creates the <see cref="IDataProvider"/> object.
    /// </summary>
    public interface IDataProviderFactory
    {
        /// <summary>
        /// Create a <see cref="IDataProvider"/> object.
        /// </summary>
        /// <returns>An <see cref="IDataProvider"/> object.</returns>
        IDataProvider Create();

        /// <summary>
        /// Create an <see cref="IDataProvider"/> object.
        /// </summary>
        /// <param name="dbExecuteFactory">The <see cref="IDbExecuteFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="expandoProviderFactory">The <see cref="IExpandoObjectDataProviderFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="genericProviderFactory">The <see cref="IGenericDataProviderFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <returns>An <see cref="IDataProvider"/> object.</returns>
        IDataProvider Create(IDbExecuteFactory dbExecuteFactory, IExpandoObjectDataProviderFactory expandoProviderFactory, IGenericDataProviderFactory genericProviderFactory);
    }
}