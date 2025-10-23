using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AMCode.Common.Extensions.Strings;

namespace AMCode.Common.Testing.IO
{
    /// <summary>
    /// A class that reads a CSV file and turns it into an <see cref="IDataReader"/> object.
    /// </summary>
    public class CSVDataReader : IDataReader, IDisposable
    {
        private StreamReader _file;
        private readonly char _delimiter;
        /* stores the header and values of csv and also virtual*/
        private string _virtualHeaderString = "", _csvHeaderstring = "",
        _csvlinestring = "", _virtuallineString = "";
        private readonly bool _firstRowHeader = true;
        private readonly bool emptyAsNull = true;

        /// <summary>
        /// Returns an array of header names as string in the order of columns 
        /// from left to right of csv file. If CSV file doesn't have header then a dummy header 
        /// with 'COL_' + 'column position' will be returned. This can be manually renamed calling 
        /// 'RenameCSVHeader'
        /// </summary>
        public string[] Header { get; private set; }

        /*
         * The values of header and values must be in same order. So using this collection.
         * This collection stores header key as header name and its related value as value. 
         * When the value of a specific 
         * header is updated the specific key value will be updated. 
         * For Original Header values from the csv file the values will be null. 
         * This is used as a check and identify this is a csv value or not.
         */
        private readonly System.Collections.Specialized.OrderedDictionary headercollection =
                    new System.Collections.Specialized.OrderedDictionary();

        /// <summary>
        /// Returns an array of strings from the current line of csv file. 
        /// Call Read() method to read the next line/record of csv file. 
        /// </summary>
        public string[] Line { get; private set; }

        private bool _iscolumnlocked = false;

        /// <summary>
        /// Creates an instance of CSV reader
        /// </summary>
        /// <param name="filePath">Path to the csv file.</param>
        /// <param name="delimiter">delimiter character used in csv file.</param>
        /// <param name="firstRowHeader">specify the csv got a header in first row or not. 
        /// Default is true and if argument is false then auto header 'ROW_xx will be used as per 
        /// the order of columns.</param>
        /// <param name="emptyAsNull"></param>
        public CSVDataReader(string filePath, char delimiter = ',', bool firstRowHeader = true, bool emptyAsNull = false)
        {
            this.emptyAsNull = emptyAsNull;
            _file = File.OpenText(filePath);
            _delimiter = delimiter;
            _firstRowHeader = firstRowHeader;
            if (_firstRowHeader == true)
            {
                Read();
                _csvHeaderstring = _csvlinestring;
                Header = ReadRow(_csvHeaderstring);
                foreach (var item in Header) //check for duplicate headers and create a header record.
                {
                    if (headercollection.Contains(item) == true)
                    {
                        throw new Exception("Duplicate found in CSV header. Cannot create a CSV reader instance with duplicate header");
                    }

                    headercollection.Add(item, null);
                }
            }
            else
            {
                //just open and close the file with read of first line to determine how many 
                //rows are there and then add default rows as  row1,row2 etc to collection.
                Read();
                _csvHeaderstring = _csvlinestring;
                Header = ReadRow(_csvHeaderstring);
                var i = 0;
                _csvHeaderstring = "";
                foreach (var item in Header)//read each column and create a dummy header.
                {
                    headercollection.Add("COL_" + i.ToString(), null);
                    _csvHeaderstring = _csvHeaderstring + "COL_" + i.ToString() + _delimiter;
                    i++;
                }

                _csvHeaderstring.TrimEnd(_delimiter);
                Header = ReadRow(_csvHeaderstring);
                Close(); //close and reopen to get the record position to beginning.
                _file = File.OpenText(filePath);
            }

            _iscolumnlocked = false; //setting this to false since above read is called 
                                     //internally during constructor and actual user read() did not start.
            _csvlinestring = "";
            Line = null;
            RecordsAffected = 0;
        }

        /// <summary>
        /// Read the next row.
        /// </summary>
        /// <returns>True if not at end of file. False otherwise.</returns>
        public bool Read()
        {
            var result = !_file.EndOfStream;
            if (result == true)
            {
                _csvlinestring = _file.ReadLine();
                if (_virtuallineString == "")
                {
                    Line = emptyAsNull ? ReadRowWithNulls(_csvlinestring) : ReadRow(_csvlinestring);
                }
                else
                {
                    var str = _virtuallineString + _delimiter + _csvlinestring;
                    Line = emptyAsNull ? ReadRowWithNulls(str) : ReadRow(str);
                }

                RecordsAffected++;
            }

            if (_iscolumnlocked == false)
            {
                _iscolumnlocked = true;
            }

            return result;
        }

        /// <summary>
        /// Adds a new virtual column at the beginning of each row. 
        /// If a virtual column exists then the new one is placed left of the first one. 
        /// Adding virtual column is possible only before read is made.
        /// </summary>
        /// <param name="columnName">Name of the header of column</param>
        /// <param name="value">Value for this column. This will be returned for every row 
        /// for this column until the value for this column is changed through method 
        /// 'UpdateVirtualcolumnValues'</param>
        /// <returns>Success status</returns>
        public bool AddVirtualColumn(string columnName, string value)
        {
            if (value == null)
            {
                return false;
            }

            if (_iscolumnlocked == true)
            {
                throw new Exception("Cannot add new records after Read() is called.");
            }

            if (headercollection.Contains(columnName) == true)
            {
                throw new Exception("Duplicate found in CSV header. Cannot create a CSV reader instance with duplicate header");
            }

            //add this to main collection so that 
            //we can check for duplicates next time col is added.
            headercollection.Add(columnName, value);

            _virtualHeaderString = _virtualHeaderString == "" ? columnName : columnName + _delimiter + _virtualHeaderString;

            Header = ReadRow(_virtualHeaderString + _delimiter + _csvHeaderstring);

            _virtuallineString = _virtuallineString == "" ? value : value + _delimiter + _virtuallineString;

            Line = ReadRow(_virtuallineString + _delimiter + _csvlinestring);
            return true;
        }

        /// <summary>
        /// Update the column header. This method must be called before Read() method is called. 
        /// Otherwise it will throw an exception.
        /// </summary>
        /// <param name="oldColumnName"></param>
        /// <param name="newColumnName"></param>
        /// <returns>Success status</returns>
        public bool RenameCSVHeader(string oldColumnName, string newColumnName)
        {
            if (_iscolumnlocked == true)
            {
                throw new Exception("Cannot update header after Read() is called.");
            }

            if (headercollection.Contains(oldColumnName) == false)
            {
                throw new Exception("CSV header not found. Cannot update.");
            }

            var value = headercollection[oldColumnName]?.ToString();
            var i = 0;

            //this collection does no have a position 
            //location property so using this way assuming the key is ordered
            foreach (var item in headercollection.Keys)
            {
                if (item.ToString() == oldColumnName)
                {
                    break;
                }

                i++;
            }

            headercollection.RemoveAt(i);
            headercollection.Insert(i, newColumnName, value);

            if (value == null) //csv header update.
            {
                _csvHeaderstring = _csvHeaderstring.Replace(oldColumnName, newColumnName);
                Header = ReadRow(_virtualHeaderString + _delimiter + _csvHeaderstring);
            }
            else //virtual header update
            {
                _virtualHeaderString = _virtualHeaderString.Replace(oldColumnName, newColumnName);
                Header = ReadRow(_virtualHeaderString + _delimiter + _csvHeaderstring);
            }

            return true;
        }

        /// <summary>
        /// Updates the value of the virtual column if it exists. Else throws exception.
        /// </summary>
        /// <param name="columnName">Name of the header of column</param>
        /// <param name="value">Value for this column. 
        /// This new value will be returned for every row for this column until 
        /// the value for this column is changed again</param>
        /// <returns>Success status</returns>
        public bool UpdateVirtualColumnValue(string columnName, string value)
        {
            if (value == null)
            {
                return false;
            }

            if (headercollection.Contains(columnName) == false)
            {
                throw new Exception("Unable to find the csv header. Cannot update value.");
            }

            if (headercollection.Contains(columnName) == true && headercollection[columnName] == null)
            {
                throw new Exception("Cannot update values for default csv based columns.");
            }

            //add this to main collection so that 
            //we can check for duplicates next time col is added.
            headercollection[columnName] = value;
            _virtuallineString = "";

            //cannot use string.replace since 
            //values may be duplicated and can update wrong column. So rebuilding the string.
            foreach (var item in headercollection.Values)
            {
                if (item != null)
                {
                    _virtuallineString = (string)item + _delimiter + _virtuallineString;
                }
            }

            _virtuallineString = _virtuallineString.TrimEnd(',');
            Line = ReadRow(_virtuallineString + _delimiter + _csvlinestring);

            return true;
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <returns>array of strings from csv line</returns>
        private string[] ReadRow(string line)
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(line) == true)
            {
                return null;
            }

            var pos = 0;
            var rows = 0;
            while (pos < line.Length)
            {
                string value;

                // Special handling for quoted field
                if (line[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    var start = pos;
                    while (pos < line.Length)
                    {
                        // Test for quote character
                        if (line[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= line.Length || line[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }

                        pos++;
                    }

                    value = line.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    var start = pos;
                    while (pos < line.Length && line[pos] != _delimiter)
                    {
                        pos++;
                    }

                    value = line.Substring(start, pos - start);
                }
                // Add field to list
                if (rows < lines.Count)
                {
                    lines[rows] = value;
                }
                else
                {
                    lines.Add(value);
                }

                rows++;

                // Eat up to and including next comma
                while (pos < line.Length && line[pos] != _delimiter)
                {
                    pos++;
                }

                if (pos < line.Length)
                {
                    pos++;
                }
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <returns>array of strings from csv line</returns>
        private string[] ReadRowWithNulls(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            var splitStr = line.SplitIgnoreComma($"{_delimiter}", true, false);
            return splitStr;
        }

        /// <summary>
        /// Close the <see cref="IDataReader"/>.
        /// </summary>
        public void Close()
        {
            _file.Close();
            _file.Dispose();
            _file = null;
        }

        /// <summary>
        /// Gets a value that indicates the depth of nesting for the current row.
        /// </summary>
        public int Depth => 1;

        /// <summary>
        /// Get a <see cref="DataTable"/> with just the columns.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> instance.</returns>
        public DataTable GetSchemaTable()
        {
            var t = new DataTable();
            t.Rows.Add(Header);
            return t;
        }

        /// <summary>
        /// Whether or not the file is closed.
        /// </summary>
        public bool IsClosed => _file == null;

        /// <summary>
        /// Read the next line.
        /// </summary>
        /// <returns>True if successful. False otherwise.</returns>
        public bool NextResult() => Read();

        /// <summary>
        /// Returns how many records read so far.
        /// </summary>
        public int RecordsAffected { get; private set; }

        /// <summary>
        /// Clean up the file.
        /// </summary>
        public void Dispose()
        {
            if (_file != null)
            {
                _file.Dispose();
                _file = null;
            }
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        public int FieldCount => Header.Length;

        /// <summary>
        /// Get a <see cref="bool"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="bool"/> value.</returns>
        public bool GetBoolean(int i) => bool.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="byte"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="byte"/> value.</returns>
        public byte GetByte(int i) => byte.Parse(Line[i]);

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

        /// <summary>
        /// Get a <see cref="char"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="char"/> value.</returns>
        public char GetChar(int i) => char.Parse(Line[i]);

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

        /// <summary>
        /// Get the current <see cref="IDataReader"/> regardless of the index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>An <see cref="IDataReader"/> object.</returns>
        public IDataReader GetData(int i) => this;

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetDataTypeName(int i) => throw new NotImplementedException();

        /// <summary>
        /// Get a <see cref="DateTime"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="DateTime"/> value.</returns>
        public DateTime GetDateTime(int i) => DateTime.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="decimal"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="decimal"/> value.</returns>
        public decimal GetDecimal(int i) => decimal.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="double"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="double"/> value.</returns>
        public double GetDouble(int i) => double.Parse(Line[i]);

        /// <summary>
        /// Returns <see cref="string"/> <see cref="Type"/>.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i) => typeof(string);

        /// <summary>
        /// Get a <see cref="float"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="float"/> value.</returns>
        public float GetFloat(int i) => float.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="Guid"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="Guid"/> value.</returns>
        public Guid GetGuid(int i) => Guid.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="short"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="short"/> value.</returns>
        public short GetInt16(int i) => short.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="int"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="int"/> value.</returns>
        public int GetInt32(int i) => int.Parse(Line[i]);

        /// <summary>
        /// Get a <see cref="long"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="long"/> value.</returns>
        public long GetInt64(int i) => long.Parse(Line[i]);

        /// <summary>
        /// Gets the column name at index i.
        /// </summary>
        /// <param name="i">Index of the column.</param>
        /// <returns>A <see cref="string"/> header name.</returns>
        public string GetName(int i) => Header[i];

        /// <summary>
        /// Get the index of the column name.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>An <see cref="int"/> index of the column name.</returns>
        public int GetOrdinal(string name)
        {
            var result = -1;
            for (var i = 0; i < Header.Length; i++)
            {
                if (Header[i] == name)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get a <see cref="string"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="string"/> value.</returns>
        public string GetString(int i) => Line[i];

        /// <summary>
        /// Get a <see cref="object"/> from column at index i.
        /// </summary>
        /// <param name="i">The column index.</param>
        /// <returns>A <see cref="object"/> value.</returns>
        public object GetValue(int i) => Line[i];

        /// <summary>
        /// Sets the current line to the provided object array.
        /// </summary>
        /// <param name="values">An array to assign the current line to.</param>
        /// <returns>A 1.</returns>
        public int GetValues(object[] values)
        {
            values = Line;
            return 1;
        }

        /// <summary>
        /// Check if the current value is null.
        /// </summary>
        /// <param name="i">Index of column.</param>
        /// <returns>True if value is null or whitespace. False otherwise.</returns>
        public bool IsDBNull(int i) => string.IsNullOrWhiteSpace(Line[i]);

        /// <summary>
        /// Get value for column name in current line.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>An <see cref="object"/> value.</returns>
        public object this[string name] => Line[GetOrdinal(name)];

        /// <summary>
        /// Get value for column index in current line.
        /// </summary>
        /// <param name="i">The index of the column.</param>
        /// <returns>An <see cref="object"/> value.</returns>
        public object this[int i] => GetValue(i);
    }
}