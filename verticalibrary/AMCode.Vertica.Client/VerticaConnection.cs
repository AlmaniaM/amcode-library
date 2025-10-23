using System;
using System.Data;
using System.Data.Common;

namespace Vertica.Data.VerticaClient
{
    /// <summary>
    /// Mock Vertica connection implementation for local development.
    /// </summary>
    public class VerticaConnection : DbConnection
    {
        private string _connectionString;
        private ConnectionState _state = ConnectionState.Closed;

        public VerticaConnection()
        {
        }

        public VerticaConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        public override string Database => "MockDatabase";

        public override string DataSource => "MockDataSource";

        public override string ServerVersion => "1.0.0";

        public override ConnectionState State => _state;

        public override void ChangeDatabase(string databaseName)
        {
            // Mock implementation
        }

        public override void Close()
        {
            _state = ConnectionState.Closed;
        }

        public override void Open()
        {
            _state = ConnectionState.Open;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new VerticaTransaction(this, isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new VerticaCommand();
        }
    }
}
