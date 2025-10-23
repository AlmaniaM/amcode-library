using System.Collections.Generic;

namespace AMCode.Documents.Common.Logging.Configuration
{
    /// <summary>
    /// Options pattern for document logging configuration
    /// </summary>
    public class DocumentLoggingOptions
    {
        public DocumentLogLevel MinimumLevel { get; set; } = DocumentLogLevel.Information;
        public bool EnableConsole { get; set; } = true;
        public bool EnableFile { get; set; } = true;
        public bool EnableStructuredLogging { get; set; } = true;
        public string LogFilePath { get; set; } = "logs/documents.log";
        public int MaxFileSizeMB { get; set; } = 100;
        public int MaxFiles { get; set; } = 10;
        public bool EnableCorrelationId { get; set; } = true;
        public bool EnablePerformanceLogging { get; set; } = true;
        public bool EnableSensitiveDataMasking { get; set; } = true;
        public string[] SensitiveDataKeys { get; set; } = { "password", "token", "secret", "key", "filepath" };
        public Dictionary<string, DocumentLogLevel> CategoryLevels { get; set; } = new();
        public Dictionary<string, object> GlobalProperties { get; set; } = new();
        
        // Document-specific settings
        public bool EnableDocumentLifecycleLogging { get; set; } = true;
        public bool EnableProviderLogging { get; set; } = true;
        public bool EnablePerformanceMetrics { get; set; } = true;
        public bool EnableFileOperationLogging { get; set; } = true;
        public bool EnableContentOperationLogging { get; set; } = true;
        public bool EnableFormattingLogging { get; set; } = false;
        
        // Provider-specific settings
        public Dictionary<DocumentType, DocumentLogLevel> DocumentTypeLevels { get; set; } = new()
        {
            { DocumentType.Excel, DocumentLogLevel.Information },
            { DocumentType.Pdf, DocumentLogLevel.Information },
            { DocumentType.Docx, DocumentLogLevel.Information }
        };
        
        /// <summary>
        /// Console-specific options
        /// </summary>
        public ConsoleDocumentLoggerOptions Console { get; set; } = new();
        
        /// <summary>
        /// File-specific options
        /// </summary>
        public FileDocumentLoggerOptions File { get; set; } = new();
        
        /// <summary>
        /// Structured logging options
        /// </summary>
        public StructuredDocumentLoggerOptions Structured { get; set; } = new();
    }
    
    /// <summary>
    /// Console logger specific options
    /// </summary>
    public class ConsoleDocumentLoggerOptions
    {
        public bool EnableColor { get; set; } = true;
        public bool EnableTimestamp { get; set; } = true;
        public bool EnableCategory { get; set; } = true;
        public bool EnableDocumentType { get; set; } = true;
        public bool EnableCorrelationId { get; set; } = true;
    }
    
    /// <summary>
    /// File logger specific options
    /// </summary>
    public class FileDocumentLoggerOptions
    {
        public bool EnableRotation { get; set; } = true;
        public string DateFormat { get; set; } = "yyyy-MM-dd";
        public bool EnableCompression { get; set; } = true;
        public bool EnableAsyncWriting { get; set; } = true;
        public int BufferSizeKB { get; set; } = 64;
    }
    
    /// <summary>
    /// Structured logger specific options
    /// </summary>
    public class StructuredDocumentLoggerOptions
    {
        public bool EnableConsole { get; set; } = true;
        public bool EnableFile { get; set; } = true;
        public bool EnableExternal { get; set; } = false;
        public string Format { get; set; } = "json";
        public string ExternalEndpoint { get; set; }
        public Dictionary<string, string> ExternalHeaders { get; set; } = new();
    }
}
