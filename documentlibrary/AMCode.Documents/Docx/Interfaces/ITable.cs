using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a table in a Word document
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Collection of rows
        /// </summary>
        ITableRows Rows { get; }
        
        /// <summary>
        /// Collection of columns
        /// </summary>
        ITableColumns Columns { get; }
        
        /// <summary>
        /// Table style
        /// </summary>
        ITableStyle Style { get; set; }
        
        /// <summary>
        /// Number of rows
        /// </summary>
        int RowCount { get; }
        
        /// <summary>
        /// Number of columns
        /// </summary>
        int ColumnCount { get; }
        
        /// <summary>
        /// Parent tables collection
        /// </summary>
        ITables Tables { get; }
        
        /// <summary>
        /// Get cell by row and column (0-based)
        /// </summary>
        ITableCell this[int row, int column] { get; }
        
        /// <summary>
        /// Add a new row
        /// </summary>
        ITableRow AddRow();
        
        /// <summary>
        /// Add a new column
        /// </summary>
        ITableColumn AddColumn();
        
        /// <summary>
        /// Remove row by index
        /// </summary>
        void RemoveRow(int index);
        
        /// <summary>
        /// Remove column by index
        /// </summary>
        void RemoveColumn(int index);
        
        /// <summary>
        /// Apply table style
        /// </summary>
        void ApplyStyle(ITableStyle style);
        
        /// <summary>
        /// Set cell value
        /// </summary>
        void SetCellValue(int row, int column, string value);
        
        /// <summary>
        /// Get cell value
        /// </summary>
        string GetCellValue(int row, int column);
    }
}
