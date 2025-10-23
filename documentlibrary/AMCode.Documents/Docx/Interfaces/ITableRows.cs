using System.Collections.Generic;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of table rows
    /// </summary>
    public interface ITableRows : IEnumerable<ITableRow>
    {
        /// <summary>
        /// Get row by index (0-based)
        /// </summary>
        ITableRow this[int index] { get; }
        
        /// <summary>
        /// Number of rows
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent table
        /// </summary>
        ITable Table { get; }
        
        /// <summary>
        /// Create a new row
        /// </summary>
        ITableRow Create();
        
        /// <summary>
        /// Remove row by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove row by reference
        /// </summary>
        void Remove(ITableRow row);
    }
}
