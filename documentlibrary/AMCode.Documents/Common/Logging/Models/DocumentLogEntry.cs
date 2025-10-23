using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AMCode.Documents.Common.Logging.Models
{
    /// <summary>
    /// Structured log entry for document operations
    /// </summary>
    public class DocumentLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public DocumentLogLevel Level { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string CorrelationId { get; set; }
        public string DocumentId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Operation { get; set; }
        public string Provider { get; set; }
        public string FilePath { get; set; }
        public long? FileSizeBytes { get; set; }
        public TimeSpan? Duration { get; set; }
        public Exception Exception { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public string Environment { get; set; }
        public string Application { get; set; } = "AMCode.Documents";
        public string Version { get; set; }
        
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions 
            { 
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
