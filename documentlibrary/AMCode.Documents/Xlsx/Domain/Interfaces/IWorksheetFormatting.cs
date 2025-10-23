using System;
using System.Drawing;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Interface for worksheet formatting operations
    /// Provides methods for applying and managing cell and range formatting
    /// </summary>
    public interface IWorksheetFormatting
    {
        /// <summary>
        /// Gets or sets the default font name for the worksheet
        /// </summary>
        string DefaultFontName { get; set; }

        /// <summary>
        /// Gets or sets the default font size for the worksheet
        /// </summary>
        double DefaultFontSize { get; set; }

        /// <summary>
        /// Gets or sets the default font color for the worksheet
        /// </summary>
        Color DefaultFontColor { get; set; }

        /// <summary>
        /// Gets or sets the default background color for the worksheet
        /// </summary>
        Color DefaultBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether the worksheet is visible
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Sets the font name for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="fontName">The font name</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontName(string cellReference, string fontName);

        /// <summary>
        /// Sets the font name for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="fontName">The font name</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontName(int row, int column, string fontName);

        /// <summary>
        /// Sets the font size for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="fontSize">The font size</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontSize(string cellReference, double fontSize);

        /// <summary>
        /// Sets the font size for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="fontSize">The font size</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontSize(int row, int column, double fontSize);

        /// <summary>
        /// Sets the font color for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="color">The font color</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontColor(string cellReference, Color color);

        /// <summary>
        /// Sets the font color for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="color">The font color</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontColor(int row, int column, Color color);

        /// <summary>
        /// Sets the background color for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="color">The background color</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellBackgroundColor(string cellReference, Color color);

        /// <summary>
        /// Sets the background color for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="color">The background color</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellBackgroundColor(int row, int column, Color color);

        /// <summary>
        /// Sets the font style for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="isBold">Whether the font should be bold</param>
        /// <param name="isItalic">Whether the font should be italic</param>
        /// <param name="isUnderline">Whether the font should be underlined</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontStyle(string cellReference, bool isBold = false, bool isItalic = false, bool isUnderline = false);

        /// <summary>
        /// Sets the font style for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="isBold">Whether the font should be bold</param>
        /// <param name="isItalic">Whether the font should be italic</param>
        /// <param name="isUnderline">Whether the font should be underlined</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFontStyle(int row, int column, bool isBold = false, bool isItalic = false, bool isUnderline = false);

        /// <summary>
        /// Sets the horizontal alignment for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="alignment">The horizontal alignment</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellHorizontalAlignment(string cellReference, HorizontalAlignment alignment);

        /// <summary>
        /// Sets the horizontal alignment for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="alignment">The horizontal alignment</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellHorizontalAlignment(int row, int column, HorizontalAlignment alignment);

        /// <summary>
        /// Sets the vertical alignment for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="alignment">The vertical alignment</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellVerticalAlignment(string cellReference, VerticalAlignment alignment);

        /// <summary>
        /// Sets the vertical alignment for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="alignment">The vertical alignment</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellVerticalAlignment(int row, int column, VerticalAlignment alignment);

        /// <summary>
        /// Sets the text wrapping for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="wrapText">Whether text should wrap</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellTextWrapping(string cellReference, bool wrapText);

        /// <summary>
        /// Sets the text wrapping for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="wrapText">Whether text should wrap</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellTextWrapping(int row, int column, bool wrapText);

        /// <summary>
        /// Sets the number format for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="numberFormat">The number format</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellNumberFormat(string cellReference, string numberFormat);

        /// <summary>
        /// Sets the number format for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="numberFormat">The number format</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellNumberFormat(int row, int column, string numberFormat);

        /// <summary>
        /// Sets the border style for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="borderStyle">The border style</param>
        /// <param name="borderColor">The border color</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellBorder(string cellReference, BorderStyle borderStyle, Color borderColor);

        /// <summary>
        /// Sets the border style for a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="borderStyle">The border style</param>
        /// <param name="borderColor">The border color</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellBorder(int row, int column, BorderStyle borderStyle, Color borderColor);

        /// <summary>
        /// Sets the row height for a specific row
        /// </summary>
        /// <param name="rowIndex">The row index (0-based)</param>
        /// <param name="height">The row height</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetRowHeight(int rowIndex, double height);

        /// <summary>
        /// Sets the column width for a specific column
        /// </summary>
        /// <param name="columnIndex">The column index (0-based)</param>
        /// <param name="width">The column width</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetColumnWidth(int columnIndex, double width);

        /// <summary>
        /// Auto-fits the row height based on content
        /// </summary>
        /// <param name="rowIndex">The row index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitRow(int rowIndex);

        /// <summary>
        /// Auto-fits the column width based on content
        /// </summary>
        /// <param name="columnIndex">The column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitColumn(int columnIndex);

        /// <summary>
        /// Auto-fits all rows based on content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitAllRows();

        /// <summary>
        /// Auto-fits all columns based on content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitAllColumns();

        /// <summary>
        /// Clears all formatting from a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearCellFormatting(string cellReference);

        /// <summary>
        /// Clears all formatting from a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearCellFormatting(int row, int column);

        /// <summary>
        /// Clears all formatting from a specific range
        /// </summary>
        /// <param name="rangeReference">The range reference</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearRangeFormatting(string rangeReference);

        /// <summary>
        /// Clears all formatting from a specific range
        /// </summary>
        /// <param name="startRow">The start row index (0-based)</param>
        /// <param name="startColumn">The start column index (0-based)</param>
        /// <param name="endRow">The end row index (0-based)</param>
        /// <param name="endColumn">The end column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearRangeFormatting(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Clears all formatting from the entire worksheet
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearAllFormatting();

        /// <summary>
        /// Copies formatting from one cell to another
        /// </summary>
        /// <param name="sourceCell">The source cell reference</param>
        /// <param name="destinationCell">The destination cell reference</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyCellFormatting(string sourceCell, string destinationCell);

        /// <summary>
        /// Copies formatting from one cell to another
        /// </summary>
        /// <param name="sourceRow">The source row index (0-based)</param>
        /// <param name="sourceColumn">The source column index (0-based)</param>
        /// <param name="destinationRow">The destination row index (0-based)</param>
        /// <param name="destinationColumn">The destination column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyCellFormatting(int sourceRow, int sourceColumn, int destinationRow, int destinationColumn);

        /// <summary>
        /// Copies formatting from one range to another
        /// </summary>
        /// <param name="sourceRange">The source range reference</param>
        /// <param name="destinationRange">The destination range reference</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyRangeFormatting(string sourceRange, string destinationRange);

        /// <summary>
        /// Copies formatting from one range to another
        /// </summary>
        /// <param name="sourceStartRow">The source start row index (0-based)</param>
        /// <param name="sourceStartColumn">The source start column index (0-based)</param>
        /// <param name="sourceEndRow">The source end row index (0-based)</param>
        /// <param name="sourceEndColumn">The source end column index (0-based)</param>
        /// <param name="destinationStartRow">The destination start row index (0-based)</param>
        /// <param name="destinationStartColumn">The destination start column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyRangeFormatting(int sourceStartRow, int sourceStartColumn, int sourceEndRow, int sourceEndColumn,
                                  int destinationStartRow, int destinationStartColumn);
    }

    /// <summary>
    /// Enumeration for horizontal alignment options
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Left alignment
        /// </summary>
        Left,

        /// <summary>
        /// Center alignment
        /// </summary>
        Center,

        /// <summary>
        /// Right alignment
        /// </summary>
        Right,

        /// <summary>
        /// Justify alignment
        /// </summary>
        Justify,

        /// <summary>
        /// General alignment (default)
        /// </summary>
        General
    }

    /// <summary>
    /// Enumeration for vertical alignment options
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// Top alignment
        /// </summary>
        Top,

        /// <summary>
        /// Middle alignment
        /// </summary>
        Middle,

        /// <summary>
        /// Bottom alignment
        /// </summary>
        Bottom,

        /// <summary>
        /// Justify alignment
        /// </summary>
        Justify
    }

    /// <summary>
    /// Enumeration for border style options
    /// </summary>
    public enum BorderStyle
    {
        /// <summary>
        /// No border
        /// </summary>
        None,

        /// <summary>
        /// Thin border
        /// </summary>
        Thin,

        /// <summary>
        /// Medium border
        /// </summary>
        Medium,

        /// <summary>
        /// Thick border
        /// </summary>
        Thick,

        /// <summary>
        /// Dotted border
        /// </summary>
        Dotted,

        /// <summary>
        /// Dashed border
        /// </summary>
        Dashed,

        /// <summary>
        /// Double border
        /// </summary>
        Double
    }
}