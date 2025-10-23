namespace AMCode.Data
{
    /// <summary>
    /// A factory class designed for creating <see cref="IDbExecute"/> objects.
    /// </summary>
    public class DbExecuteFactory : IDbExecuteFactory
    {
        private readonly IQueryCancellation queryCancellation;
        private readonly IDbBridgeProviderFactory dbBridgeProviderFactory;

        /// <summary>
        /// Create an instance of a <see cref="DbExecuteFactory"/> class.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> for passing down to the
        /// <see cref="IDbExecute"/> object.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling query requests.</param>
        public DbExecuteFactory(IDbBridgeProviderFactory dbBridgeProviderFactory, IQueryCancellation queryCancellation)
        {
            this.queryCancellation = queryCancellation;
            this.dbBridgeProviderFactory = dbBridgeProviderFactory;
        }

        /// <summary>
        /// Create an instance of a <see cref="DbExecute"/> class.
        /// </summary>
        /// <inheritdoc/>
        public IDbExecute Create() => new DbExecute(dbBridgeProviderFactory, queryCancellation);
    }
}