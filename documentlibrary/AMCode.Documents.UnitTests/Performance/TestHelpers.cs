using System;
using System.Collections.Generic;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Pdf.Domain.Interfaces;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Test implementation of IWorkbookLogger for performance testing
    /// </summary>
    public class TestWorkbookLogger : IWorkbookLogger
    {
        public List<string> LogMessages { get; } = new List<string>();
        public List<Exception> LoggedExceptions { get; } = new List<Exception>();

        public void LogWorkbookOperation(string operation, TimeSpan duration)
        {
            LogMessages.Add($"Operation: {operation}, Duration: {duration}");
        }

        public void LogError(string message, Exception exception = null)
        {
            LogMessages.Add($"Error: {message}");
            if (exception != null)
                LoggedExceptions.Add(exception);
        }

        public void LogPerformance(string operation, long memoryUsed, TimeSpan duration)
        {
            LogMessages.Add($"Performance - {operation}: Memory: {memoryUsed} bytes, Duration: {duration}");
        }

        public void LogWarning(string message)
        {
            LogMessages.Add($"Warning: {message}");
        }
    }

    /// <summary>
    /// Test implementation of IWorkbookValidator for performance testing
    /// </summary>
    public class TestWorkbookValidator : IWorkbookValidator
    {
        public bool ValidateWorkbook(IWorkbook workbook)
        {
            return workbook != null;
        }

        public bool ValidateWorksheet(IWorksheet worksheet)
        {
            return worksheet != null;
        }

        public bool ValidateRange(IRange range)
        {
            return range != null;
        }

        public bool ValidateCellReference(string cellReference)
        {
            return !string.IsNullOrEmpty(cellReference);
        }
    }

    /// <summary>
    /// Test implementation of IPdfLogger for performance testing
    /// </summary>
    public class TestPdfLogger : IPdfLogger
    {
        public List<string> LogMessages { get; } = new List<string>();
        public List<Exception> LoggedExceptions { get; } = new List<Exception>();

        public void LogPdfOperation(string operation, TimeSpan duration)
        {
            LogMessages.Add($"PDF Operation: {operation}, Duration: {duration}");
        }

        public void LogError(string message, Exception exception = null)
        {
            LogMessages.Add($"PDF Error: {message}");
            if (exception != null)
                LoggedExceptions.Add(exception);
        }

        public void LogPerformance(string operation, long memoryUsed, TimeSpan duration)
        {
            LogMessages.Add($"PDF Performance - {operation}: Memory: {memoryUsed} bytes, Duration: {duration}");
        }

        public void LogWarning(string message)
        {
            LogMessages.Add($"PDF Warning: {message}");
        }
    }
}
