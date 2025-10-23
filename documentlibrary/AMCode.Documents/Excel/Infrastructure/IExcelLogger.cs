using System;
using AMCode.Documents.Common.Logging;

namespace AMCode.Documents.Excel.Infrastructure
{
    /// <summary>
    /// Excel-specific logging interface
    /// </summary>
    public interface IExcelLogger : IDocumentLogger
    {
        /// <summary>
        /// Log workbook operations
        /// </summary>
        void LogWorkbookOperation(string operation, object context = null);
        
        /// <summary>
        /// Log worksheet operations
        /// </summary>
        void LogWorksheetOperation(string operation, string worksheetName, object context = null);
        
        /// <summary>
        /// Log cell operations
        /// </summary>
        void LogCellOperation(string operation, string cellAddress, object context = null);
        
        /// <summary>
        /// Log range operations
        /// </summary>
        void LogRangeOperation(string operation, string rangeAddress, object context = null);
        
        /// <summary>
        /// Log styling operations
        /// </summary>
        void LogStylingOperation(string operation, object styleContext = null);
        
        /// <summary>
        /// Log Syncfusion-specific operations
        /// </summary>
        void LogSyncfusionOperation(string operation, object context = null);
        
        /// <summary>
        /// Log OpenXML-specific operations
        /// </summary>
        void LogOpenXmlOperation(string operation, object context = null);
    }
}
