using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Structured logger configuration
    /// </summary>
    public class StructuredDocumentLoggerConfiguration
    {
        public bool EnableConsole { get; set; } = true;
        public bool EnableFile { get; set; } = true;
        public bool EnableExternal { get; set; } = false;
        public string Format { get; set; } = "json";
        public string ExternalEndpoint { get; set; }
        public Dictionary<string, string> ExternalHeaders { get; set; } = new();
        public int ExternalTimeoutSeconds { get; set; } = 30;
    }

    /// <summary>
    /// Structured logger implementation for document operations
    /// </summary>
    public class StructuredDocumentLogger : BaseDocumentLogger
    {
        private readonly StructuredDocumentLoggerConfiguration _structuredConfig;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public StructuredDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration, StructuredDocumentLoggerConfiguration structuredConfig)
            : base(category, provider, configuration)
        {
            _structuredConfig = structuredConfig ?? throw new ArgumentNullException(nameof(structuredConfig));
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(_structuredConfig.ExternalTimeoutSeconds);
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        protected override void WriteLog(DocumentLogEntry logEntry)
        {
            var structuredMessage = CreateStructuredMessage(logEntry);

            if (_structuredConfig.EnableConsole)
            {
                WriteToConsole(structuredMessage);
            }

            if (_structuredConfig.EnableFile)
            {
                WriteToFile(structuredMessage);
            }

            if (_structuredConfig.EnableExternal && !string.IsNullOrEmpty(_structuredConfig.ExternalEndpoint))
            {
                _ = Task.Run(() => WriteToExternalAsync(structuredMessage));
            }
        }

        protected override IDocumentLogger CreateScopedLogger(IDictionary<string, object> context)
        {
            var logger = new StructuredDocumentLogger(_category, _provider, _configuration, _structuredConfig);
            // Copy context to the new logger
            foreach (var kvp in context)
            {
                logger._context[kvp.Key] = kvp.Value;
            }
            return logger;
        }

        private string CreateStructuredMessage(DocumentLogEntry logEntry)
        {
            return _structuredConfig.Format.ToLower() switch
            {
                "json" => JsonSerializer.Serialize(logEntry, _jsonOptions),
                "xml" => CreateXmlMessage(logEntry),
                "csv" => CreateCsvMessage(logEntry),
                _ => JsonSerializer.Serialize(logEntry, _jsonOptions)
            };
        }

        private string CreateXmlMessage(DocumentLogEntry logEntry)
        {
            var xml = new StringBuilder();
            xml.AppendLine("<LogEntry>");
            xml.AppendLine($"  <Timestamp>{logEntry.Timestamp:yyyy-MM-ddTHH:mm:ss.fffZ}</Timestamp>");
            xml.AppendLine($"  <Level>{logEntry.Level}</Level>");
            xml.AppendLine($"  <Message>{EscapeXml(logEntry.Message)}</Message>");
            xml.AppendLine($"  <Category>{EscapeXml(logEntry.Category)}</Category>");
            
            if (!string.IsNullOrEmpty(logEntry.CorrelationId))
                xml.AppendLine($"  <CorrelationId>{EscapeXml(logEntry.CorrelationId)}</CorrelationId>");
            
            if (!string.IsNullOrEmpty(logEntry.DocumentId))
                xml.AppendLine($"  <DocumentId>{EscapeXml(logEntry.DocumentId)}</DocumentId>");
            
            if (logEntry.DocumentType != DocumentType.Unknown)
                xml.AppendLine($"  <DocumentType>{logEntry.DocumentType}</DocumentType>");
            
            if (!string.IsNullOrEmpty(logEntry.Operation))
                xml.AppendLine($"  <Operation>{EscapeXml(logEntry.Operation)}</Operation>");
            
            if (!string.IsNullOrEmpty(logEntry.Provider))
                xml.AppendLine($"  <Provider>{EscapeXml(logEntry.Provider)}</Provider>");
            
            if (!string.IsNullOrEmpty(logEntry.FilePath))
                xml.AppendLine($"  <FilePath>{EscapeXml(logEntry.FilePath)}</FilePath>");
            
            if (logEntry.FileSizeBytes.HasValue)
                xml.AppendLine($"  <FileSizeBytes>{logEntry.FileSizeBytes}</FileSizeBytes>");
            
            if (logEntry.Duration.HasValue)
                xml.AppendLine($"  <Duration>{logEntry.Duration.Value.TotalMilliseconds}</Duration>");
            
            if (logEntry.Exception != null)
            {
                xml.AppendLine("  <Exception>");
                xml.AppendLine($"    <Type>{EscapeXml(logEntry.Exception.GetType().Name)}</Type>");
                xml.AppendLine($"    <Message>{EscapeXml(logEntry.Exception.Message)}</Message>");
                xml.AppendLine($"    <StackTrace>{EscapeXml(logEntry.Exception.StackTrace)}</StackTrace>");
                xml.AppendLine("  </Exception>");
            }
            
            if (logEntry.Properties.Count > 0)
            {
                xml.AppendLine("  <Properties>");
                foreach (var prop in logEntry.Properties)
                {
                    xml.AppendLine($"    <Property name=\"{EscapeXml(prop.Key)}\">{EscapeXml(prop.Value?.ToString())}</Property>");
                }
                xml.AppendLine("  </Properties>");
            }
            
            xml.AppendLine("</LogEntry>");
            return xml.ToString();
        }

        private string CreateCsvMessage(DocumentLogEntry logEntry)
        {
            var csv = new StringBuilder();
            csv.Append($"\"{logEntry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}\",");
            csv.Append($"\"{logEntry.Level}\",");
            csv.Append($"\"{EscapeCsv(logEntry.Message)}\",");
            csv.Append($"\"{EscapeCsv(logEntry.Category)}\",");
            csv.Append($"\"{EscapeCsv(logEntry.CorrelationId ?? "")}\",");
            csv.Append($"\"{EscapeCsv(logEntry.DocumentId ?? "")}\",");
            csv.Append($"\"{logEntry.DocumentType}\",");
            csv.Append($"\"{EscapeCsv(logEntry.Operation ?? "")}\",");
            csv.Append($"\"{EscapeCsv(logEntry.Provider ?? "")}\",");
            csv.Append($"\"{EscapeCsv(logEntry.FilePath ?? "")}\",");
            csv.Append($"\"{logEntry.FileSizeBytes ?? 0}\",");
            csv.Append($"\"{logEntry.Duration?.TotalMilliseconds ?? 0}\"");
            
            if (logEntry.Exception != null)
            {
                csv.Append($",\"{EscapeCsv(logEntry.Exception.Message)}\"");
            }
            
            return csv.ToString();
        }

        private void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }

        private void WriteToFile(string message)
        {
            // For now, use a simple file write
            // In a production implementation, this would use the FileDocumentLogger
            try
            {
                var logPath = Path.Combine("logs", "structured.log");
                Directory.CreateDirectory("logs");
                File.AppendAllText(logPath, message + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to write structured log to file: {ex.Message}");
            }
        }

        private async Task WriteToExternalAsync(string message)
        {
            try
            {
                var content = new StringContent(message, Encoding.UTF8, "application/json");
                
                foreach (var header in _structuredConfig.ExternalHeaders)
                {
                    content.Headers.Add(header.Key, header.Value);
                }

                var response = await _httpClient.PostAsync(_structuredConfig.ExternalEndpoint, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[WARNING] External logging failed: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to send structured log to external endpoint: {ex.Message}");
            }
        }

        private string EscapeXml(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace("\"", "\"\"");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
