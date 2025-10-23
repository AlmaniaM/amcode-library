using AMCode.Documents.Pdf;
using System;
using System.Collections.Generic;

namespace AMCode.Documents.UnitTests.Pdf
{
    /// <summary>
    /// Test implementation of IPdfLogger for unit testing
    /// </summary>
    public class TestPdfLogger : IPdfLogger
    {
        private readonly List<string> _logMessages = new List<string>();
        private readonly List<Exception> _exceptions = new List<Exception>();

        public IReadOnlyList<string> LogMessages => _logMessages.AsReadOnly();
        public IReadOnlyList<Exception> Exceptions => _exceptions.AsReadOnly();

        public void LogDocumentOperation(string operation, object context = null)
        {
            var message = $"Operation: {operation}";
            if (context != null)
            {
                message += $", Context: {context}";
            }
            _logMessages.Add(message);
        }

        public void LogError(string operation, Exception exception)
        {
            var message = $"Error in {operation}: {exception.Message}";
            _logMessages.Add(message);
            _exceptions.Add(exception);
        }

        public void LogInfo(string message)
        {
            _logMessages.Add($"INFO: {message}");
        }

        public void LogWarning(string message, object context = null)
        {
            var logMessage = $"WARNING: {message}";
            if (context != null)
            {
                logMessage += $", Context: {context}";
            }
            _logMessages.Add(logMessage);
        }

        public void LogInformation(string message, object context = null)
        {
            var logMessage = $"INFO: {message}";
            if (context != null)
            {
                logMessage += $", Context: {context}";
            }
            _logMessages.Add(logMessage);
        }

        public void LogDebug(string message, object context = null)
        {
            var logMessage = $"DEBUG: {message}";
            if (context != null)
            {
                logMessage += $", Context: {context}";
            }
            _logMessages.Add(logMessage);
        }

        public void Clear()
        {
            _logMessages.Clear();
            _exceptions.Clear();
        }
    }
}
