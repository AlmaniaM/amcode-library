using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AMCode.Data.Logging.Models
{
    /// <summary>
    /// Structured log entry with searchable properties
    /// </summary>
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string CorrelationId { get; set; }
        public string Operation { get; set; }
        public string Method { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }
        public Exception Exception { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public TimeSpan? Duration { get; set; }
        public string Environment { get; set; }
        public string Application { get; set; } = "AMCode.Data";
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
