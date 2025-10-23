using AMCode.Data.Exceptions;

namespace AMCode.Data
{
    /// <summary>
    /// A factory class for creating <see cref="IGenericDataProvider"/>s.
    /// </summary>
    public class GenericDataProviderFactory : IGenericDataProviderFactory
    {
        private readonly IQueryCancellation queryCancellation;
        private readonly IDbBridgeProviderFactory dbBridgeProviderFactory;
        private readonly IExpandoObjectDataProviderFactory expandoObjectDataProviderFactory;

        /// <summary>
        /// Create an instance of he <see cref="GenericDataProviderFactory"/> class.
        /// </summary>
        public GenericDataProviderFactory() { }

        /// <summary>
        /// Create an instance of he <see cref="GenericDataProviderFactory"/> class.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> for passing down to the
        /// <see cref="IGenericDataProvider"/> object.</param>
        /// <param name="expandoObjectDataProviderFactory">A <see cref="IExpandoObjectDataProviderFactory"/> for passing down to the
        /// <see cref="IGenericDataProvider"/> object.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling ongoing queries.</param>
        public GenericDataProviderFactory(IDbBridgeProviderFactory dbBridgeProviderFactory, IExpandoObjectDataProviderFactory expandoObjectDataProviderFactory, IQueryCancellation queryCancellation)
        {
            this.queryCancellation = queryCancellation;
            this.dbBridgeProviderFactory = dbBridgeProviderFactory;
            this.expandoObjectDataProviderFactory = expandoObjectDataProviderFactory;
        }

        /// <summary>
        /// Create a <see cref="IGenericDataProvider"/> object.
        /// </summary>
        /// <exception cref="DefaultFactoryMethodParametersMissingException"></exception>
        /// <returns>An <see cref="IGenericDataProvider"/> object.</returns>
        public IGenericDataProvider Create()
        {
            if (dbBridgeProviderFactory == null || expandoObjectDataProviderFactory == null || queryCancellation == null)
            {
                throw new DefaultFactoryMethodParametersMissingException(
                    $"[{nameof(GenericDataProviderFactory)}][{nameof(Create)}]()",
                    $"Cannot create an instance of {nameof(GenericDataProvider)} when {nameof(IDbBridgeProviderFactory)}, {nameof(IExpandoObjectDataProviderFactory)}, or {nameof(IQueryCancellation)} objects are null."
                );
            }

            return new GenericDataProvider(dbBridgeProviderFactory, expandoObjectDataProviderFactory, queryCancellation);
        }

        /// <summary>
        /// Create a <see cref="IGenericDataProvider"/> object.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> object for the <see cref="IGenericDataProvider"/>
        /// to use for creating new <see cref="IDbBridge"/> objects.</param>
        /// <param name="expandoObjectDataProviderFactory">A <see cref="IExpandoObjectDataProviderFactory"/> object for the <see cref="IGenericDataProvider"/>
        /// to use for creating new <see cref="IExpandoObjectDataProvider"/> objects.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling queries.</param>
        /// <exception cref="DefaultFactoryMethodParametersMissingException"></exception>
        /// <returns>An <see cref="IGenericDataProvider"/> object.</returns>
        public IGenericDataProvider Create(IDbBridgeProviderFactory dbBridgeProviderFactory, IExpandoObjectDataProviderFactory expandoObjectDataProviderFactory, IQueryCancellation queryCancellation)
        {
            if (dbBridgeProviderFactory == null || expandoObjectDataProviderFactory == null || queryCancellation == null)
            {
                throw new DefaultFactoryMethodParametersMissingException(
                    $"[{nameof(GenericDataProviderFactory)}][{nameof(Create)}]({nameof(dbBridgeProviderFactory)}, {nameof(expandoObjectDataProviderFactory)}, {nameof(queryCancellation)})",
                    $"Cannot create an instance of {nameof(GenericDataProvider)} when {nameof(IDbBridgeProviderFactory)}, {nameof(IExpandoObjectDataProviderFactory)}, or {nameof(IQueryCancellation)} objects are null."
                );
            }

            return new GenericDataProvider(dbBridgeProviderFactory, expandoObjectDataProviderFactory, queryCancellation);
        }
    }
}