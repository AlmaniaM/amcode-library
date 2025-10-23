using AMCode.SyncfusionIo.Xlsx.Common;

namespace AMCode.SyncfusionIo.Xlsx.Extensions
{
    /// <summary>
    /// A static class designed to hold extension methods for the <see cref="ExcelBordersIndex"/> type.
    /// </summary>
    public static class ExcelBorderIndexExtensions
    {
        /// <summary>
        /// Check if the border index represents any of the outer cell edges.
        /// </summary>
        /// <param name="borderIndex">The border index to check.</param>
        /// <returns>A boolean of <c>true</c> if the border index represents an outer cell edge. Otherwise, it'll return <c>false</c>.</returns>
        public static bool IsOuterEdge(this ExcelBordersIndex borderIndex)
        {
            return borderIndex == ExcelBordersIndex.EdgeTop
                || borderIndex == ExcelBordersIndex.EdgeRight
                || borderIndex == ExcelBordersIndex.EdgeBottom
                || borderIndex == ExcelBordersIndex.EdgeLeft;
        }
    }
}