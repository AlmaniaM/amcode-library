using System;
using System.Collections.Generic;

namespace AMCode.Documents.Common.Logging.Models
{
    /// <summary>
    /// Document performance metrics for logging
    /// </summary>
    public class DocumentPerformanceMetrics
    {
        public TimeSpan Duration { get; set; }
        public long MemoryUsedBytes { get; set; }
        public int PageCount { get; set; }
        public int WorksheetCount { get; set; }
        public int CellCount { get; set; }
        public int ParagraphCount { get; set; }
        public long FileSizeBytes { get; set; }
        public int OperationCount { get; set; }
        public double CpuUsagePercent { get; set; }
        public IDictionary<string, object> CustomMetrics { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Creates a performance metrics instance for document creation
        /// </summary>
        public static DocumentPerformanceMetrics ForDocumentCreation(TimeSpan duration, long fileSizeBytes, int pageCount = 0, int worksheetCount = 0)
        {
            return new DocumentPerformanceMetrics
            {
                Duration = duration,
                FileSizeBytes = fileSizeBytes,
                PageCount = pageCount,
                WorksheetCount = worksheetCount,
                OperationCount = 1
            };
        }
        
        /// <summary>
        /// Creates a performance metrics instance for content operations
        /// </summary>
        public static DocumentPerformanceMetrics ForContentOperation(TimeSpan duration, int operationCount, long memoryUsedBytes = 0)
        {
            return new DocumentPerformanceMetrics
            {
                Duration = duration,
                OperationCount = operationCount,
                MemoryUsedBytes = memoryUsedBytes
            };
        }
        
        /// <summary>
        /// Adds a custom metric
        /// </summary>
        public DocumentPerformanceMetrics WithMetric(string key, object value)
        {
            CustomMetrics[key] = value;
            return this;
        }
        
        /// <summary>
        /// Merges this metrics with another metrics instance
        /// </summary>
        public DocumentPerformanceMetrics Merge(DocumentPerformanceMetrics other)
        {
            if (other == null) return this;
            
            return new DocumentPerformanceMetrics
            {
                Duration = Duration + other.Duration,
                MemoryUsedBytes = Math.Max(MemoryUsedBytes, other.MemoryUsedBytes),
                PageCount = PageCount + other.PageCount,
                WorksheetCount = WorksheetCount + other.WorksheetCount,
                CellCount = CellCount + other.CellCount,
                ParagraphCount = ParagraphCount + other.ParagraphCount,
                FileSizeBytes = Math.Max(FileSizeBytes, other.FileSizeBytes),
                OperationCount = OperationCount + other.OperationCount,
                CpuUsagePercent = Math.Max(CpuUsagePercent, other.CpuUsagePercent),
                CustomMetrics = new Dictionary<string, object>(CustomMetrics)
            };
        }
    }
}
