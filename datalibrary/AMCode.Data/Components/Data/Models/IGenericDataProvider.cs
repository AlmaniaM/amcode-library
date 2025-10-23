using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.DataTransform;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a data reader that converts database queries into
    /// collections of statically typed objects.
    /// </summary>
    public interface IGenericDataProvider
    {
        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        Task<IList<T>> GetListOfAsync<T>(string query, CancellationToken cancellationToken = default) where T : new();

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
        Task<IList<T>> GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Executes a SQL string and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        Task<IList<T>> GetListOfAsync<T>(string query, IDbConnection connection, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Executes a SQL string and maps the result to a list of <typeparamref name="T"/>'s.
        /// The properties of the <typeparamref name="T"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="columns">The <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>'s to use
        /// for transforming the data.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <typeparamref name="T"/>'s.</returns>
        Task<IList<T>> GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns, IDbConnection connection, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Used for selecting a single column value from the first row of the table. 
        /// </summary>
        /// <typeparam name="T">The type of object you want the result converted to.</typeparam>
        /// <param name="columnName">The name of the column to fetch the value from.</param>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The provided generic <typeparamref name="T"/> object.</returns>
        Task<T> GetValueOfAsync<T>(string columnName, string query, CancellationToken cancellationToken = default);
    }
}