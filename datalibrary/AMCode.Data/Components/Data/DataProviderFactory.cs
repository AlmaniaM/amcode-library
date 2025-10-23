using AMCode.Data.Exceptions;
using AMCode.Data.Logging;

namespace AMCode.Data
{
    /// <summary>
    /// A factory class designed to create <see cref="IDataProvider"/> objects.
    /// </summary>
    public class DataProviderFactory : IDataProviderFactory
    {
        private readonly IDbExecuteFactory dbExecuteFactory;
        private readonly IExpandoObjectDataProviderFactory expandoProviderFactory;
        private readonly IGenericDataProviderFactory genericProviderFactory;
        private readonly ILogger<DataProvider> _logger;

        /// <summary>
        /// Create an instance of the <see cref="DataProviderFactory"/> class.
        /// </summary>
        public DataProviderFactory() { }

        /// <summary>
        /// Create an instance of the <see cref="DataProviderFactory"/> class.
        /// </summary>
        /// <param name="dbExecuteFactory">The <see cref="IDbExecuteFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="expandoProviderFactory">The <see cref="IExpandoObjectDataProviderFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="genericProviderFactory">The <see cref="IGenericDataProviderFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="logger">Optional logger for tracking operations.</param>
        public DataProviderFactory(IDbExecuteFactory dbExecuteFactory, IExpandoObjectDataProviderFactory expandoProviderFactory, IGenericDataProviderFactory genericProviderFactory, ILogger<DataProvider> logger = null)
        {
            this.dbExecuteFactory = dbExecuteFactory;
            this.expandoProviderFactory = expandoProviderFactory;
            this.genericProviderFactory = genericProviderFactory;
            this._logger = logger;
        }

        /// <summary>
        /// Create a <see cref="IDataProvider"/> object.
        /// </summary>
        /// <exception cref="DefaultFactoryMethodParametersMissingException"></exception>
        /// <returns>An <see cref="IDataProvider"/> object.</returns>
        public IDataProvider Create()
        {
            if (dbExecuteFactory == null || expandoProviderFactory == null || genericProviderFactory == null)
            {
                throw new DefaultFactoryMethodParametersMissingException(
                    $"[{nameof(DataProviderFactory)}][{nameof(Create)}]()",
                    $"Cannot create an instance of {nameof(DataProvider)} when {nameof(IDbExecuteFactory)}, {nameof(IExpandoObjectDataProviderFactory)}, or {nameof(IGenericDataProviderFactory)} objects are null."
                );
            }

            return new DataProvider(dbExecuteFactory, expandoProviderFactory, genericProviderFactory, _logger);
        }

        /// <summary>
        /// Create an <see cref="IDataProvider"/> object.
        /// </summary>
        /// <param name="dbExecuteFactory">The <see cref="IDbExecuteFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="expandoProviderFactory">The <see cref="IExpandoObjectDataProviderFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <param name="genericProviderFactory">The <see cref="IGenericDataProviderFactory"/> to pass to the <see cref="IDataProvider"/> object.</param>
        /// <exception cref="DefaultFactoryMethodParametersMissingException"></exception>
        /// <returns>An <see cref="IDataProvider"/> object.</returns>
        public IDataProvider Create(IDbExecuteFactory dbExecuteFactory, IExpandoObjectDataProviderFactory expandoProviderFactory, IGenericDataProviderFactory genericProviderFactory)
        {
            if (dbExecuteFactory == null || expandoProviderFactory == null || genericProviderFactory == null)
            {
                throw new DefaultFactoryMethodParametersMissingException(
                    $"[{nameof(DataProviderFactory)}][{nameof(Create)}]({nameof(dbExecuteFactory)}, {nameof(expandoProviderFactory)}, {nameof(genericProviderFactory)})",
                    $"Cannot create an instance of {nameof(DataProvider)} when {nameof(IDbExecuteFactory)}, {nameof(IExpandoObjectDataProviderFactory)}, or {nameof(IGenericDataProviderFactory)} objects are null."
                );
            }

            return new DataProvider(dbExecuteFactory, expandoProviderFactory, genericProviderFactory, _logger);
        }
    }
}