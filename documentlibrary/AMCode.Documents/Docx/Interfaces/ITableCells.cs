using System.Collections.Generic;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Collection of table cells
    /// </summary>
    public interface ITableCells : IEnumerable<ITableCell>
    {
        /// <summary>
        /// Get cell by index (0-based)
        /// </summary>
        ITableCell this[int index] { get; }
        
        /// <summary>
        /// Number of cells
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Parent row
        /// </summary>
        ITableRow Row { get; }
        
        /// <summary>
        /// Create a new cell
        /// </summary>
        ITableCell Create();
        
        /// <summary>
        /// Create a new cell with content
        /// </summary>
        ITableCell Create(string content);
        
        /// <summary>
        /// Remove cell by index
        /// </summary>
        void Remove(int index);
        
        /// <summary>
        /// Remove cell by reference
        /// </summary>
        void Remove(ITableCell cell);
    }
}
