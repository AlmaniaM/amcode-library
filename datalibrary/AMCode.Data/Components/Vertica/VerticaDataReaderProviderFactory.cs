namespace AMCode.Data.Vertica
{
    /// <summary>
    /// A factory class designed to create an <see cref="IDbBridge"/> our of ODBC data access objects.
    /// </summary>
    public class VerticaDataReaderProviderFactory : IDbBridgeProviderFactory
    {
        private readonly string connectionString;

        /// <summary>
        /// Create an instance of the <see cref="VerticaDataReaderProviderFactory"/> class.
        /// </summary>
        /// <param name="connectionString"></param>
        public VerticaDataReaderProviderFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Create an instance of an <see cref="IDbBridge"/>.
        /// </summary>
        /// <returns>A <see cref="IDbBridge"/>.</returns>
        public IDbBridge Create() => new DbBridge(new VerticaConnectionFactory(connectionString), new VerticaCommandFactory());
    }
}