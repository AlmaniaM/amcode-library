using System;

namespace AMCode.Storage.Interfaces
{
    /// <summary>
    /// Core logging interface for storage operations.
    /// Provides standard logging methods for information, warnings, errors, debug, and trace messages.
    /// </summary>
    public interface IStorageLogger
    {
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to format into the message.</param>
        void LogInformation(string message, params object[] args);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to format into the message.</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to format into the message.</param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// Logs an error message with an exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to format into the message.</param>
        void LogError(Exception exception, string message, params object[] args);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to format into the message.</param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to format into the message.</param>
        void LogTrace(string message, params object[] args);
    }
}
