using System;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a table row
    /// </summary>
    public interface ITableRow
    {
        /// <summary>
        /// Collection of cells in this row
        /// </summary>
        ITableCells Cells { get; }
        
        /// <summary>
        /// Row index (0-based)
        /// </summary>
        int Index { get; }
        
        /// <summary>
        /// Parent table
        /// </summary>
        ITable Table { get; }
        
        /// <summary>
        /// Get cell by column index (0-based)
        /// </summary>
        ITableCell this[int columnIndex] { get; }
        
        /// <summary>
        /// Add a new cell to this row
        /// </summary>
        ITableCell AddCell();
        
        /// <summary>
        /// Add a new cell with content
        /// </summary>
        ITableCell AddCell(string content);
        
        /// <summary>
        /// Remove cell by column index
        /// </summary>
        void RemoveCell(int columnIndex);
    }
}
