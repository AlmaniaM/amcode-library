using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Console logger configuration
    /// </summary>
    public class ConsoleDocumentLoggerConfiguration
    {
        public bool EnableColor { get; set; } = true;
        public bool EnableTimestamp { get; set; } = true;
        public bool EnableCategory { get; set; } = true;
        public bool EnableDocumentType { get; set; } = true;
        public bool EnableCorrelationId { get; set; } = true;
    }

    /// <summary>
    /// Console logger implementation for document operations
    /// </summary>
    public class ConsoleDocumentLogger : BaseDocumentLogger
    {
        private readonly ConsoleDocumentLoggerConfiguration _consoleConfig;

        public ConsoleDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration, ConsoleDocumentLoggerConfiguration consoleConfig)
            : base(category, provider, configuration)
        {
            _consoleConfig = consoleConfig ?? throw new ArgumentNullException(nameof(consoleConfig));
        }

        protected override void WriteLog(DocumentLogEntry logEntry)
        {
            if (!_configuration.EnableConsole) return;

            var originalColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = GetColorForLevel(logEntry.Level);
                
                if (_configuration.EnableStructuredLogging)
                {
                    Console.WriteLine(logEntry.ToJson());
                }
                else
                {
                    var formattedMessage = FormatDocumentLogMessage(logEntry);
                    Console.WriteLine(formattedMessage);
                }
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        protected override IDocumentLogger CreateScopedLogger(IDictionary<string, object> context)
        {
            var logger = new ConsoleDocumentLogger(_category, _provider, _configuration, _consoleConfig);
            // Copy context to the new logger
            foreach (var kvp in context)
            {
                logger._context[kvp.Key] = kvp.Value;
            }
            return logger;
        }

        private ConsoleColor GetColorForLevel(DocumentLogLevel level)
        {
            return level switch
            {
                DocumentLogLevel.Trace => ConsoleColor.Gray,
                DocumentLogLevel.Debug => ConsoleColor.Cyan,
                DocumentLogLevel.Information => ConsoleColor.White,
                DocumentLogLevel.Warning => ConsoleColor.Yellow,
                DocumentLogLevel.Error => ConsoleColor.Red,
                DocumentLogLevel.Critical => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };
        }

        private string FormatDocumentLogMessage(DocumentLogEntry logEntry)
        {
            var timestamp = logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var level = logEntry.Level.ToString().ToUpper().PadRight(5);
            var category = $"[{logEntry.Category}]";
            var documentType = logEntry.DocumentType != DocumentType.Unknown ? $" [{logEntry.DocumentType}]" : "";
            var correlationId = string.IsNullOrEmpty(logEntry.CorrelationId) ? "" : $" [{logEntry.CorrelationId}]";
            
            return $"{timestamp} {level} {category}{documentType}{correlationId}: {logEntry.Message}";
        }
    }
}
