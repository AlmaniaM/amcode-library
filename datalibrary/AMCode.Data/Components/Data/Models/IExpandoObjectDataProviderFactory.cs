namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a factory for creating the <see cref="IExpandoObjectDataProvider"/> object.
    /// </summary>
    public interface IExpandoObjectDataProviderFactory
    {
        /// <summary>
        /// Create a <see cref="IExpandoObjectDataProvider"/> object.
        /// </summary>
        /// <returns>An <see cref="IExpandoObjectDataProvider"/> object.</returns>
        IExpandoObjectDataProvider Create();

        /// <summary>
        /// Create a <see cref="IExpandoObjectDataProvider"/> object.
        /// </summary>
        /// <param name="factory">A <see cref="IDbBridgeProviderFactory"/> object for the <see cref="IExpandoObjectDataProvider"/>
        /// to use for creating new <see cref="IDbBridge"/> objects.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling queries.</param>
        /// <returns>An <see cref="IExpandoObjectDataProvider"/> object.</returns>
        IExpandoObjectDataProvider Create(IDbBridgeProviderFactory factory, IQueryCancellation queryCancellation);
    }
}