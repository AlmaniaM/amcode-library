using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using AMCode.Data.Logging.Configuration;
using AMCode.Data.Logging.Models;

namespace AMCode.Data.Logging.Infrastructure
{
    /// <summary>
    /// Base logger implementation with common functionality for AMCode.Data
    /// </summary>
    public abstract class BaseLogger : ILogger
    {
        protected readonly string _category;
        protected readonly ILoggerProvider _provider;
        protected readonly LoggingConfiguration _configuration;
        protected readonly IDictionary<string, object> _context;

        protected BaseLogger(string category, ILoggerProvider provider, LoggingConfiguration configuration)
        {
            _category = category ?? throw new ArgumentNullException(nameof(category));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = new Dictionary<string, object>();
        }

        public virtual void Log(LogLevel level, string message, Exception exception = null, object context = null)
        {
            if (!IsEnabled(level)) return;

            var logEntry = CreateLogEntry(level, message, exception, context);
            WriteLog(logEntry);
        }

        public virtual void Log(LogLevel level, string message, IDictionary<string, object> properties, Exception exception = null)
        {
            if (!IsEnabled(level)) return;

            var logEntry = CreateLogEntry(level, message, exception, null);
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    logEntry.Properties[property.Key] = property.Value;
                }
            }
            WriteLog(logEntry);
        }

        public virtual void LogInformation(string message, object context = null)
        {
            Log(LogLevel.Information, message, null, context);
        }

        public virtual void LogWarning(string message, object context = null)
        {
            Log(LogLevel.Warning, message, null, context);
        }

        public virtual void LogError(string message, Exception exception = null, object context = null)
        {
            Log(LogLevel.Error, message, exception, context);
        }

        public virtual void LogDebug(string message, object context = null)
        {
            Log(LogLevel.Debug, message, null, context);
        }

        public virtual void LogTrace(string message, object context = null)
        {
            Log(LogLevel.Trace, message, null, context);
        }

        public virtual void LogPerformance(string operation, TimeSpan duration, object context = null)
        {
            var message = $"Performance: {operation} completed in {duration.TotalMilliseconds}ms";
            var properties = new Dictionary<string, object>
            {
                ["Operation"] = operation,
                ["Duration"] = duration.TotalMilliseconds,
                ["DurationMs"] = duration.TotalMilliseconds,
                ["DurationTicks"] = duration.Ticks
            };
            Log(LogLevel.Information, message, properties, null);
        }

        public virtual ILogger WithContext(string key, object value)
        {
            var newContext = new Dictionary<string, object>(_context) { [key] = value };
            return CreateScopedLogger(newContext);
        }

        public virtual ILogger WithContext(IDictionary<string, object> context)
        {
            var newContext = new Dictionary<string, object>(_context);
            if (context != null)
            {
                foreach (var kvp in context)
                {
                    newContext[kvp.Key] = kvp.Value;
                }
            }
            return CreateScopedLogger(newContext);
        }

        protected virtual bool IsEnabled(LogLevel level)
        {
            return level >= _configuration.MinimumLevel;
        }

        protected virtual LogEntry CreateLogEntry(LogLevel level, string message, Exception exception, object context)
        {
            var logEntry = new LogEntry
            {
                Level = level,
                Message = message,
                Category = _category,
                Exception = exception,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                Application = "AMCode.Data",
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            };

            // Add context information
            foreach (var kvp in _context)
            {
                logEntry.Properties[kvp.Key] = kvp.Value;
            }

            // Add correlation ID if enabled
            if (_configuration.EnableCorrelationId)
            {
                logEntry.CorrelationId = GetCorrelationId();
            }

            // Add caller information
            var callerInfo = GetCallerInfo();
            logEntry.Method = callerInfo.Method;
            logEntry.File = callerInfo.File;
            logEntry.LineNumber = callerInfo.LineNumber;

            return logEntry;
        }

        protected abstract void WriteLog(LogEntry logEntry);
        protected abstract ILogger CreateScopedLogger(IDictionary<string, object> context);

        private string GetCorrelationId()
        {
            return Activity.Current?.Id ?? Guid.NewGuid().ToString();
        }

        private (string Method, string File, int LineNumber) GetCallerInfo()
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(3); // Adjust based on call stack depth
            return (
                frame?.GetMethod()?.Name ?? "Unknown",
                frame?.GetFileName() ?? "Unknown",
                frame?.GetFileLineNumber() ?? 0
            );
        }
    }
}
