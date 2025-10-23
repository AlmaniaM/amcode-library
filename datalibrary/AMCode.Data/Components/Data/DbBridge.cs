using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Data.Exceptions;

namespace AMCode.Data
{
    /// <summary>
    /// A class designed to be the lowest level bridge between code and the database. This will
    /// communicate directly with the database and using <see cref="IDbConnection"/>s and
    /// <see cref="IDbCommand"/>s.
    /// </summary>
    public class DbBridge : IDbBridge
    {
        private IDbConnection connection;

        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbCommandFactory commandFactory;

        /// <summary>
        /// Create an instance of a <see cref="IDbBridge"/> class.
        /// </summary>
        /// <param name="connectionFactory">The <see cref="IDbConnectionFactory"/> that will provide the
        /// <see cref="IDbConnection"/> to use to the database.</param>
        /// <param name="commandFactory">The <see cref="IDbCommandFactory"/> that will provide the
        /// <see cref="IDbCommand"/> to use for executing commands on the database.</param>
        public DbBridge(IDbConnectionFactory connectionFactory, IDbCommandFactory commandFactory)
        {
            this.connectionFactory = connectionFactory;
            this.commandFactory = commandFactory;
        }

        /// <summary>
        /// Connect to the DB.
        /// </summary>
        /// <param name="open">If true the <see cref="IDbConnection"/> will be returned as <see cref="ConnectionState.Open"/>.
        /// If false, then just the <see cref="IDbConnection"/> will be returned.</param>
        /// <returns>The <see cref="IDbConnection"/> that was used to connect to the DB.</returns>
        public IDbConnection Connect(bool open = true)
        {
            connection = connectionFactory.Create();
            openConnection(connection);
            return connection;
        }

        /// <summary>
        /// Disconnect and dispose of local resources.
        /// </summary>
        public void Disconnect()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        public async Task ExecuteAsync(string query, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    cancellationToken.ThrowIfCancellationRequested();
                    command.ExecuteNonQuery();
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A custom <see cref="IDbConnection"/></param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        public async Task ExecuteAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            if (connection.State == ConnectionState.Broken)
            {
                throw new DbConnectionBrokenStateException(
                    $"[{nameof(DbBridge)}][{nameof(ExecuteAsync)}]({nameof(query)}, {nameof(connection)}, {nameof(cancellationToken)})",
                    string.Empty
                );
            }

            await Task.Run(() =>
            {
                using (var command = commandFactory.Create(connection, query))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    command.CommandText = query;

                    cancellationToken.ThrowIfCancellationRequested();

                    command.ExecuteNonQuery();
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Execute a query and get a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        /// <returns>A populated <see cref="IDataReader"/> to consume.</returns>
        public async Task<IDataReader> QueryAsync(string query, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    cancellationToken.ThrowIfCancellationRequested();

                    var dr = command.ExecuteReader();

                    cancellationToken.ThrowIfCancellationRequested();

                    return dr;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Executes the provided string and does not return anything.
        /// </summary>
        /// <param name="query">A string SQL query to be executed in Vertica.</param>
        /// <param name="connection">A custom <see cref="IDbConnection"/></param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        public async Task<IDataReader> QueryAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            if (connection.State == ConnectionState.Broken)
            {
                throw new DbConnectionBrokenStateException(
                    $"[{nameof(DbBridge)}][{nameof(QueryAsync)}]({nameof(query)}, {nameof(connection)}, {nameof(cancellationToken)})",
                    string.Empty
                );
            }

            return await Task.Run(() =>
            {
                using (var command = commandFactory.Create(connection, query))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var dr = command.ExecuteReader();

                    cancellationToken.ThrowIfCancellationRequested();

                    return dr;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Dispose of open connections.
        /// </summary>
        public void Dispose() => Disconnect();

        private void openConnection(IDbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}