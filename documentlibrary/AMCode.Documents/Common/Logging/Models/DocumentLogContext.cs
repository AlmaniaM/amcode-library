using System;
using System.Collections.Generic;

namespace AMCode.Documents.Common.Logging.Models
{
    /// <summary>
    /// Document-specific context information for logging
    /// </summary>
    public class DocumentLogContext
    {
        public string DocumentId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string OperationId { get; set; }
        public string OperationName { get; set; }
        public string Provider { get; set; }
        public string FilePath { get; set; }
        public long? FileSizeBytes { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string CorrelationId { get; set; }
        public IDictionary<string, object> CustomProperties { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Creates a copy of the context with additional properties
        /// </summary>
        public DocumentLogContext WithProperty(string key, object value)
        {
            var newContext = new DocumentLogContext
            {
                DocumentId = DocumentId,
                DocumentType = DocumentType,
                OperationId = OperationId,
                OperationName = OperationName,
                Provider = Provider,
                FilePath = FilePath,
                FileSizeBytes = FileSizeBytes,
                UserId = UserId,
                SessionId = SessionId,
                CorrelationId = CorrelationId,
                CustomProperties = new Dictionary<string, object>(CustomProperties)
            };
            
            newContext.CustomProperties[key] = value;
            return newContext;
        }
        
        /// <summary>
        /// Merges this context with another context
        /// </summary>
        public DocumentLogContext Merge(DocumentLogContext other)
        {
            if (other == null) return this;
            
            var merged = new DocumentLogContext
            {
                DocumentId = other.DocumentId ?? DocumentId,
                DocumentType = other.DocumentType != DocumentType.Unknown ? other.DocumentType : DocumentType,
                OperationId = other.OperationId ?? OperationId,
                OperationName = other.OperationName ?? OperationName,
                Provider = other.Provider ?? Provider,
                FilePath = other.FilePath ?? FilePath,
                FileSizeBytes = other.FileSizeBytes ?? FileSizeBytes,
                UserId = other.UserId ?? UserId,
                SessionId = other.SessionId ?? SessionId,
                CorrelationId = other.CorrelationId ?? CorrelationId,
                CustomProperties = new Dictionary<string, object>(CustomProperties)
            };
            
            foreach (var kvp in other.CustomProperties)
            {
                merged.CustomProperties[kvp.Key] = kvp.Value;
            }
            
            return merged;
        }
    }
}
