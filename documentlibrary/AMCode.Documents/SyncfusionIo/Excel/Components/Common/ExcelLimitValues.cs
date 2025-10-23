using AMCode.SyncfusionIo.Xlsx.Common;

namespace AMCode.SyncfusionIo.Xlsx.Common
{
    /// <summary>
    /// A class designed to hold Excel max number of things.
    /// </summary>
    public class ExcelLimitValues
    {
        /// <summary>
        /// Get the max number of columns for an .xlsx file.
        /// </summary>
        public static int MaxColumnCount => 16384;

        /// <summary>
        /// Get the max number of rows for an .xlsx file.
        /// </summary>
        public static int MaxRowCount => 1048576;
    }
}