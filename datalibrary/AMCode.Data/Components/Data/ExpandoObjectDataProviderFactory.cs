using AMCode.Data.Exceptions;

namespace AMCode.Data
{
    /// <summary>
    /// A factory class designed to create <see cref="IExpandoObjectDataProvider"/>s.
    /// </summary>
    public class ExpandoObjectDataProviderFactory : IExpandoObjectDataProviderFactory
    {
        private readonly IQueryCancellation queryCancellation;
        private readonly IDbBridgeProviderFactory dbBridgeProviderFactory;

        /// <summary>
        /// Create an instance of the <see cref="ExpandoObjectDataProviderFactory"/> class.
        /// </summary>
        public ExpandoObjectDataProviderFactory() { }

        /// <summary>
        /// Create an instance of the <see cref="ExpandoObjectDataProviderFactory"/> class.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> for passing down to the
        /// <see cref="ExpandoObjectDataProvider"/> object.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling query requests.</param>
        public ExpandoObjectDataProviderFactory(IDbBridgeProviderFactory dbBridgeProviderFactory, IQueryCancellation queryCancellation)
        {
            this.queryCancellation = queryCancellation;
            this.dbBridgeProviderFactory = dbBridgeProviderFactory;
        }

        /// <summary>
        /// Create a <see cref="IExpandoObjectDataProvider"/> object.
        /// </summary>
        /// <returns>An <see cref="IExpandoObjectDataProvider"/> object.</returns>
        public IExpandoObjectDataProvider Create()
        {
            if (dbBridgeProviderFactory == null || queryCancellation == null)
            {
                throw new DefaultFactoryMethodParametersMissingException(
                    $"[{nameof(ExpandoObjectDataProviderFactory)}][{nameof(Create)}]()",
                    $"Cannot create an instance of {nameof(ExpandoObjectDataProvider)} when {nameof(IDbBridgeProviderFactory)} or {nameof(IQueryCancellation)} objects are null."
                );
            }

            return new ExpandoObjectDataProvider(dbBridgeProviderFactory, queryCancellation);
        }

        /// <summary>
        /// Create a <see cref="IExpandoObjectDataProvider"/> object.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> object for the <see cref="IExpandoObjectDataProvider"/>
        /// to use for creating new <see cref="IDbBridge"/> objects.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling queries.</param>
        /// <returns>An <see cref="IExpandoObjectDataProvider"/> object.</returns>
        public IExpandoObjectDataProvider Create(IDbBridgeProviderFactory dbBridgeProviderFactory, IQueryCancellation queryCancellation)
        {
            if (dbBridgeProviderFactory == null || queryCancellation == null)
            {
                throw new DefaultFactoryMethodParametersMissingException(
                    $"[{nameof(ExpandoObjectDataProviderFactory)}][{nameof(Create)}]({nameof(dbBridgeProviderFactory)}, {nameof(queryCancellation)})",
                    $"Cannot create an instance of {nameof(ExpandoObjectDataProvider)} when {nameof(IDbBridgeProviderFactory)} or {nameof(IQueryCancellation)} objects are null."
                );
            }

            return new ExpandoObjectDataProvider(dbBridgeProviderFactory, queryCancellation);
        }
    }
}