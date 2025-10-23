using System;
using System.Data;
using System.Data.Common;

namespace Vertica.Data.VerticaClient
{
    /// <summary>
    /// Mock Vertica command implementation for local development.
    /// </summary>
    public class VerticaCommand : DbCommand
    {
        private string _commandText;
        private VerticaConnection _connection;
        private int _commandTimeout = 30;

        public VerticaCommand()
        {
        }

        public VerticaCommand(string commandText, VerticaConnection connection)
        {
            _commandText = commandText;
            _connection = connection;
        }

        public override string CommandText
        {
            get => _commandText;
            set => _commandText = value;
        }

        public override int CommandTimeout
        {
            get => _commandTimeout;
            set => _commandTimeout = value;
        }

        public override CommandType CommandType { get; set; } = CommandType.Text;

        public override bool DesignTimeVisible { get; set; } = false;

        public override UpdateRowSource UpdatedRowSource { get; set; } = UpdateRowSource.None;

        protected override DbConnection DbConnection
        {
            get => _connection;
            set => _connection = (VerticaConnection)value;
        }

        protected override DbParameterCollection DbParameterCollection => new VerticaParameterCollection();

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
            // Mock implementation
        }

        public override int ExecuteNonQuery()
        {
            return 0; // Mock implementation
        }

        public override object ExecuteScalar()
        {
            return null; // Mock implementation
        }

        public override void Prepare()
        {
            // Mock implementation
        }

        protected override DbParameter CreateDbParameter()
        {
            return new VerticaParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new VerticaDataReader();
        }
    }
}
