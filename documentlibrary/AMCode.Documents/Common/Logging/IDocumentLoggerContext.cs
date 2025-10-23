using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging
{
    /// <summary>
    /// Document logger context interface
    /// </summary>
    public interface IDocumentLoggerContext
    {
        string DocumentId { get; set; }
        DocumentType DocumentType { get; set; }
        string OperationId { get; set; }
        string OperationName { get; set; }
        string CorrelationId { get; set; }
        IDictionary<string, object> Properties { get; set; }
        
        IDocumentLoggerContext WithProperty(string key, object value);
        IDocumentLoggerContext WithDocument(string documentId, DocumentType documentType);
        IDocumentLoggerContext WithOperation(string operationId, string operationName);
    }

    /// <summary>
    /// Correlation ID provider interface
    /// </summary>
    public interface ICorrelationIdProvider
    {
        string GetCorrelationId();
        void SetCorrelationId(string correlationId);
    }

    /// <summary>
    /// Document logger context implementation
    /// </summary>
    public class DocumentLoggerContext : IDocumentLoggerContext
    {
        public string DocumentId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string OperationId { get; set; }
        public string OperationName { get; set; }
        public string CorrelationId { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public IDocumentLoggerContext WithProperty(string key, object value)
        {
            var newContext = new DocumentLoggerContext
            {
                DocumentId = DocumentId,
                DocumentType = DocumentType,
                OperationId = OperationId,
                OperationName = OperationName,
                CorrelationId = CorrelationId,
                Properties = new Dictionary<string, object>(Properties)
            };
            
            newContext.Properties[key] = value;
            return newContext;
        }

        public IDocumentLoggerContext WithDocument(string documentId, DocumentType documentType)
        {
            var newContext = new DocumentLoggerContext
            {
                DocumentId = documentId,
                DocumentType = documentType,
                OperationId = OperationId,
                OperationName = OperationName,
                CorrelationId = CorrelationId,
                Properties = new Dictionary<string, object>(Properties)
            };
            
            return newContext;
        }

        public IDocumentLoggerContext WithOperation(string operationId, string operationName)
        {
            var newContext = new DocumentLoggerContext
            {
                DocumentId = DocumentId,
                DocumentType = DocumentType,
                OperationId = operationId,
                OperationName = operationName,
                CorrelationId = CorrelationId,
                Properties = new Dictionary<string, object>(Properties)
            };
            
            return newContext;
        }
    }

    /// <summary>
    /// Correlation ID provider implementation
    /// </summary>
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private string _correlationId;

        public string GetCorrelationId()
        {
            return _correlationId ?? Guid.NewGuid().ToString();
        }

        public void SetCorrelationId(string correlationId)
        {
            _correlationId = correlationId;
        }
    }
}
