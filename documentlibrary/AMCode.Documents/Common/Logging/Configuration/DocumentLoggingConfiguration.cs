using System.Collections.Generic;

namespace AMCode.Documents.Common.Logging.Configuration
{
    /// <summary>
    /// Document-specific logging configuration settings
    /// </summary>
    public class DocumentLoggingConfiguration
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
        public bool EnableFormattingLogging { get; set; } = false; // Verbose by default
        
        // Provider-specific settings
        public Dictionary<DocumentType, DocumentLogLevel> DocumentTypeLevels { get; set; } = new()
        {
            { DocumentType.Excel, DocumentLogLevel.Information },
            { DocumentType.Pdf, DocumentLogLevel.Information },
            { DocumentType.Docx, DocumentLogLevel.Information }
        };
    }
}
