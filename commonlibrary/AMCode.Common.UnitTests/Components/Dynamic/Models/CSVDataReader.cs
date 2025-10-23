using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AMCode.Common.UnitTests.Dynamic.Models
{
    /// <summary>
    /// A mock CSV data reader for testing purposes.
    /// </summary>
    public class CSVDataReader : IDataReader
    {
        private readonly List<string[]> _rows;
        private readonly string[] _headers;
        private int _currentRowIndex = -1;

        public CSVDataReader(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            _headers = lines[0].Split(',');
            _rows = lines.Skip(1).Select(line => line.Split(',')).ToList();
        }

        public CSVDataReader(string filePath, char delimiter)
        {
            var lines = File.ReadAllLines(filePath);
            _headers = lines[0].Split(delimiter);
            _rows = lines.Skip(1).Select(line => line.Split(delimiter)).ToList();
        }

        public object this[string name] => GetValue(name);
        public object this[int i] => GetValue(i);
        public int Depth => 0;
        public bool IsClosed => false;
        public int RecordsAffected => -1;
        public int FieldCount => _headers.Length;

        public void Close() { }
        public void Dispose() { }

        public bool Read()
        {
            _currentRowIndex++;
            return _currentRowIndex < _rows.Count;
        }

        public bool NextResult() => false;

        public string GetName(int i) => _headers[i];
        public int GetOrdinal(string name) => Array.IndexOf(_headers, name);
        public object GetValue(int i) => _rows[_currentRowIndex][i];
        public object GetValue(string name) => GetValue(GetOrdinal(name));
        public int GetValues(object[] values) => throw new NotImplementedException();
        public bool GetBoolean(int i) => Convert.ToBoolean(GetValue(i));
        public byte GetByte(int i) => Convert.ToByte(GetValue(i));
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public char GetChar(int i) => Convert.ToChar(GetValue(i));
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public IDataReader GetData(int i) => throw new NotImplementedException();
        public string GetDataTypeName(int i) => typeof(string).Name;
        public DateTime GetDateTime(int i) => Convert.ToDateTime(GetValue(i));
        public decimal GetDecimal(int i) => Convert.ToDecimal(GetValue(i));
        public double GetDouble(int i) => Convert.ToDouble(GetValue(i));
        public Type GetFieldType(int i) => typeof(string);
        public float GetFloat(int i) => Convert.ToSingle(GetValue(i));
        public Guid GetGuid(int i) => Guid.Parse(GetValue(i).ToString());
        public short GetInt16(int i) => Convert.ToInt16(GetValue(i));
        public int GetInt32(int i) => Convert.ToInt32(GetValue(i));
        public long GetInt64(int i) => Convert.ToInt64(GetValue(i));
        public string GetString(int i) => GetValue(i).ToString();
        public bool IsDBNull(int i) => GetValue(i) == null || GetValue(i) == DBNull.Value;
        public DataTable GetSchemaTable() => throw new NotImplementedException();
    }
}
