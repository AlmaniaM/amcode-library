namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a factory for creating the <see cref="IGenericDataProvider"/> object.
    /// </summary>
    public interface IGenericDataProviderFactory
    {
        /// <summary>
        /// Create a <see cref="IGenericDataProvider"/> object.
        /// </summary>
        /// <returns>An <see cref="IGenericDataProvider"/> object.</returns>
        IGenericDataProvider Create();

        /// <summary>
        /// Create a <see cref="IGenericDataProvider"/> object.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> object for the <see cref="IGenericDataProvider"/>
        /// to use for creating new <see cref="IDbBridge"/> objects.</param>
        /// <param name="expandoObjectDataProviderFactory">A <see cref="IExpandoObjectDataProviderFactory"/> object for the <see cref="IGenericDataProvider"/>
        /// to use for creating new <see cref="IExpandoObjectDataProvider"/> objects.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling queries.</param>
        /// <returns>An <see cref="IGenericDataProvider"/> object.</returns>
        IGenericDataProvider Create(IDbBridgeProviderFactory dbBridgeProviderFactory, IExpandoObjectDataProviderFactory expandoObjectDataProviderFactory, IQueryCancellation queryCancellation);
    }
}