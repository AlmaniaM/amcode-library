using System.Collections.Generic;

namespace AMCode.Data.Logging.Configuration
{
    /// <summary>
    /// Logging configuration settings for AMCode.Data library
    /// </summary>
    public class LoggingConfiguration
    {
        public LogLevel MinimumLevel { get; set; } = LogLevel.Information;
        public bool EnableConsole { get; set; } = true;
        public bool EnableFile { get; set; } = false;
        public bool EnableStructuredLogging { get; set; } = true;
        public string LogFilePath { get; set; } = "logs/amcode-data.log";
        public int MaxFileSizeMB { get; set; } = 100;
        public int MaxFiles { get; set; } = 10;
        public bool EnableCorrelationId { get; set; } = true;
        public bool EnablePerformanceLogging { get; set; } = true;
        public bool EnableSensitiveDataMasking { get; set; } = true;
        public string[] SensitiveDataKeys { get; set; } = { "password", "connectionstring", "secret", "key" };
        public Dictionary<string, LogLevel> CategoryLevels { get; set; } = new();
        public Dictionary<string, object> GlobalProperties { get; set; } = new();
    }
}
