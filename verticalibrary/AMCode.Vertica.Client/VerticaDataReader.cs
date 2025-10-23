using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Vertica.Data.VerticaClient
{
    /// <summary>
    /// Mock Vertica data reader implementation for local development.
    /// </summary>
    public class VerticaDataReader : DbDataReader
    {
        public override int Depth => 0;

        public override int FieldCount => 0;

        public override bool HasRows => false;

        public override bool IsClosed => true;

        public override int RecordsAffected => 0;

        public override object this[int ordinal] => null;

        public override object this[string name] => null;

        public override bool GetBoolean(int ordinal)
        {
            return false;
        }

        public override byte GetByte(int ordinal)
        {
            return 0;
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return 0;
        }

        public override char GetChar(int ordinal)
        {
            return '\0';
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return 0;
        }

        public override string GetDataTypeName(int ordinal)
        {
            return "String";
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return DateTime.MinValue;
        }

        public override decimal GetDecimal(int ordinal)
        {
            return 0;
        }

        public override double GetDouble(int ordinal)
        {
            return 0;
        }

        public override Type GetFieldType(int ordinal)
        {
            return typeof(string);
        }

        public override float GetFloat(int ordinal)
        {
            return 0;
        }

        public override Guid GetGuid(int ordinal)
        {
            return Guid.Empty;
        }

        public override short GetInt16(int ordinal)
        {
            return 0;
        }

        public override int GetInt32(int ordinal)
        {
            return 0;
        }

        public override long GetInt64(int ordinal)
        {
            return 0;
        }

        public override string GetName(int ordinal)
        {
            return $"Column{ordinal}";
        }

        public override int GetOrdinal(string name)
        {
            return 0;
        }

        public override string GetString(int ordinal)
        {
            return string.Empty;
        }

        public override object GetValue(int ordinal)
        {
            return null;
        }

        public override int GetValues(object[] values)
        {
            return 0;
        }

        public override bool IsDBNull(int ordinal)
        {
            return true;
        }

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            return false;
        }

        public override IEnumerator GetEnumerator()
        {
            return new List<object>().GetEnumerator();
        }
    }
}
