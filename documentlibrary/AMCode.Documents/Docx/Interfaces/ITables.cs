using System.Collections.Generic;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of tables in a document
    /// </summary>
    public interface ITables : IEnumerable<ITable>
    {
        /// <summary>
        /// Get table by index (0-based)
        /// </summary>
        ITable this[int index] { get; }
        
        /// <summary>
        /// Number of tables
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent document
        /// </summary>
        IDocument Document { get; }
        
        /// <summary>
        /// Create a new table with specified dimensions
        /// </summary>
        ITable Create(int rows, int columns);
        
        /// <summary>
        /// Create a new table with specified dimensions and style
        /// </summary>
        ITable Create(int rows, int columns, ITableStyle style);
        
        /// <summary>
        /// Remove table by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove table by reference
        /// </summary>
        void Remove(ITable table);
    }
}
