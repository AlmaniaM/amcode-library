using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.DataTransform;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Data.Exceptions;
using AMCode.Data.Extensions;

namespace AMCode.Data
{
    /// <summary>
    /// A class designed for executing queries and mapping the data into type object properties.
    /// </summary>
    public class GenericDataProvider : IGenericDataProvider
    {
        private readonly IQueryCancellation queryCancellation;
        private readonly IExpandoObjectDataProviderFactory expandoObjectDataProviderFactory;
        private readonly IDbBridgeProviderFactory dbBridgeProviderFactory;

        /// <summary>
        /// Create an instance of the <see cref="GenericDataProvider"/> class.
        /// </summary>
        /// <param name="dbBridgeProviderFactory">A <see cref="IDbBridgeProviderFactory"/> for creating a <see cref="IDbBridge"/> in order to execute queries.</param>
        /// <param name="expandoObjectDataProviderFactory">A <see cref="IExpandoObjectDataProviderFactory"/> for creating <see cref="IExpandoObjectDataProvider"/>s
        /// for creating <see cref="ExpandoObject"/>s from queried data.</param>
        /// <param name="queryCancellation">A <see cref="IQueryCancellation"/> object for canceling ongoing queries.</param>
        public GenericDataProvider(IDbBridgeProviderFactory dbBridgeProviderFactory, IExpandoObjectDataProviderFactory expandoObjectDataProviderFactory, IQueryCancellation queryCancellation)
        {
            this.queryCancellation = queryCancellation;
            this.expandoObjectDataProviderFactory = expandoObjectDataProviderFactory;
            this.dbBridgeProviderFactory = dbBridgeProviderFactory;
        }

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public Task<IList<T>> GetListOfAsync<T>(string query, CancellationToken cancellationToken = default)
            where T : new() => getListOfAsync(query, dr => dr.ToList<T>(cancellationToken), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="columns">The <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>'s to use
        /// for transforming the data.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public Task<IList<T>> GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken = default)
            where T : new() => getListOfAsync(query, dr => dr.ToList<T>(columns), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public Task<IList<T>> GetListOfAsync<T>(string query, IDbConnection connection, CancellationToken cancellationToken = default)
            where T : new() => GetListOfAsync(query, connection, dr => dr.ToList<T>(cancellationToken), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="columns">The <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>'s to use
        /// for transforming the data.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public Task<IList<T>> GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns, IDbConnection connection, CancellationToken cancellationToken = default)
            where T : new() => GetListOfAsync(query, connection, dr => dr.ToList<T>(columns, cancellationToken), cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="getList">A <see cref="Func{T, TResult}"/> that returns a <see cref="IList{T}"/> of <typeparamref name="T"/> objects.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        private async Task<IList<T>> getListOfAsync<T>(string query, Func<IDataReader, IList<T>> getList, CancellationToken cancellationToken = default) where T : new()
        {
            using (var db = dbBridgeProviderFactory.Create())
            {
                using (var connection = db.Connect(true))
                {
                    // Get a SQL statement to execute in order to cancel this request if the cancellation token makes that request
                    var cancelSessionQuery = await queryCancellation.GetCancellationRequestAsync(connection);

                    // This will listen for the cancellation request and run the close session statement.
                    cancellationToken.Register(async () => await queryCancellation.ExecuteCancellationRequestAsync(cancelSessionQuery));

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
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="getList">A <see cref="Func{T, TResult}"/> that returns a <see cref="IList{T}"/> of <typeparamref name="T"/> objects.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public async Task<IList<T>> GetListOfAsync<T>(string query, IDbConnection connection, Func<IDataReader, IList<T>> getList, CancellationToken cancellationToken = default) where T : new()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            if (connection.State == ConnectionState.Broken)
            {
                throw new DbConnectionBrokenStateException(
                    $"[{nameof(GenericDataProvider)}][{nameof(GetListOfAsync)}]({nameof(query)}, {nameof(connection)}, {nameof(getList)}, {nameof(cancellationToken)})",
                    string.Empty
                );
            }

            // Get a SQL statement to execute in order to cancel this request if the cancellation token makes that request
            var cancelSessionQuery = await queryCancellation.GetCancellationRequestAsync(connection);

            // This will listen for the cancellation request and run the close session statement.
            cancellationToken.Register(async () => await queryCancellation.ExecuteCancellationRequestAsync(cancelSessionQuery));

            // The using will take care of disconnecting the db
            using (var db = dbBridgeProviderFactory.Create())
            {
                using (var dr = await db.QueryAsync(query, connection, cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var items = getList(dr);

                    cancellationToken.ThrowIfCancellationRequested();

                    return items;
                }
            }
        }

        /// <summary>
        /// Used for selecting a single column value from the first row of the table. 
        /// </summary>
        /// <typeparam name="T">The type of object you want the result converted to.</typeparam>
        /// <param name="columnName">The name of the column to fetch the value from.</param>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>The provided generic <see cref="Type"/> <typeparamref name="T"/></returns>
        public async Task<T> GetValueOfAsync<T>(string columnName, string query, CancellationToken cancellationToken = default)
        {
            var expandoDataProvider = expandoObjectDataProviderFactory.Create(dbBridgeProviderFactory, queryCancellation);

            cancellationToken.ThrowIfCancellationRequested();

            var result = await expandoDataProvider.GetExpandoListAsync(query, cancellationToken);

            if (result.Count > 0)
            {
                result.First().TryGetValue(columnName, out T resultValue);
                return resultValue;
            }

            return default;
        }
    }
}