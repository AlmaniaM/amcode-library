using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Fluent API interface for building worksheets
    /// Provides a builder pattern for creating and configuring worksheets
    /// </summary>
    public interface IWorksheetBuilder
    {
        /// <summary>
        /// Sets the name of the worksheet
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithName(string name);

        /// <summary>
        /// Sets the index of the worksheet in the workbook
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithIndex(int index);

        /// <summary>
        /// Sets the visibility of the worksheet
        /// </summary>
        /// <param name="isVisible">True to make visible, false to hide</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithVisibility(bool isVisible);

        /// <summary>
        /// Sets the tab color of the worksheet
        /// </summary>
        /// <param name="color">The tab color (hex format, e.g., "FF0000")</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithTabColor(string color);

        /// <summary>
        /// Sets the tab color of the worksheet using RGB values
        /// </summary>
        /// <param name="red">Red component (0-255)</param>
        /// <param name="green">Green component (0-255)</param>
        /// <param name="blue">Blue component (0-255)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithTabColor(int red, int green, int blue);

        /// <summary>
        /// Sets the protection password for the worksheet
        /// </summary>
        /// <param name="password">The protection password</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithProtection(string password);

        /// <summary>
        /// Sets the protection options for the worksheet
        /// </summary>
        /// <param name="options">The protection options</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithProtection(WorksheetProtectionOptions options);

        /// <summary>
        /// Sets the default row height for the worksheet
        /// </summary>
        /// <param name="height">The row height in points</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultRowHeight(double height);

        /// <summary>
        /// Sets the default column width for the worksheet
        /// </summary>
        /// <param name="width">The column width in characters</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultColumnWidth(double width);

        /// <summary>
        /// Sets the zoom level for the worksheet
        /// </summary>
        /// <param name="zoomLevel">The zoom level as a percentage (10-400)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithZoomLevel(int zoomLevel);

        /// <summary>
        /// Sets the freeze panes for the worksheet
        /// </summary>
        /// <param name="row">The row to freeze at (0-based)</param>
        /// <param name="column">The column to freeze at (0-based)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithFreezePanes(int row, int column);

        /// <summary>
        /// Sets the gridlines visibility for the worksheet
        /// </summary>
        /// <param name="showGridlines">True to show gridlines, false to hide</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithGridlines(bool showGridlines);

        /// <summary>
        /// Sets the headers visibility for the worksheet
        /// </summary>
        /// <param name="showHeaders">True to show headers, false to hide</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithHeaders(bool showHeaders);

        /// <summary>
        /// Sets the print orientation for the worksheet
        /// </summary>
        /// <param name="orientation">The print orientation</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintOrientation(PrintOrientation orientation);

        /// <summary>
        /// Sets the print margins for the worksheet
        /// </summary>
        /// <param name="margins">The print margins</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintMargins(PrintMargins margins);

        /// <summary>
        /// Sets the print scaling for the worksheet
        /// </summary>
        /// <param name="scaling">The print scaling percentage</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintScaling(int scaling);

        /// <summary>
        /// Sets the print area for the worksheet
        /// </summary>
        /// <param name="range">The print area range (e.g., "A1:Z100")</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintArea(string range);

        /// <summary>
        /// Sets the print titles for the worksheet
        /// </summary>
        /// <param name="rows">The rows to repeat at top (e.g., "1:1")</param>
        /// <param name="columns">The columns to repeat at left (e.g., "A:A")</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintTitles(string rows, string columns);

        /// <summary>
        /// Sets the data for the worksheet from a 2D array
        /// </summary>
        /// <param name="data">The 2D array of data</param>
        /// <param name="startRow">The starting row (0-based)</param>
        /// <param name="startColumn">The starting column (0-based)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithData(object[,] data, int startRow = 0, int startColumn = 0);

        /// <summary>
        /// Sets the data for the worksheet from a list of objects
        /// </summary>
        /// <typeparam name="T">The type of objects in the list</typeparam>
        /// <param name="data">The list of objects</param>
        /// <param name="startRow">The starting row (0-based)</param>
        /// <param name="startColumn">The starting column (0-based)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithData<T>(IEnumerable<T> data, int startRow = 0, int startColumn = 0);

        /// <summary>
        /// Sets the data for the worksheet from a DataTable
        /// </summary>
        /// <param name="dataTable">The DataTable</param>
        /// <param name="startRow">The starting row (0-based)</param>
        /// <param name="startColumn">The starting column (0-based)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithData(System.Data.DataTable dataTable, int startRow = 0, int startColumn = 0);

        /// <summary>
        /// Sets the headers for the worksheet
        /// </summary>
        /// <param name="headers">The header values</param>
        /// <param name="row">The row to place headers in (0-based)</param>
        /// <param name="startColumn">The starting column (0-based)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithHeaders(IEnumerable<string> headers, int row = 0, int startColumn = 0);

        /// <summary>
        /// Sets the formulas for the worksheet
        /// </summary>
        /// <param name="formulas">The formulas dictionary (cell reference -> formula)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithFormulas(Dictionary<string, string> formulas);

        /// <summary>
        /// Sets a formula for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="formula">The formula</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithFormula(string cellReference, string formula);

        /// <summary>
        /// Sets the formatting for a range
        /// </summary>
        /// <param name="range">The range to format</param>
        /// <param name="formatting">The formatting settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithFormatting(string range, CellFormatting formatting);

        /// <summary>
        /// Sets the column widths for the worksheet
        /// </summary>
        /// <param name="widths">The column widths dictionary (column index -> width)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithColumnWidths(Dictionary<int, double> widths);

        /// <summary>
        /// Sets the width for a specific column
        /// </summary>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="width">The column width</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithColumnWidth(int column, double width);

        /// <summary>
        /// Sets the row heights for the worksheet
        /// </summary>
        /// <param name="heights">The row heights dictionary (row index -> height)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithRowHeights(Dictionary<int, double> heights);

        /// <summary>
        /// Sets the height for a specific row
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="height">The row height</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithRowHeight(int row, double height);

        /// <summary>
        /// Sets the auto-fit for columns
        /// </summary>
        /// <param name="columns">The columns to auto-fit (null for all)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithAutoFitColumns(IEnumerable<int> columns = null);

        /// <summary>
        /// Sets the auto-fit for rows
        /// </summary>
        /// <param name="rows">The rows to auto-fit (null for all)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithAutoFitRows(IEnumerable<int> rows = null);

        /// <summary>
        /// Sets the merge cells for the worksheet
        /// </summary>
        /// <param name="ranges">The ranges to merge</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithMergeCells(IEnumerable<string> ranges);

        /// <summary>
        /// Sets the merge cells for a specific range
        /// </summary>
        /// <param name="range">The range to merge</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithMergeCell(string range);

        /// <summary>
        /// Sets the split panes for the worksheet
        /// </summary>
        /// <param name="horizontalSplit">The horizontal split position</param>
        /// <param name="verticalSplit">The vertical split position</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithSplitPanes(int horizontalSplit, int verticalSplit);

        /// <summary>
        /// Sets the selection for the worksheet
        /// </summary>
        /// <param name="range">The range to select</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithSelection(string range);

        /// <summary>
        /// Sets the active cell for the worksheet
        /// </summary>
        /// <param name="cellReference">The active cell reference</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithActiveCell(string cellReference);

        /// <summary>
        /// Sets the data validation for a range
        /// </summary>
        /// <param name="range">The range to apply validation to</param>
        /// <param name="validation">The validation rules</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDataValidation(string range, DataValidationRule validation);

        /// <summary>
        /// Sets the conditional formatting for a range
        /// </summary>
        /// <param name="range">The range to apply formatting to</param>
        /// <param name="formatting">The conditional formatting rules</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithConditionalFormatting(string range, ConditionalFormattingRule formatting);

        /// <summary>
        /// Sets the auto-filter for the worksheet
        /// </summary>
        /// <param name="range">The range to apply auto-filter to</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithAutoFilter(string range);

        /// <summary>
        /// Sets the sort order for the worksheet
        /// </summary>
        /// <param name="sortOrder">The sort order configuration</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithSortOrder(SortOrderConfiguration sortOrder);

        /// <summary>
        /// Sets the grouping for the worksheet
        /// </summary>
        /// <param name="grouping">The grouping configuration</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithGrouping(GroupingConfiguration grouping);

        /// <summary>
        /// Sets the outline level for the worksheet
        /// </summary>
        /// <param name="level">The outline level</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithOutlineLevel(int level);

        /// <summary>
        /// Sets the summary row position for the worksheet
        /// </summary>
        /// <param name="position">The summary row position</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithSummaryRowPosition(SummaryRowPosition position);

        /// <summary>
        /// Sets the summary column position for the worksheet
        /// </summary>
        /// <param name="position">The summary column position</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithSummaryColumnPosition(SummaryColumnPosition position);

        /// <summary>
        /// Sets the calculation mode for the worksheet
        /// </summary>
        /// <param name="mode">The calculation mode</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithCalculationMode(CalculationMode mode);

        /// <summary>
        /// Sets the calculation order for the worksheet
        /// </summary>
        /// <param name="order">The calculation order</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithCalculationOrder(CalculationOrder order);

        /// <summary>
        /// Sets the iteration settings for the worksheet
        /// </summary>
        /// <param name="enabled">True to enable iteration</param>
        /// <param name="maxIterations">Maximum number of iterations</param>
        /// <param name="maxChange">Maximum change threshold</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithIteration(bool enabled, int maxIterations, double maxChange);

        /// <summary>
        /// Sets the precision as displayed for the worksheet
        /// </summary>
        /// <param name="enabled">True to enable precision as displayed</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrecisionAsDisplayed(bool enabled);

        /// <summary>
        /// Sets the date system for the worksheet
        /// </summary>
        /// <param name="system">The date system</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDateSystem(DateSystem system);

        /// <summary>
        /// Sets the reference style for the worksheet
        /// </summary>
        /// <param name="style">The reference style</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithReferenceStyle(ReferenceStyle style);

        /// <summary>
        /// Sets the default font for the worksheet
        /// </summary>
        /// <param name="font">The default font settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultFont(FontSettings font);

        /// <summary>
        /// Sets the default number format for the worksheet
        /// </summary>
        /// <param name="format">The number format string</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultNumberFormat(string format);

        /// <summary>
        /// Sets the default alignment for the worksheet
        /// </summary>
        /// <param name="alignment">The default alignment settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultAlignment(AlignmentSettings alignment);

        /// <summary>
        /// Sets the default border for the worksheet
        /// </summary>
        /// <param name="border">The default border settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultBorder(BorderSettings border);

        /// <summary>
        /// Sets the default fill for the worksheet
        /// </summary>
        /// <param name="fill">The default fill settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultFill(FillSettings fill);

        /// <summary>
        /// Sets the default protection for the worksheet
        /// </summary>
        /// <param name="protection">The default protection settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDefaultProtection(ProtectionSettings protection);

        /// <summary>
        /// Sets the custom properties for the worksheet
        /// </summary>
        /// <param name="properties">The custom properties dictionary</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithCustomProperties(Dictionary<string, object> properties);

        /// <summary>
        /// Sets a custom property for the worksheet
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithCustomProperty(string name, object value);

        /// <summary>
        /// Sets the comments for the worksheet
        /// </summary>
        /// <param name="comments">The comments dictionary (cell reference -> comment text)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithComments(Dictionary<string, string> comments);

        /// <summary>
        /// Sets a comment for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="comment">The comment text</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithComment(string cellReference, string comment);

        /// <summary>
        /// Sets the hyperlinks for the worksheet
        /// </summary>
        /// <param name="hyperlinks">The hyperlinks dictionary (cell reference -> URL)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithHyperlinks(Dictionary<string, string> hyperlinks);

        /// <summary>
        /// Sets a hyperlink for a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="url">The hyperlink URL</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithHyperlink(string cellReference, string url);

        /// <summary>
        /// Sets the named ranges for the worksheet
        /// </summary>
        /// <param name="namedRanges">The named ranges dictionary (name -> range)</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithNamedRanges(Dictionary<string, string> namedRanges);

        /// <summary>
        /// Sets a named range for the worksheet
        /// </summary>
        /// <param name="name">The named range name</param>
        /// <param name="range">The range reference</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithNamedRange(string name, string range);

        /// <summary>
        /// Sets the data validation for the worksheet
        /// </summary>
        /// <param name="validations">The data validation rules</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDataValidations(IEnumerable<DataValidationRule> validations);

        /// <summary>
        /// Sets the conditional formatting for the worksheet
        /// </summary>
        /// <param name="formattingRules">The conditional formatting rules</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithConditionalFormattingRules(IEnumerable<ConditionalFormattingRule> formattingRules);

        /// <summary>
        /// Sets the sort order for the worksheet
        /// </summary>
        /// <param name="sortOrders">The sort order configurations</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithSortOrders(IEnumerable<SortOrderConfiguration> sortOrders);

        /// <summary>
        /// Sets the grouping for the worksheet
        /// </summary>
        /// <param name="groupings">The grouping configurations</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithGroupings(IEnumerable<GroupingConfiguration> groupings);

        /// <summary>
        /// Sets the outline for the worksheet
        /// </summary>
        /// <param name="outline">The outline configuration</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithOutline(OutlineConfiguration outline);

        /// <summary>
        /// Sets the calculation settings for the worksheet
        /// </summary>
        /// <param name="calculationSettings">The calculation settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithCalculationSettings(CalculationSettings calculationSettings);

        /// <summary>
        /// Sets the display options for the worksheet
        /// </summary>
        /// <param name="displayOptions">The display options</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithDisplayOptions(DisplayOptions displayOptions);

        /// <summary>
        /// Sets the print settings for the worksheet
        /// </summary>
        /// <param name="printSettings">The print settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintSettings(PrintSettings printSettings);

        /// <summary>
        /// Sets the page setup for the worksheet
        /// </summary>
        /// <param name="pageSetup">The page setup settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPageSetup(PageSetup pageSetup);

        /// <summary>
        /// Sets the header and footer for the worksheet
        /// </summary>
        /// <param name="headerFooter">The header and footer settings</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithHeaderFooter(HeaderFooter headerFooter);

        /// <summary>
        /// Sets the print options for the worksheet
        /// </summary>
        /// <param name="printOptions">The print options</param>
        /// <returns>The worksheet builder for method chaining</returns>
        IWorksheetBuilder WithPrintOptions(PrintOptions printOptions);

        /// <summary>
        /// Builds the worksheet with all configured settings
        /// </summary>
        /// <returns>Result containing the built worksheet or error information</returns>
        Result<IWorksheetContent> Build();
    }
}