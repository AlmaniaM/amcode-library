using System;
using System.Collections.Generic;
using AMCode.Data.Logging.Configuration;
using AMCode.Data.Logging.Models;

namespace AMCode.Data.Logging.Infrastructure.Console
{
    /// <summary>
    /// Console logger implementation with color-coded output for AMCode.Data
    /// </summary>
    public class ConsoleLogger : BaseLogger
    {
        private readonly ConsoleLoggerConfiguration _consoleConfig;

        public ConsoleLogger(string category, ILoggerProvider provider, LoggingConfiguration configuration, ConsoleLoggerConfiguration consoleConfig)
            : base(category, provider, configuration)
        {
            _consoleConfig = consoleConfig ?? throw new ArgumentNullException(nameof(consoleConfig));
        }

        protected override void WriteLog(LogEntry logEntry)
        {
            if (!_configuration.EnableConsole) return;

            var originalColor = System.Console.ForegroundColor;
            try
            {
                System.Console.ForegroundColor = GetColorForLevel(logEntry.Level);
                
                if (_configuration.EnableStructuredLogging)
                {
                    System.Console.WriteLine(logEntry.ToJson());
                }
                else
                {
                    var formattedMessage = FormatLogMessage(logEntry);
                    System.Console.WriteLine(formattedMessage);
                }
            }
            finally
            {
                System.Console.ForegroundColor = originalColor;
            }
        }

        protected override ILogger CreateScopedLogger(IDictionary<string, object> context)
        {
            return new ConsoleLogger(_category, _provider, _configuration, _consoleConfig)
            {
                _context = new Dictionary<string, object>(context)
            };
        }

        private System.ConsoleColor GetColorForLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => System.ConsoleColor.Gray,
                LogLevel.Debug => System.ConsoleColor.Cyan,
                LogLevel.Information => System.ConsoleColor.White,
                LogLevel.Warning => System.ConsoleColor.Yellow,
                LogLevel.Error => System.ConsoleColor.Red,
                LogLevel.Critical => System.ConsoleColor.Magenta,
                _ => System.ConsoleColor.White
            };
        }

        private string FormatLogMessage(LogEntry logEntry)
        {
            var timestamp = logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var level = logEntry.Level.ToString().ToUpper().PadRight(5);
            var category = $"[{logEntry.Category}]";
            var correlationId = string.IsNullOrEmpty(logEntry.CorrelationId) ? "" : $" [{logEntry.CorrelationId}]";
            
            return $"{timestamp} {level} {category}{correlationId}: {logEntry.Message}";
        }
    }
}
