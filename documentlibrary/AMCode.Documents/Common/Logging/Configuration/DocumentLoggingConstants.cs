using System.Collections.Generic;

namespace AMCode.Documents.Common.Logging.Configuration
{
    /// <summary>
    /// Constants and defaults for document logging
    /// </summary>
    public static class DocumentLoggingConstants
    {
        /// <summary>
        /// Default log file path
        /// </summary>
        public const string DefaultLogFilePath = "logs/documents.log";
        
        /// <summary>
        /// Default maximum file size in MB
        /// </summary>
        public const int DefaultMaxFileSizeMB = 100;
        
        /// <summary>
        /// Default maximum number of files
        /// </summary>
        public const int DefaultMaxFiles = 10;
        
        /// <summary>
        /// Default buffer size in KB
        /// </summary>
        public const int DefaultBufferSizeKB = 64;
        
        /// <summary>
        /// Default date format for log files
        /// </summary>
        public const string DefaultDateFormat = "yyyy-MM-dd";
        
        /// <summary>
        /// Default application name
        /// </summary>
        public const string DefaultApplicationName = "AMCode.Documents";
        
        /// <summary>
        /// Default log level
        /// </summary>
        public const DocumentLogLevel DefaultLogLevel = DocumentLogLevel.Information;
        
        /// <summary>
        /// Default sensitive data keys
        /// </summary>
        public static readonly string[] DefaultSensitiveDataKeys = 
        {
            "password", "token", "secret", "key", "filepath", "connectionstring", "apikey"
        };
        
        /// <summary>
        /// Default category levels
        /// </summary>
        public static readonly Dictionary<string, DocumentLogLevel> DefaultCategoryLevels = new()
        {
            { "Microsoft", DocumentLogLevel.Warning },
            { "System", DocumentLogLevel.Warning },
            { "Microsoft.Hosting.Lifetime", DocumentLogLevel.Information },
            { "AMCode.Documents", DocumentLogLevel.Information }
        };
        
        /// <summary>
        /// Default document type levels
        /// </summary>
        public static readonly Dictionary<DocumentType, DocumentLogLevel> DefaultDocumentTypeLevels = new()
        {
            { DocumentType.Excel, DocumentLogLevel.Information },
            { DocumentType.Pdf, DocumentLogLevel.Information },
            { DocumentType.Docx, DocumentLogLevel.Information },
            { DocumentType.Unknown, DocumentLogLevel.Warning }
        };
        
        /// <summary>
        /// Log level names for display
        /// </summary>
        public static readonly Dictionary<DocumentLogLevel, string> LogLevelNames = new()
        {
            { DocumentLogLevel.Trace, "TRACE" },
            { DocumentLogLevel.Debug, "DEBUG" },
            { DocumentLogLevel.Information, "INFO" },
            { DocumentLogLevel.Warning, "WARN" },
            { DocumentLogLevel.Error, "ERROR" },
            { DocumentLogLevel.Critical, "CRITICAL" },
            { DocumentLogLevel.None, "NONE" }
        };
        
        /// <summary>
        /// Document type names for display
        /// </summary>
        public static readonly Dictionary<DocumentType, string> DocumentTypeNames = new()
        {
            { DocumentType.Excel, "Excel" },
            { DocumentType.Pdf, "PDF" },
            { DocumentType.Docx, "DOCX" },
            { DocumentType.Unknown, "Unknown" }
        };
    }
}
