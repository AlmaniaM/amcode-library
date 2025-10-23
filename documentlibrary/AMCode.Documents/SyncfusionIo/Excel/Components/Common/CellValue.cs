using System;

namespace AMCode.SyncfusionIo.Xlsx.Common
{
    /// <summary>
    /// A class designed to represent a cell value.
    /// </summary>
    public class CellValue : ICellValue
    {
        /// <inheritdoc/>
        public object Value { get; set; }

        /// <inheritdoc/>
        public Type ValueType { get; set; }
    }
}