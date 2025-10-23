using System.Collections.Generic;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of table columns
    /// </summary>
    public interface ITableColumns : IEnumerable<ITableColumn>
    {
        /// <summary>
        /// Get column by index (0-based)
        /// </summary>
        ITableColumn this[int index] { get; }
        
        /// <summary>
        /// Number of columns
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent table
        /// </summary>
        ITable Table { get; }
        
        /// <summary>
        /// Create a new column
        /// </summary>
        ITableColumn Create();
        
        /// <summary>
        /// Remove column by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove column by reference
        /// </summary>
        void Remove(ITableColumn column);
    }
}
