namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a factory for creating the <see cref="IDbExecute"/> object.
    /// </summary>
    public interface IDbBridgeProviderFactory
    {
        /// <summary>
        /// Create an instance of an <see cref="IDbBridge"/>.
        /// </summary>
        /// <returns>A <see cref="IDbBridge"/>.</returns>
        IDbBridge Create();
    }
}