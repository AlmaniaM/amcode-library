using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Formats document log entries for different output formats
    /// </summary>
    public class DocumentLogFormatter
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public DocumentLogFormatter()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// Formats a log entry as JSON
        /// </summary>
        public string FormatAsJson(DocumentLogEntry logEntry)
        {
            return JsonSerializer.Serialize(logEntry, _jsonOptions);
        }

        /// <summary>
        /// Formats a log entry as XML
        /// </summary>
        public string FormatAsXml(DocumentLogEntry logEntry)
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

        /// <summary>
        /// Formats a log entry as CSV
        /// </summary>
        public string FormatAsCsv(DocumentLogEntry logEntry)
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

        /// <summary>
        /// Formats a log entry as plain text
        /// </summary>
        public string FormatAsText(DocumentLogEntry logEntry)
        {
            var timestamp = logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var level = logEntry.Level.ToString().ToUpper().PadRight(5);
            var category = $"[{logEntry.Category}]";
            var documentType = logEntry.DocumentType != DocumentType.Unknown ? $" [{logEntry.DocumentType}]" : "";
            var correlationId = string.IsNullOrEmpty(logEntry.CorrelationId) ? "" : $" [{logEntry.CorrelationId}]";
            
            return $"{timestamp} {level} {category}{documentType}{correlationId}: {logEntry.Message}";
        }

        /// <summary>
        /// Formats a log entry using the specified format
        /// </summary>
        public string Format(DocumentLogEntry logEntry, string format = "text")
        {
            return format.ToLower() switch
            {
                "json" => FormatAsJson(logEntry),
                "xml" => FormatAsXml(logEntry),
                "csv" => FormatAsCsv(logEntry),
                "text" => FormatAsText(logEntry),
                _ => FormatAsText(logEntry)
            };
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
    }
}
