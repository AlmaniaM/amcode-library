using System;
using System.Collections.Generic;

namespace AMCode.Data.Logging
{
    /// <summary>
    /// Core logging interface for AMCode.Data library
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message with specified level
        /// </summary>
        void Log(LogLevel level, string message, Exception exception = null, object context = null);
        
        /// <summary>
        /// Logs a message with structured data
        /// </summary>
        void Log(LogLevel level, string message, IDictionary<string, object> properties, Exception exception = null);
        
        /// <summary>
        /// Logs an information message
        /// </summary>
        void LogInformation(string message, object context = null);
        
        /// <summary>
        /// Logs a warning message
        /// </summary>
        void LogWarning(string message, object context = null);
        
        /// <summary>
        /// Logs an error message
        /// </summary>
        void LogError(string message, Exception exception = null, object context = null);
        
        /// <summary>
        /// Logs a debug message
        /// </summary>
        void LogDebug(string message, object context = null);
        
        /// <summary>
        /// Logs a trace message
        /// </summary>
        void LogTrace(string message, object context = null);
        
        /// <summary>
        /// Logs performance metrics
        /// </summary>
        void LogPerformance(string operation, TimeSpan duration, object context = null);
        
        /// <summary>
        /// Creates a scoped logger with context
        /// </summary>
        ILogger WithContext(string key, object value);
        
        /// <summary>
        /// Creates a scoped logger with multiple context properties
        /// </summary>
        ILogger WithContext(IDictionary<string, object> context);
    }
}
