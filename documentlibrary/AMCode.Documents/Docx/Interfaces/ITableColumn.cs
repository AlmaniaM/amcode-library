using System;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a table column
    /// </summary>
    public interface ITableColumn
    {
        /// <summary>
        /// Column index (0-based)
        /// </summary>
        int Index { get; }
        
        /// <summary>
        /// Column width
        /// </summary>
        double Width { get; set; }
        
        /// <summary>
        /// Parent table
        /// </summary>
        ITable Table { get; }
        
        /// <summary>
        /// Get cell by row index (0-based)
        /// </summary>
        ITableCell this[int rowIndex] { get; }
        
        /// <summary>
        /// Set column width
        /// </summary>
        void SetWidth(double width);
    }
}
