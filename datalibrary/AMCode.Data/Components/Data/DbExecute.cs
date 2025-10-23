using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Data
{
    /// <summary>
    /// A class designed only for executing non queries.
    /// </summary>
    public class DbExecute : IDbExecute
    {
        private readonly IQueryCancellation dataCancellation;
        private readonly IDbBridgeProviderFactory dbBridgeProviderFactory;

        /// <summary>
        /// Create an instance of a <see cref="IDbExecute"/> class.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> for providing
        /// a <see cref="IDbBridge"/> for executing non queries.</param>
        /// <param name="dataCancellation">A <see cref="IQueryCancellation"/> object for canceling ongoing queries.</param>
        public DbExecute(IDbBridgeProviderFactory dbBridgeProviderFactory, IQueryCancellation dataCancellation)
        {
            this.dataCancellation = dataCancellation;
            this.dbBridgeProviderFactory = dbBridgeProviderFactory;
        }

        /// <summary>
        /// Executes the provided string and does not return anything.
        /// </summary>
        /// <param name="query">A string SQL query to be executed in Vertica.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        public async Task ExecuteAsync(string query, CancellationToken cancellationToken = default)
        {
            using (var db = dbBridgeProviderFactory.Create())
            {
                var connection = db.Connect(true);

                // Get a SQL statement to execute in order to cancel this request if the cancellation token makes that request
                var cancelSessionQuery = await dataCancellation.GetCancellationRequestAsync(connection);

                // This will listen for the cancellation request and run the close session statement.
                cancellationToken.Register(async () => await dataCancellation.ExecuteCancellationRequestAsync(cancelSessionQuery));

                await db.ExecuteAsync(query);
            }
        }
    }
}