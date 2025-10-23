using AMCode.Storage.Interfaces;
using System.Collections.Generic;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Mock implementation of IStorageLogger for unit testing.
    /// Captures all log messages for verification in tests.
    /// </summary>
    public class MockStorageLogger : IStorageLogger
    {
        /// <summary>
        /// Gets the list of information log messages.
        /// </summary>
        public List<string> InformationLogs { get; } = new List<string>();

        /// <summary>
        /// Gets the list of warning log messages.
        /// </summary>
        public List<string> WarningLogs { get; } = new List<string>();

        /// <summary>
        /// Gets the list of error log messages.
        /// </summary>
        public List<string> ErrorLogs { get; } = new List<string>();

        /// <summary>
        /// Gets the list of debug log messages.
        /// </summary>
        public List<string> DebugLogs { get; } = new List<string>();

        /// <summary>
        /// Gets the list of trace log messages.
        /// </summary>
        public List<string> TraceLogs { get; } = new List<string>();

        /// <summary>
        /// Gets the list of exceptions logged with error messages.
        /// </summary>
        public List<Exception> LoggedExceptions { get; } = new List<Exception>();

        /// <inheritdoc />
        public void LogInformation(string message, params object[] args)
        {
            InformationLogs.Add(string.Format(message, args));
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] args)
        {
            WarningLogs.Add(string.Format(message, args));
        }

        /// <inheritdoc />
        public void LogError(string message, params object[] args)
        {
            ErrorLogs.Add(string.Format(message, args));
        }

        /// <inheritdoc />
        public void LogError(Exception exception, string message, params object[] args)
        {
            ErrorLogs.Add($"{string.Format(message, args)} | Exception: {exception.Message}");
            LoggedExceptions.Add(exception);
        }

        /// <inheritdoc />
        public void LogDebug(string message, params object[] args)
        {
            DebugLogs.Add(string.Format(message, args));
        }

        /// <inheritdoc />
        public void LogTrace(string message, params object[] args)
        {
            TraceLogs.Add(string.Format(message, args));
        }

        /// <summary>
        /// Clears all captured log messages and exceptions.
        /// </summary>
        public void Clear()
        {
            InformationLogs.Clear();
            WarningLogs.Clear();
            ErrorLogs.Clear();
            DebugLogs.Clear();
            TraceLogs.Clear();
            LoggedExceptions.Clear();
        }

        /// <summary>
        /// Gets the total count of all log messages.
        /// </summary>
        public int TotalLogCount => InformationLogs.Count + WarningLogs.Count + ErrorLogs.Count + DebugLogs.Count + TraceLogs.Count;
    }
}
