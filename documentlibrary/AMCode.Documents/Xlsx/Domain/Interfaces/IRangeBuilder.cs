using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Enums;
using AMCode.Documents.Xlsx.Domain.Interfaces;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Fluent API interface for building ranges
    /// Provides a builder pattern for creating and configuring cell ranges
    /// </summary>
    public interface IRangeBuilder
    {
        /// <summary>
        /// Sets the range address
        /// </summary>
        /// <param name="address">The range address (e.g., "A1:B5")</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAddress(string address);

        /// <summary>
        /// Sets the range address using start and end cells
        /// </summary>
        /// <param name="startCell">The start cell reference (e.g., "A1")</param>
        /// <param name="endCell">The end cell reference (e.g., "B5")</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAddress(string startCell, string endCell);

        /// <summary>
        /// Sets the range address using row and column indices
        /// </summary>
        /// <param name="startRow">The start row index (0-based)</param>
        /// <param name="startColumn">The start column index (0-based)</param>
        /// <param name="endRow">The end row index (0-based)</param>
        /// <param name="endColumn">The end column index (0-based)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAddress(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Sets the worksheet for the range
        /// </summary>
        /// <param name="worksheet">The worksheet containing the range</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithWorksheet(IWorksheet worksheet);

        /// <summary>
        /// Sets the value for the entire range
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithValue(object value);

        /// <summary>
        /// Sets the values for the range from a 2D array
        /// </summary>
        /// <param name="values">The 2D array of values</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithValues(object[,] values);

        /// <summary>
        /// Sets the values for the range from a list of objects
        /// </summary>
        /// <typeparam name="T">The type of objects in the list</typeparam>
        /// <param name="values">The list of values</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithValues<T>(IEnumerable<T> values);

        /// <summary>
        /// Sets the values for the range from a DataTable
        /// </summary>
        /// <param name="dataTable">The DataTable</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithValues(System.Data.DataTable dataTable);

        /// <summary>
        /// Sets the formula for the entire range
        /// </summary>
        /// <param name="formula">The formula to set</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFormula(string formula);

        /// <summary>
        /// Sets the formulas for the range from a 2D array
        /// </summary>
        /// <param name="formulas">The 2D array of formulas</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFormulas(string[,] formulas);

        /// <summary>
        /// Sets the formulas for the range from a list of strings
        /// </summary>
        /// <param name="formulas">The list of formulas</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFormulas(IEnumerable<string> formulas);

        /// <summary>
        /// Sets the formulas for the range from a dictionary
        /// </summary>
        /// <param name="formulas">The formulas dictionary (cell reference -> formula)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFormulas(Dictionary<string, string> formulas);

        /// <summary>
        /// Sets the number format for the entire range
        /// </summary>
        /// <param name="format">The number format string</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithNumberFormat(string format);

        /// <summary>
        /// Sets the number formats for the range from a 2D array
        /// </summary>
        /// <param name="formats">The 2D array of number formats</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithNumberFormats(string[,] formats);

        /// <summary>
        /// Sets the number formats for the range from a list of strings
        /// </summary>
        /// <param name="formats">The list of number formats</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithNumberFormats(IEnumerable<string> formats);

        /// <summary>
        /// Sets the number formats for the range from a dictionary
        /// </summary>
        /// <param name="formats">The number formats dictionary (cell reference -> format)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithNumberFormats(Dictionary<string, string> formats);

        /// <summary>
        /// Sets the font for the entire range
        /// </summary>
        /// <param name="font">The font settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFont(FontSettings font);

        /// <summary>
        /// Sets the font name for the entire range
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFontName(string fontName);

        /// <summary>
        /// Sets the font size for the entire range
        /// </summary>
        /// <param name="fontSize">The font size</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFontSize(double fontSize);

        /// <summary>
        /// Sets the font style for the entire range
        /// </summary>
        /// <param name="bold">True for bold</param>
        /// <param name="italic">True for italic</param>
        /// <param name="underline">True for underline</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFontStyle(bool bold = false, bool italic = false, bool underline = false);

        /// <summary>
        /// Sets the font color for the entire range
        /// </summary>
        /// <param name="color">The font color (hex format, e.g., "FF0000")</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFontColor(string color);

        /// <summary>
        /// Sets the font color for the entire range using RGB values
        /// </summary>
        /// <param name="red">Red component (0-255)</param>
        /// <param name="green">Green component (0-255)</param>
        /// <param name="blue">Blue component (0-255)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFontColor(int red, int green, int blue);

        /// <summary>
        /// Sets the alignment for the entire range
        /// </summary>
        /// <param name="alignment">The alignment settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAlignment(AlignmentSettings alignment);

        /// <summary>
        /// Sets the horizontal alignment for the entire range
        /// </summary>
        /// <param name="horizontalAlignment">The horizontal alignment</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithHorizontalAlignment(HorizontalAlignment horizontalAlignment);

        /// <summary>
        /// Sets the vertical alignment for the entire range
        /// </summary>
        /// <param name="verticalAlignment">The vertical alignment</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithVerticalAlignment(VerticalAlignment verticalAlignment);

        /// <summary>
        /// Sets the text wrapping for the entire range
        /// </summary>
        /// <param name="wrapText">True to wrap text</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithTextWrapping(bool wrapText);

        /// <summary>
        /// Sets the text orientation for the entire range
        /// </summary>
        /// <param name="orientation">The text orientation in degrees</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithTextOrientation(int orientation);

        /// <summary>
        /// Sets the text indentation for the entire range
        /// </summary>
        /// <param name="indent">The text indentation level</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithTextIndent(int indent);

        /// <summary>
        /// Sets the text shrinking for the entire range
        /// </summary>
        /// <param name="shrinkToFit">True to shrink text to fit</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithTextShrinking(bool shrinkToFit);

        /// <summary>
        /// Sets the border for the entire range
        /// </summary>
        /// <param name="border">The border settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithBorder(BorderSettings border);

        /// <summary>
        /// Sets the border style for the entire range
        /// </summary>
        /// <param name="style">The border style</param>
        /// <param name="weight">The border weight</param>
        /// <param name="color">The border color (hex format)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithBorderStyle(BorderStyle style, BorderWeight weight, string color);

        /// <summary>
        /// Sets the border for specific sides of the range
        /// </summary>
        /// <param name="sides">The border sides</param>
        /// <param name="style">The border style</param>
        /// <param name="weight">The border weight</param>
        /// <param name="color">The border color (hex format)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithBorderSides(BorderSides sides, BorderStyle style, BorderWeight weight, string color);

        /// <summary>
        /// Sets the fill for the entire range
        /// </summary>
        /// <param name="fill">The fill settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFill(FillSettings fill);

        /// <summary>
        /// Sets the background color for the entire range
        /// </summary>
        /// <param name="color">The background color (hex format, e.g., "FF0000")</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithBackgroundColor(string color);

        /// <summary>
        /// Sets the background color for the entire range using RGB values
        /// </summary>
        /// <param name="red">Red component (0-255)</param>
        /// <param name="green">Green component (0-255)</param>
        /// <param name="blue">Blue component (0-255)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithBackgroundColor(int red, int green, int blue);

        /// <summary>
        /// Sets the pattern fill for the entire range
        /// </summary>
        /// <param name="pattern">The pattern type</param>
        /// <param name="foregroundColor">The foreground color (hex format)</param>
        /// <param name="backgroundColor">The background color (hex format)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithPatternFill(PatternType pattern, string foregroundColor, string backgroundColor);

        /// <summary>
        /// Sets the gradient fill for the entire range
        /// </summary>
        /// <param name="gradientType">The gradient type</param>
        /// <param name="color1">The first color (hex format)</param>
        /// <param name="color2">The second color (hex format)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithGradientFill(GradientType gradientType, string color1, string color2);

        /// <summary>
        /// Sets the protection for the entire range
        /// </summary>
        /// <param name="protection">The protection settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithProtection(ProtectionSettings protection);

        /// <summary>
        /// Sets the locked state for the entire range
        /// </summary>
        /// <param name="locked">True to lock the range</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithLocked(bool locked);

        /// <summary>
        /// Sets the hidden state for the entire range
        /// </summary>
        /// <param name="hidden">True to hide the range</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithHidden(bool hidden);

        /// <summary>
        /// Sets the formula hidden state for the entire range
        /// </summary>
        /// <param name="formulaHidden">True to hide formulas</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFormulaHidden(bool formulaHidden);

        /// <summary>
        /// Sets the data validation for the range
        /// </summary>
        /// <param name="validation">The data validation rule</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithDataValidation(DataValidationRule validation);

        /// <summary>
        /// Sets the conditional formatting for the range
        /// </summary>
        /// <param name="formatting">The conditional formatting rule</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithConditionalFormatting(ConditionalFormattingRule formatting);

        /// <summary>
        /// Sets the comment for the range
        /// </summary>
        /// <param name="comment">The comment text</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithComment(string comment);

        /// <summary>
        /// Sets the hyperlink for the range
        /// </summary>
        /// <param name="url">The hyperlink URL</param>
        /// <param name="displayText">The display text (optional)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithHyperlink(string url, string displayText = null);

        /// <summary>
        /// Sets the named range for the range
        /// </summary>
        /// <param name="name">The named range name</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithNamedRange(string name);

        /// <summary>
        /// Sets the merge state for the range
        /// </summary>
        /// <param name="merged">True to merge the range</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithMerged(bool merged);

        /// <summary>
        /// Sets the auto-fit for the range
        /// </summary>
        /// <param name="autoFit">True to auto-fit the range</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAutoFit(bool autoFit);

        /// <summary>
        /// Sets the row height for the range
        /// </summary>
        /// <param name="height">The row height in points</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithRowHeight(double height);

        /// <summary>
        /// Sets the column width for the range
        /// </summary>
        /// <param name="width">The column width in characters</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithColumnWidth(double width);

        /// <summary>
        /// Sets the row height for specific rows in the range
        /// </summary>
        /// <param name="heights">The row heights dictionary (row index -> height)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithRowHeights(Dictionary<int, double> heights);

        /// <summary>
        /// Sets the column width for specific columns in the range
        /// </summary>
        /// <param name="widths">The column widths dictionary (column index -> width)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithColumnWidths(Dictionary<int, double> widths);

        /// <summary>
        /// Sets the auto-fit for rows in the range
        /// </summary>
        /// <param name="autoFitRows">True to auto-fit rows</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAutoFitRows(bool autoFitRows);

        /// <summary>
        /// Sets the auto-fit for columns in the range
        /// </summary>
        /// <param name="autoFitColumns">True to auto-fit columns</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithAutoFitColumns(bool autoFitColumns);

        /// <summary>
        /// Sets the sort order for the range
        /// </summary>
        /// <param name="sortOrder">The sort order configuration</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithSortOrder(SortOrderConfiguration sortOrder);

        /// <summary>
        /// Sets the filter for the range
        /// </summary>
        /// <param name="filter">True to enable auto-filter</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithFilter(bool filter);

        /// <summary>
        /// Sets the grouping for the range
        /// </summary>
        /// <param name="grouping">The grouping configuration</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithGrouping(GroupingConfiguration grouping);

        /// <summary>
        /// Sets the outline level for the range
        /// </summary>
        /// <param name="level">The outline level</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithOutlineLevel(int level);

        /// <summary>
        /// Sets the summary row position for the range
        /// </summary>
        /// <param name="position">The summary row position</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithSummaryRowPosition(SummaryRowPosition position);

        /// <summary>
        /// Sets the summary column position for the range
        /// </summary>
        /// <param name="position">The summary column position</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithSummaryColumnPosition(SummaryColumnPosition position);

        /// <summary>
        /// Sets the calculation mode for the range
        /// </summary>
        /// <param name="mode">The calculation mode</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithCalculationMode(CalculationMode mode);

        /// <summary>
        /// Sets the calculation order for the range
        /// </summary>
        /// <param name="order">The calculation order</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithCalculationOrder(CalculationOrder order);

        /// <summary>
        /// Sets the iteration settings for the range
        /// </summary>
        /// <param name="enabled">True to enable iteration</param>
        /// <param name="maxIterations">Maximum number of iterations</param>
        /// <param name="maxChange">Maximum change threshold</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithIteration(bool enabled, int maxIterations, double maxChange);

        /// <summary>
        /// Sets the precision as displayed for the range
        /// </summary>
        /// <param name="enabled">True to enable precision as displayed</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithPrecisionAsDisplayed(bool enabled);

        /// <summary>
        /// Sets the date system for the range
        /// </summary>
        /// <param name="system">The date system</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithDateSystem(DateSystem system);

        /// <summary>
        /// Sets the reference style for the range
        /// </summary>
        /// <param name="style">The reference style</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithReferenceStyle(ReferenceStyle style);

        /// <summary>
        /// Sets the custom properties for the range
        /// </summary>
        /// <param name="properties">The custom properties dictionary</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithCustomProperties(Dictionary<string, object> properties);

        /// <summary>
        /// Sets a custom property for the range
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithCustomProperty(string name, object value);

        /// <summary>
        /// Sets the comments for the range
        /// </summary>
        /// <param name="comments">The comments dictionary (cell reference -> comment text)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithComments(Dictionary<string, string> comments);

        /// <summary>
        /// Sets the hyperlinks for the range
        /// </summary>
        /// <param name="hyperlinks">The hyperlinks dictionary (cell reference -> URL)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithHyperlinks(Dictionary<string, string> hyperlinks);

        /// <summary>
        /// Sets the named ranges for the range
        /// </summary>
        /// <param name="namedRanges">The named ranges dictionary (name -> range)</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithNamedRanges(Dictionary<string, string> namedRanges);

        /// <summary>
        /// Sets the data validation for the range
        /// </summary>
        /// <param name="validations">The data validation rules</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithDataValidations(IEnumerable<DataValidationRule> validations);

        /// <summary>
        /// Sets the conditional formatting for the range
        /// </summary>
        /// <param name="formattingRules">The conditional formatting rules</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithConditionalFormattingRules(IEnumerable<ConditionalFormattingRule> formattingRules);

        /// <summary>
        /// Sets the sort order for the range
        /// </summary>
        /// <param name="sortOrders">The sort order configurations</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithSortOrders(IEnumerable<SortOrderConfiguration> sortOrders);

        /// <summary>
        /// Sets the grouping for the range
        /// </summary>
        /// <param name="groupings">The grouping configurations</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithGroupings(IEnumerable<GroupingConfiguration> groupings);

        /// <summary>
        /// Sets the outline for the range
        /// </summary>
        /// <param name="outline">The outline configuration</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithOutline(OutlineConfiguration outline);

        /// <summary>
        /// Sets the calculation settings for the range
        /// </summary>
        /// <param name="calculationSettings">The calculation settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithCalculationSettings(CalculationSettings calculationSettings);

        /// <summary>
        /// Sets the display options for the range
        /// </summary>
        /// <param name="displayOptions">The display options</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithDisplayOptions(DisplayOptions displayOptions);

        /// <summary>
        /// Sets the print settings for the range
        /// </summary>
        /// <param name="printSettings">The print settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithPrintSettings(PrintSettings printSettings);

        /// <summary>
        /// Sets the page setup for the range
        /// </summary>
        /// <param name="pageSetup">The page setup settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithPageSetup(PageSetup pageSetup);

        /// <summary>
        /// Sets the header and footer for the range
        /// </summary>
        /// <param name="headerFooter">The header and footer settings</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithHeaderFooter(HeaderFooter headerFooter);

        /// <summary>
        /// Sets the print options for the range
        /// </summary>
        /// <param name="printOptions">The print options</param>
        /// <returns>The range builder for method chaining</returns>
        IRangeBuilder WithPrintOptions(PrintOptions printOptions);

        /// <summary>
        /// Builds the range with all configured settings
        /// </summary>
        /// <returns>Result containing the built range or error information</returns>
        Result<IRange> Build();
    }
}
