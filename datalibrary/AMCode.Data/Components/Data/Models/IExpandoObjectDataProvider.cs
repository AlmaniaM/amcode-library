using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.DataTransform;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents an <see cref="IDataProvider"/> which converts database
    /// queries into collections of <see cref="ExpandoObject"/>s.
    /// </summary>
    public interface IExpandoObjectDataProvider
    {
        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        Task<IList<ExpandoObject>> GetExpandoListAsync(string query, CancellationToken cancellationToken = default);

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
        Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken = default);

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
        Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns, IDbConnection connection, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a SQL string on vertica and maps the result to a list of <seealso cref="ExpandoObject"/>'s.
        /// The properties of the <seealso cref="ExpandoObject"/>'s will resemble the column names of the SQL
        /// statement and will appear in the same order.
        /// </summary>
        /// <param name="query">The query you want to execute.</param>
        /// <param name="connection">A <seealso cref="IDbConnection"/> object.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A list of <seealso cref="ExpandoObject"/>'s.</returns>
        Task<IList<ExpandoObject>> GetExpandoListAsync(string query, IDbConnection connection, CancellationToken cancellationToken = default);
    }
}