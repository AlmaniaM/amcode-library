namespace AMCode.Data.Odbc
{
    /// <summary>
    /// A factory class designed to create an <see cref="IDbBridge"/> our of ODBC data access objects.
    /// </summary>
    public class OdbcDataReaderProviderFactory : IDbBridgeProviderFactory
    {
        private readonly string connectionString;

        /// <summary>
        /// Create an instance of the <see cref="OdbcDataReaderProviderFactory"/> class.
        /// </summary>
        /// <param name="connectionString"></param>
        public OdbcDataReaderProviderFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Create an instance of an <see cref="IDbBridge"/>.
        /// </summary>
        /// <returns>A <see cref="IDbBridge"/>.</returns>
        public IDbBridge Create() => new DbBridge(new OdbcConnectionFactory(connectionString), new OdbcCommandFactory());
    }
}