using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Data.Extensions;

namespace AMCode.Data.Vertica
{
    /// <summary>
    /// A class designed to kill ongoing queries on a Vertica database.
    /// </summary>
    public class VerticaQueryCancellation : IQueryCancellation
    {
        private readonly IDbBridgeProviderFactory dbFactory;

        /// <summary>
        /// Creates a new instance of a <see cref="VerticaQueryCancellation"/>.
        /// </summary>
        /// <param name="dbFactory">Provide a <see cref="IDbBridgeProviderFactory"/> for access to
        /// a Vertica database.</param>
        public VerticaQueryCancellation(IDbBridgeProviderFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        /// <summary>
        /// Get a SQL query that will cancel the current session. Used for canceling sessions when a <seealso cref="CancellationToken"/> 
        /// is called.
        /// </summary>
        /// <param name="connection">A <see cref="IDbConnection"/> to get the session ID from.</param>
        /// <returns>A <see cref="string"/> query to run if you want to kill the provided back-end connection.</returns>
        public async Task<string> GetCancellationRequestAsync(IDbConnection connection)
        {
            var sessionIdsQuery = "SELECT session_id, statement_id from current_session";
            IList<ExpandoObject> sessionInfo = new List<ExpandoObject>();
            using (var db = dbFactory.Create())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (connection.State == ConnectionState.Broken)
                {
                    throw new Exception("[VerticaQueryCancellation][GetCancellationRequest](connection) The provided connection state is broken.");
                }

                using (var dr = await db.QueryAsync(sessionIdsQuery, connection))
                {
                    sessionInfo = dr.ToExpandoList();
                }
            }

            sessionInfo[0].TryGetValue("session_id", out string sessionId);
            var cancelSessionQuery = sessionId == null ? "" : $"SELECT CLOSE_SESSION('{sessionId}');";
            return cancelSessionQuery;
        }

        /// <summary>
        /// Execute a query in the back-end.
        /// </summary>
        /// <param name="request">The <see cref="string"/> query to execute.</param>
        public async Task ExecuteCancellationRequestAsync(string request)
        {
            using (var db = dbFactory.Create())
            {
                db.Connect(true);
                await db.ExecuteAsync(request);
            }
        }
    }
}