using System;
using System.Data;
using System.Data.Common;

namespace Vertica.Data.VerticaClient
{
    /// <summary>
    /// Mock Vertica parameter implementation for local development.
    /// </summary>
    public class VerticaParameter : DbParameter
    {
        public override DbType DbType { get; set; }

        public override ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        public override bool IsNullable { get; set; } = true;

        public override string ParameterName { get; set; } = string.Empty;

        public override byte Precision { get; set; }

        public override byte Scale { get; set; }

        public override int Size { get; set; }

        public override string SourceColumn { get; set; } = string.Empty;

        public override bool SourceColumnNullMapping { get; set; }

        public override object Value { get; set; }

        public override void ResetDbType()
        {
            DbType = DbType.String;
        }
    }
}
