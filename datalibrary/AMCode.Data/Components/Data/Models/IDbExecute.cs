using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a database execute action.
    /// </summary>
    public interface IDbExecute
    {
        /// <summary>
        /// Executes the provided string and does not return anything.
        /// </summary>
        /// <param name="query">A string SQL query to be executed in Vertica.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        Task ExecuteAsync(string query, CancellationToken cancellationToken = default);
    }
}