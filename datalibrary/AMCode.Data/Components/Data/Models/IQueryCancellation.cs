using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a query cancellation object.
    /// </summary>
    public interface IQueryCancellation
    {
        /// <summary>
        /// Execute a query in the back-end.
        /// </summary>
        /// <param name="request">The <see cref="string"/> query to execute.</param>
        Task ExecuteCancellationRequestAsync(string request);

        /// <summary>
        /// Get a SQL query that will cancel the current session. Used for canceling sessions when a <seealso cref="CancellationToken"/> 
        /// is called.
        /// </summary>
        /// <param name="connection">A <see cref="IDbConnection"/> to get the session ID from.</param>
        /// <returns>A <see cref="string"/> query to run if you want to kill the provided back-end connection.</returns>
        Task<string> GetCancellationRequestAsync(IDbConnection connection);
    }
}