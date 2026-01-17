using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Domain.Objects;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Fluent API interface for building workbooks
    /// Provides a builder pattern for creating and configuring workbooks
    /// </summary>
    public interface IWorkbookBuilder
    {
        /// <summary>
        /// Sets the title of the workbook
        /// </summary>
        /// <param name="title">The workbook title</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithTitle(string title);

        /// <summary>
        /// Sets the author of the workbook
        /// </summary>
        /// <param name="author">The workbook author</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithAuthor(string author);

        /// <summary>
        /// Sets the subject of the workbook
        /// </summary>
        /// <param name="subject">The workbook subject</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithSubject(string subject);

        /// <summary>
        /// Sets the company associated with the workbook
        /// </summary>
        /// <param name="company">The company name</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCompany(string company);

        /// <summary>
        /// Sets the category of the workbook
        /// </summary>
        /// <param name="category">The workbook category</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCategory(string category);

        /// <summary>
        /// Sets the keywords for the workbook
        /// </summary>
        /// <param name="keywords">The workbook keywords</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithKeywords(string keywords);

        /// <summary>
        /// Sets the comments for the workbook
        /// </summary>
        /// <param name="comments">The workbook comments</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithComments(string comments);

        /// <summary>
        /// Sets the manager of the workbook
        /// </summary>
        /// <param name="manager">The workbook manager</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithManager(string manager);

        /// <summary>
        /// Sets the application that created the workbook
        /// </summary>
        /// <param name="application">The application name</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithApplication(string application);

        /// <summary>
        /// Sets the version of the application that created the workbook
        /// </summary>
        /// <param name="applicationVersion">The application version</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithApplicationVersion(string applicationVersion);

        /// <summary>
        /// Sets the template used for the workbook
        /// </summary>
        /// <param name="template">The template name</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithTemplate(string template);

        /// <summary>
        /// Sets the revision number of the workbook
        /// </summary>
        /// <param name="revision">The revision number</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithRevision(int revision);

        /// <summary>
        /// Sets the total editing time in minutes
        /// </summary>
        /// <param name="totalEditingTime">The total editing time in minutes</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithTotalEditingTime(int totalEditingTime);

        /// <summary>
        /// Sets the number of pages in the workbook
        /// </summary>
        /// <param name="pages">The number of pages</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPages(int pages);

        /// <summary>
        /// Sets the number of words in the workbook
        /// </summary>
        /// <param name="words">The number of words</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithWords(int words);

        /// <summary>
        /// Sets the number of characters in the workbook
        /// </summary>
        /// <param name="characters">The number of characters</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCharacters(int characters);

        /// <summary>
        /// Sets the number of characters with spaces in the workbook
        /// </summary>
        /// <param name="charactersWithSpaces">The number of characters with spaces</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCharactersWithSpaces(int charactersWithSpaces);

        /// <summary>
        /// Sets the number of lines in the workbook
        /// </summary>
        /// <param name="lines">The number of lines</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithLines(int lines);

        /// <summary>
        /// Sets the number of paragraphs in the workbook
        /// </summary>
        /// <param name="paragraphs">The number of paragraphs</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithParagraphs(int paragraphs);

        /// <summary>
        /// Sets the creation date of the workbook
        /// </summary>
        /// <param name="created">The creation date</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCreated(DateTime created);

        /// <summary>
        /// Sets the last modified date of the workbook
        /// </summary>
        /// <param name="modified">The last modified date</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithModified(DateTime modified);

        /// <summary>
        /// Sets the metadata for the workbook
        /// </summary>
        /// <param name="metadata">The workbook metadata</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithMetadata(WorkbookCreationMetadata metadata);

        /// <summary>
        /// Sets the custom properties for the workbook
        /// </summary>
        /// <param name="properties">The custom properties dictionary</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCustomProperties(Dictionary<string, object> properties);

        /// <summary>
        /// Sets a custom property for the workbook
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCustomProperty(string name, object value);

        /// <summary>
        /// Sets the protection password for the workbook
        /// </summary>
        /// <param name="password">The protection password</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithProtection(string password);

        /// <summary>
        /// Sets the protection options for the workbook
        /// </summary>
        /// <param name="options">The protection options</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithProtection(WorkbookProtectionOptions options);

        /// <summary>
        /// Sets the read-only state for the workbook
        /// </summary>
        /// <param name="readOnly">True to make read-only</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithReadOnly(bool readOnly);

        /// <summary>
        /// Sets the calculation mode for the workbook
        /// </summary>
        /// <param name="mode">The calculation mode</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCalculationMode(CalculationMode mode);

        /// <summary>
        /// Sets the calculation order for the workbook
        /// </summary>
        /// <param name="order">The calculation order</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCalculationOrder(CalculationOrder order);

        /// <summary>
        /// Sets the iteration settings for the workbook
        /// </summary>
        /// <param name="enabled">True to enable iteration</param>
        /// <param name="maxIterations">Maximum number of iterations</param>
        /// <param name="maxChange">Maximum change threshold</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithIteration(bool enabled, int maxIterations, double maxChange);

        /// <summary>
        /// Sets the precision as displayed for the workbook
        /// </summary>
        /// <param name="enabled">True to enable precision as displayed</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPrecisionAsDisplayed(bool enabled);

        /// <summary>
        /// Sets the date system for the workbook
        /// </summary>
        /// <param name="system">The date system</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDateSystem(DateSystem system);

        /// <summary>
        /// Sets the reference style for the workbook
        /// </summary>
        /// <param name="style">The reference style</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithReferenceStyle(ReferenceStyle style);

        /// <summary>
        /// Sets the default font for the workbook
        /// </summary>
        /// <param name="font">The default font settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultFont(FontSettings font);

        /// <summary>
        /// Sets the default number format for the workbook
        /// </summary>
        /// <param name="format">The number format string</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultNumberFormat(string format);

        /// <summary>
        /// Sets the default alignment for the workbook
        /// </summary>
        /// <param name="alignment">The default alignment settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultAlignment(AlignmentSettings alignment);

        /// <summary>
        /// Sets the default border for the workbook
        /// </summary>
        /// <param name="border">The default border settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultBorder(BorderSettings border);

        /// <summary>
        /// Sets the default fill for the workbook
        /// </summary>
        /// <param name="fill">The default fill settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultFill(FillSettings fill);

        /// <summary>
        /// Sets the default protection for the workbook
        /// </summary>
        /// <param name="protection">The default protection settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultProtection(ProtectionSettings protection);

        /// <summary>
        /// Sets the auto-save settings for the workbook
        /// </summary>
        /// <param name="enabled">True to enable auto-save</param>
        /// <param name="interval">The auto-save interval in minutes</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithAutoSave(bool enabled, int interval = 10);

        /// <summary>
        /// Sets the calculation settings for the workbook
        /// </summary>
        /// <param name="enabled">True to enable calculation</param>
        /// <param name="mode">The calculation mode</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCalculation(bool enabled, CalculationMode mode = CalculationMode.Automatic);

        /// <summary>
        /// Sets the display options for the workbook
        /// </summary>
        /// <param name="options">The display options</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDisplayOptions(WorkbookDisplayOptions options);

        /// <summary>
        /// Sets the print settings for the workbook
        /// </summary>
        /// <param name="settings">The print settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPrintSettings(WorkbookPrintSettings settings);

        /// <summary>
        /// Sets the page setup for the workbook
        /// </summary>
        /// <param name="pageSetup">The page setup settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPageSetup(WorkbookPageSetup pageSetup);

        /// <summary>
        /// Sets the header and footer for the workbook
        /// </summary>
        /// <param name="headerFooter">The header and footer settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithHeaderFooter(WorkbookHeaderFooter headerFooter);

        /// <summary>
        /// Sets the print options for the workbook
        /// </summary>
        /// <param name="printOptions">The print options</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPrintOptions(WorkbookPrintOptions printOptions);

        /// <summary>
        /// Sets the worksheets for the workbook
        /// </summary>
        /// <param name="worksheets">The worksheets to add</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithWorksheets(IEnumerable<IWorksheetBuilder> worksheets);

        /// <summary>
        /// Sets a worksheet for the workbook
        /// </summary>
        /// <param name="worksheet">The worksheet builder</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithWorksheet(IWorksheetBuilder worksheet);

        /// <summary>
        /// Sets the worksheet names for the workbook
        /// </summary>
        /// <param name="names">The worksheet names</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithWorksheetNames(IEnumerable<string> names);

        /// <summary>
        /// Sets the worksheet count for the workbook
        /// </summary>
        /// <param name="count">The number of worksheets to create</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithWorksheetCount(int count);

        /// <summary>
        /// Sets the default worksheet settings for the workbook
        /// </summary>
        /// <param name="settings">The default worksheet settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDefaultWorksheetSettings(DefaultWorksheetSettings settings);

        /// <summary>
        /// Sets the named ranges for the workbook
        /// </summary>
        /// <param name="namedRanges">The named ranges dictionary (name -> range)</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithNamedRanges(Dictionary<string, string> namedRanges);

        /// <summary>
        /// Sets a named range for the workbook
        /// </summary>
        /// <param name="name">The named range name</param>
        /// <param name="range">The range reference</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithNamedRange(string name, string range);

        /// <summary>
        /// Sets the data validation for the workbook
        /// </summary>
        /// <param name="validations">The data validation rules</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDataValidations(IEnumerable<DataValidationRule> validations);

        /// <summary>
        /// Sets the conditional formatting for the workbook
        /// </summary>
        /// <param name="formattingRules">The conditional formatting rules</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithConditionalFormattingRules(IEnumerable<ConditionalFormattingRule> formattingRules);

        /// <summary>
        /// Sets the sort order for the workbook
        /// </summary>
        /// <param name="sortOrders">The sort order configurations</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithSortOrders(IEnumerable<SortOrderConfiguration> sortOrders);

        /// <summary>
        /// Sets the grouping for the workbook
        /// </summary>
        /// <param name="groupings">The grouping configurations</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithGroupings(IEnumerable<GroupingConfiguration> groupings);

        /// <summary>
        /// Sets the outline for the workbook
        /// </summary>
        /// <param name="outline">The outline configuration</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithOutline(WorkbookOutlineConfiguration outline);

        /// <summary>
        /// Sets the calculation settings for the workbook
        /// </summary>
        /// <param name="calculationSettings">The calculation settings</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCalculationSettings(WorkbookCalculationSettings calculationSettings);

        /// <summary>
        /// Sets the drawing objects for the workbook
        /// </summary>
        /// <param name="drawingObjects">The drawing objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDrawingObjects(IEnumerable<DrawingObject> drawingObjects);

        /// <summary>
        /// Sets the charts for the workbook
        /// </summary>
        /// <param name="chartObjects">The chart objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithChartObjects(IEnumerable<ChartObject> chartObjects);

        /// <summary>
        /// Sets the pivot tables for the workbook
        /// </summary>
        /// <param name="pivotTableObjects">The pivot table objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPivotTableObjects(IEnumerable<PivotTableObject> pivotTableObjects);

        /// <summary>
        /// Sets the tables for the workbook
        /// </summary>
        /// <param name="tableObjects">The table objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithTableObjects(IEnumerable<TableObject> tableObjects);

        /// <summary>
        /// Sets the named ranges for the workbook
        /// </summary>
        /// <param name="namedRangeObjects">The named range objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithNamedRangeObjects(IEnumerable<NamedRangeObject> namedRangeObjects);

        /// <summary>
        /// Sets the data validation for the workbook
        /// </summary>
        /// <param name="dataValidationObjects">The data validation objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDataValidationObjects(IEnumerable<DataValidationObject> dataValidationObjects);

        /// <summary>
        /// Sets the conditional formatting for the workbook
        /// </summary>
        /// <param name="conditionalFormattingObjects">The conditional formatting objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithConditionalFormattingObjects(IEnumerable<ConditionalFormattingObject> conditionalFormattingObjects);

        /// <summary>
        /// Sets the sort order for the workbook
        /// </summary>
        /// <param name="sortOrderObjects">The sort order objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithSortOrderObjects(IEnumerable<SortOrderObject> sortOrderObjects);

        /// <summary>
        /// Sets the grouping for the workbook
        /// </summary>
        /// <param name="groupingObjects">The grouping objects</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithGroupingObjects(IEnumerable<GroupingObject> groupingObjects);

        /// <summary>
        /// Sets the outline for the workbook
        /// </summary>
        /// <param name="outlineObject">The outline object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithOutlineObject(WorkbookOutlineObject outlineObject);

        /// <summary>
        /// Sets the calculation settings for the workbook
        /// </summary>
        /// <param name="calculationSettingsObject">The calculation settings object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithCalculationSettingsObject(WorkbookCalculationSettingsObject calculationSettingsObject);

        /// <summary>
        /// Sets the display options for the workbook
        /// </summary>
        /// <param name="displayOptionsObject">The display options object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithDisplayOptionsObject(WorkbookDisplayOptionsObject displayOptionsObject);

        /// <summary>
        /// Sets the print settings for the workbook
        /// </summary>
        /// <param name="printSettingsObject">The print settings object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPrintSettingsObject(WorkbookPrintSettingsObject printSettingsObject);

        /// <summary>
        /// Sets the page setup for the workbook
        /// </summary>
        /// <param name="pageSetupObject">The page setup object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPageSetupObject(WorkbookPageSetupObject pageSetupObject);

        /// <summary>
        /// Sets the header and footer for the workbook
        /// </summary>
        /// <param name="headerFooterObject">The header and footer object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithHeaderFooterObject(WorkbookHeaderFooterObject headerFooterObject);

        /// <summary>
        /// Sets the print options for the workbook
        /// </summary>
        /// <param name="printOptionsObject">The print options object</param>
        /// <returns>The workbook builder for method chaining</returns>
        IWorkbookBuilder WithPrintOptionsObject(WorkbookPrintOptionsObject printOptionsObject);

        /// <summary>
        /// Builds the workbook with all configured settings
        /// </summary>
        /// <returns>Result containing the built workbook or error information</returns>
        Result<IWorkbookDomain> Build();
    }
}
