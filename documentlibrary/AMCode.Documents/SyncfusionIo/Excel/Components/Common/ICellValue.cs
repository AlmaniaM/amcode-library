using System;

namespace AMCode.SyncfusionIo.Xlsx.Common
{
    /// <summary>
    /// An interface designed to represent a cell value.
    /// </summary>
    public interface ICellValue
    {
        /// <summary>
        /// The cell value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// The cell value data type.
        /// </summary>
        Type ValueType { get; set; }
    }
}