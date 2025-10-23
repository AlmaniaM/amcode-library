using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Data
{
    /// <summary>
    /// A interface that represents a database bridge for executing queries.
    /// </summary>
    public interface IDbBridge : IDisposable
    {
        /// <summary>
        /// Connect to the DB.
        /// </summary>
        /// <param name="open">If true the <see cref="IDbConnection"/> will be returned as <see cref="ConnectionState.Open"/>.
        /// If false, then just the <see cref="IDbConnection"/> will be returned.</param>
        /// <returns>The <see cref="IDbConnection"/> that's was used to connect to the DB.</returns>
        IDbConnection Connect(bool open = true);

        /// <summary>
        /// Disconnect and dispose of local resources.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        Task ExecuteAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A custom <see cref="IDbConnection"/></param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        Task ExecuteAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute a query and get a <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        /// <returns>A populated <see cref="IDataReader"/> to consume.</returns>
        Task<IDataReader> QueryAsync(string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the provided string and does not return anything.
        /// </summary>
        /// <param name="query">A string SQL query to be executed in Vertica.</param>
        /// <param name="connection">A custom <see cref="IDbConnection"/></param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel ongoing requests.</param>
        Task<IDataReader> QueryAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default);
    }
}