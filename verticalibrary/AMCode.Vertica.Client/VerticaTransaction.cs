using System;
using System.Data;
using System.Data.Common;

namespace Vertica.Data.VerticaClient
{
    /// <summary>
    /// Mock Vertica transaction implementation for local development.
    /// </summary>
    public class VerticaTransaction : DbTransaction
    {
        public override IsolationLevel IsolationLevel { get; }

        protected override DbConnection DbConnection { get; }

        public VerticaTransaction(DbConnection connection, IsolationLevel isolationLevel)
        {
            DbConnection = connection;
            IsolationLevel = isolationLevel;
        }

        public override void Commit()
        {
            // Mock implementation
        }

        public override void Rollback()
        {
            // Mock implementation
        }
    }
}
