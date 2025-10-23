using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.DataTransform;
using AMCode.Data.Logging;
using AMCode.Data.Logging.Infrastructure;

namespace AMCode.Data
{
    /// <summary>
    /// A class designed to be a proxy between the user and a <see cref="IGenericDataProvider"/> and a <see cref="IExpandoObjectDataProvider"/>.
    /// </summary>
    public class DataProvider : IDataProvider
    {
        private readonly IDbExecuteFactory dbExecuteFactory;
        private readonly IExpandoObjectDataProviderFactory expandoProviderFactory;
        private readonly IGenericDataProviderFactory genericProviderFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Create an instance of the <see cref="DataProvider"/> class.
        /// </summary>
        /// <param name="dbExecuteFactory">A <see cref="IDbExecuteFactory"/> for executing non queries.</param>
        /// <param name="expandoProviderFactory">A <see cref="ExpandoObjectDataProviderFactory"/> for executing queries and returning <see cref="ExpandoObject"/>s.</param>
        /// <param name="genericProviderFactory">A <see cref="IGenericDataProviderFactory"/> for executing queries and mapping the fields to object properties.</param>
        /// <param name="logger">Optional logger for tracking operations.</param>
        public DataProvider(IDbExecuteFactory dbExecuteFactory, IExpandoObjectDataProviderFactory expandoProviderFactory, IGenericDataProviderFactory genericProviderFactory, ILogger<DataProvider> logger = null)
        {
            this.dbExecuteFactory = dbExecuteFactory;
            this.expandoProviderFactory = expandoProviderFactory;
            this.genericProviderFactory = genericProviderFactory;
            this._logger = logger ?? new NoOpLogger();
        }

        /// <summary>
        /// Executes the provided string and does not return anything.
        /// </summary>
        /// <param name="query">A string SQL query to be executed in Vertica.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        public async Task ExecuteAsync(string query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Executing query", new { Query = query });
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                await dbExecuteFactory.Create().ExecuteAsync(query, cancellationToken);
                stopwatch.Stop();
                _logger.LogPerformance("QueryExecution", stopwatch.Elapsed, new { Query = query });
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Query execution failed", new { Query = query });
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public async Task<IList<ExpandoObject>> GetExpandoListAsync(string query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Executing ExpandoObject query", new { Query = query });
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var result = await expandoProviderFactory.Create().GetExpandoListAsync(query, cancellationToken);
                stopwatch.Stop();
                _logger.LogPerformance("ExpandoObjectQuery", stopwatch.Elapsed, new { Query = query, ResultCount = result.Count });
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "ExpandoObject query failed", new { Query = query });
                throw;
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
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken = default) => expandoProviderFactory.Create().GetExpandoListAsync(query, columns, cancellationToken);

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
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns, IDbConnection connection, CancellationToken cancellationToken = default) => expandoProviderFactory.Create().GetExpandoListAsync(query, columns, connection, cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        public Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default) => expandoProviderFactory.Create().GetExpandoListAsync(query, connection, cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public async Task<IList<T>> GetListOfAsync<T>(string query, CancellationToken cancellationToken = default) where T : new()
        {
            _logger.LogInformation("Executing generic query", new { Query = query, Type = typeof(T).Name });
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var result = await genericProviderFactory.Create().GetListOfAsync<T>(query, cancellationToken);
                stopwatch.Stop();
                _logger.LogPerformance("GenericQuery", stopwatch.Elapsed, new { Query = query, Type = typeof(T).Name, ResultCount = result.Count });
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Generic query failed", new { Query = query, Type = typeof(T).Name });
                throw;
            }
        }

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
        public Task<IList<T>> GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken = default) where T : new() => genericProviderFactory.Create().GetListOfAsync<T>(query, columns, cancellationToken);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        public Task<IList<T>> GetListOfAsync<T>(string query, IDbConnection connection, CancellationToken cancellationToken = default) where T : new() => genericProviderFactory.Create().GetListOfAsync<T>(query, connection, cancellationToken);

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
        public Task<IList<T>> GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns, IDbConnection connection, CancellationToken cancellationToken = default) where T : new() => genericProviderFactory.Create().GetListOfAsync<T>(query, columns, connection, cancellationToken);

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
            _logger.LogInformation("Executing value query", new { Query = query, ColumnName = columnName, Type = typeof(T).Name });
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var result = await genericProviderFactory.Create().GetValueOfAsync<T>(columnName, query, cancellationToken);
                stopwatch.Stop();
                _logger.LogPerformance("ValueQuery", stopwatch.Elapsed, new { Query = query, ColumnName = columnName, Type = typeof(T).Name });
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Value query failed", new { Query = query, ColumnName = columnName, Type = typeof(T).Name });
                throw;
            }
        }
    }
}