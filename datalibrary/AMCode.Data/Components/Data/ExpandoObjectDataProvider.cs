using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.DataTransform;
using AMCode.Data.Exceptions;
using AMCode.Data.Extensions;

namespace AMCode.Data
{
    /// <summary>
    /// A class designed for executing queries and mapping the data into <see cref="ExpandoObject"/>s.
    /// </summary>
    public class ExpandoObjectDataProvider : IExpandoObjectDataProvider
    {
        private readonly IQueryCancellation dataCancellation;
        private readonly IDbBridgeProviderFactory dbBridgeProviderFactory;

        /// <summary>
        /// Create an instance of the <see cref="ExpandoObjectDataProvider"/> class.
        /// </summary>
        /// <param name="dbBridgeProviderFactory"></param>
        /// <param name="dataCancellation"></param>
        public ExpandoObjectDataProvider(IDbBridgeProviderFactory dbBridgeProviderFactory, IQueryCancellation dataCancellation)
        {
            this.dataCancellation = dataCancellation;
            this.dbBridgeProviderFactory = dbBridgeProviderFactory;
        }

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, CancellationToken cancellationToken = default)
            => getExpandoListAsync(query, dr => dr.ToExpandoList(cancellationToken), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="columns">The <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>'s to use
        /// for transforming the data.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken = default)
            => getExpandoListAsync(query, dr => dr.ToExpandoList(columns, cancellationToken), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="getList"></param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        private async Task<IList<ExpandoObject>> getExpandoListAsync(string query, Func<IDataReader, IList<ExpandoObject>> getList, CancellationToken cancellationToken = default)
        {
            using (var db = dbBridgeProviderFactory.Create())
            {
                using (var connection = db.Connect(true))
                {
                    // Get a SQL statement to execute in order to cancel this request if the cancellation token makes that request
                    var cancelSessionQuery = await dataCancellation.GetCancellationRequestAsync(connection);

                    // This will listen for the cancellation request and run the close session statement.
                    cancellationToken.Register(async () => await dataCancellation.ExecuteCancellationRequestAsync(cancelSessionQuery));

                    using (var dr = await db.QueryAsync(query, cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var items = getList(dr);

                        cancellationToken.ThrowIfCancellationRequested();

                        return items;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="columns">The <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>'s to use
        /// for transforming the data.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns, IDbConnection connection, CancellationToken cancellationToken = default)
            => GetExpandoListAsync(query, connection, dr => dr.ToExpandoList(columns, cancellationToken), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="getList">A <see cref="Func{T, TResult}"/> that returns a <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public async Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IDbConnection connection, Func<IDataReader, IList<ExpandoObject>> getList, CancellationToken cancellationToken = default)
        {
            using (var db = dbBridgeProviderFactory.Create())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (connection.State == ConnectionState.Broken)
                {
                    throw new DbConnectionBrokenStateException(
                        $"[{nameof(ExpandoObjectDataProvider)}][{nameof(GetExpandoListAsync)}]({nameof(query)}, {nameof(connection)}, {nameof(getList)}, {nameof(cancellationToken)})",
                        string.Empty
                    );
                }

                // Get a SQL statement to execute in order to cancel this request if the cancellation token makes that request
                var cancelSessionQuery = await dataCancellation.GetCancellationRequestAsync(connection);

                // This will listen for the cancellation request and run the close session statement.
                cancellationToken.Register(async () => await dataCancellation.ExecuteCancellationRequestAsync(cancelSessionQuery));

                using (var dr = await db.QueryAsync(query, connection, cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var items = getList(dr);

                    cancellationToken.ThrowIfCancellationRequested();

                    cancellationToken.ThrowIfCancellationRequested();

                    return items;
                }
            }
        }

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default)
            => GetExpandoListAsync(query, connection, dr => dr.ToExpandoList(cancellationToken), cancellationToken);
    }
}