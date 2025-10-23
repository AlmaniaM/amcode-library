using System;
using System.Collections.Generic;
using System.IO;

namespace AMCode.Documents.Xlsx.Tests
{
    /// <summary>
    /// Configuration class for test settings
    /// </summary>
    public class TestConfiguration
    {
        /// <summary>
        /// Gets or sets the output directory for test reports
        /// </summary>
        public string OutputDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "TestOutput");

        /// <summary>
        /// Gets or sets the test data directory
        /// </summary>
        public string TestDataDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "TestData");

        /// <summary>
        /// Gets or sets the list of test categories to run
        /// </summary>
        public List<string> TestCategories { get; set; } = new List<string> { "Unit", "Integration", "Performance" };

        /// <summary>
        /// Gets or sets whether to generate file reports
        /// </summary>
        public bool GenerateFileReports { get; set; } = false;

        /// <summary>
        /// Gets or sets the test timeout in milliseconds
        /// </summary>
        public int TestTimeout { get; set; } = 30000;

        /// <summary>
        /// Gets or sets whether to run tests in parallel
        /// </summary>
        public bool RunTestsInParallel { get; set; } = false;

        /// <summary>
        /// Gets or sets the maximum degree of parallelism
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;

        /// <summary>
        /// Gets or sets whether to include performance metrics
        /// </summary>
        public bool IncludePerformanceMetrics { get; set; } = true;

        /// <summary>
        /// Gets or sets the performance threshold in milliseconds
        /// </summary>
        public int PerformanceThreshold { get; set; } = 1000;

        /// <summary>
        /// Gets or sets whether to generate detailed logs
        /// </summary>
        public bool GenerateDetailedLogs { get; set; } = false;

        /// <summary>
        /// Gets or sets the log level
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Gets or sets whether to stop on first failure
        /// </summary>
        public bool StopOnFirstFailure { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to retry failed tests
        /// </summary>
        public bool RetryFailedTests { get; set; } = false;

        /// <summary>
        /// Gets or sets the number of retry attempts
        /// </summary>
        public int RetryAttempts { get; set; } = 3;

        /// <summary>
        /// Gets or sets whether to clean up test artifacts
        /// </summary>
        public bool CleanupTestArtifacts { get; set; } = true;

        /// <summary>
        /// Gets or sets the test data cleanup threshold in days
        /// </summary>
        public int TestDataCleanupThresholdDays { get; set; } = 7;
    }

    /// <summary>
    /// Log levels for test configuration
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}
