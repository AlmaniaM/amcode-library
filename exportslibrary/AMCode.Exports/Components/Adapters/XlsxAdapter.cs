using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Exports.Book;
using AMCode.Xlsx;

namespace AMCode.Exports.Adapters
{
    /// <summary>
    /// Adapter class to bridge between AMCode.Exports and AMCode.Xlsx interfaces
    /// </summary>
    public static class XlsxAdapter
    {
        /// <summary>
        /// Convert AMCode.Exports IColumnStyleParam to AMCode.Xlsx IColumnStyleParam
        /// Since AMCode.Exports IColumnStyleParam inherits from AMCode.Xlsx IStyleParam,
        /// we can cast directly
        /// </summary>
        /// <param name="exportsStyleParam">The AMCode.Exports style parameter</param>
        /// <returns>The AMCode.Xlsx style parameter</returns>
        public static AMCode.Xlsx.IColumnStyleParam ConvertToXlsxStyleParam(AMCode.Exports.Book.IColumnStyleParam exportsStyleParam)
        {
            if (exportsStyleParam == null)
                return null;

            // Since AMCode.Exports IColumnStyleParam inherits from AMCode.Xlsx IStyleParam,
            // we can create a new ColumnStyleParam with the same values
            return new AMCode.Xlsx.ColumnStyleParam
            {
                ColumnIndex = exportsStyleParam.ColumnIndex,
                ColumnName = exportsStyleParam.ColumnName,
                Bold = exportsStyleParam.Bold,
                Italic = exportsStyleParam.Italic,
                FontSize = exportsStyleParam.FontSize,
                Color = exportsStyleParam.Color,
                FontColor = exportsStyleParam.FontColor,
                NumberFormat = exportsStyleParam.NumberFormat,
                HorizontalAlignment = exportsStyleParam.HorizontalAlignment,
                FillPattern = exportsStyleParam.FillPattern,
                PatternColor = exportsStyleParam.PatternColor,
                ColorIndex = exportsStyleParam.ColorIndex,
                BorderStyles = exportsStyleParam.BorderStyles
            };
        }

        /// <summary>
        /// Convert AMCode.Exports IStyleParam to AMCode.Xlsx IStyleParam
        /// </summary>
        /// <param name="exportsStyleParam">The AMCode.Exports style parameter</param>
        /// <returns>The AMCode.Xlsx style parameter</returns>
        public static AMCode.Xlsx.IStyleParam ConvertToXlsxStyleParam(AMCode.Xlsx.IStyleParam exportsStyleParam)
        {
            if (exportsStyleParam == null)
                return null;

            return new AMCode.Xlsx.StyleParam
            {
                Bold = exportsStyleParam.Bold,
                Italic = exportsStyleParam.Italic,
                FontSize = exportsStyleParam.FontSize,
                Color = exportsStyleParam.Color,
                FontColor = exportsStyleParam.FontColor,
                NumberFormat = exportsStyleParam.NumberFormat,
                HorizontalAlignment = exportsStyleParam.HorizontalAlignment,
                FillPattern = exportsStyleParam.FillPattern,
                PatternColor = exportsStyleParam.PatternColor,
                ColorIndex = exportsStyleParam.ColorIndex,
                BorderStyles = exportsStyleParam.BorderStyles
            };
        }
    }
}
