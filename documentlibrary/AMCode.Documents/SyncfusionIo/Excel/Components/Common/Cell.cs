namespace AMCode.SyncfusionIo.Xlsx.Common
{
    /// <summary>
    /// A class designed to represent the coordinates of a cell.
    /// </summary>
    public class Cell : ICell
    {
        /// <inheritdoc/>
        public ICellValue CellValue { get; set; }

        /// <inheritdoc/>
        public int ColumnIndex { get; set; }

        /// <inheritdoc/>
        public int RowIndex { get; set; }
    }
}