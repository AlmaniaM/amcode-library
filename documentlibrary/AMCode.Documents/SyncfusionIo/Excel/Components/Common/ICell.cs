namespace AMCode.SyncfusionIo.Xlsx.Common
{
    /// <summary>
    /// An interface designed to represent a cell.
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// The cell value.
        /// </summary>
        ICellValue CellValue { get; set; }

        /// <summary>
        /// The one-based column index.
        /// </summary>
        int ColumnIndex { get; set; }

        /// <summary>
        /// The one-based row index.
        /// </summary>
        int RowIndex { get; set; }
    }
}