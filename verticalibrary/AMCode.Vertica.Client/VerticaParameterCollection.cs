using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Vertica.Data.VerticaClient
{
    /// <summary>
    /// Mock Vertica parameter collection implementation for local development.
    /// </summary>
    public class VerticaParameterCollection : DbParameterCollection
    {
        private readonly ArrayList _parameters = new ArrayList();

        public override int Count => _parameters.Count;

        public override bool IsFixedSize => false;

        public override bool IsReadOnly => false;

        public override bool IsSynchronized => false;

        public override object SyncRoot => this;

        public override int Add(object value)
        {
            return _parameters.Add(value);
        }

        public override void AddRange(Array values)
        {
            foreach (var value in values)
            {
                _parameters.Add(value);
            }
        }

        public override void Clear()
        {
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            return _parameters.Contains(value);
        }

        public override bool Contains(string value)
        {
            return false; // Mock implementation
        }

        public override void CopyTo(Array array, int index)
        {
            _parameters.CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            return _parameters.IndexOf(value);
        }

        public override int IndexOf(string parameterName)
        {
            return -1; // Mock implementation
        }

        public override void Insert(int index, object value)
        {
            _parameters.Insert(index, value);
        }

        public override void Remove(object value)
        {
            _parameters.Remove(value);
        }

        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            // Mock implementation
        }

        protected override DbParameter GetParameter(int index)
        {
            return (DbParameter)_parameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return null; // Mock implementation
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            _parameters[index] = value;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            // Mock implementation
        }
    }
}
