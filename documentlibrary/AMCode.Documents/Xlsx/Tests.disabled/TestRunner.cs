using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AMCode.Documents.Xlsx.Tests
{
    /// <summary>
    /// Console application to execute all Xlsx tests with reporting and performance metrics
    /// </summary>
    public class TestRunner
    {
        private readonly TestConfiguration _configuration;
        private readonly TestResults _results;

        /// <summary>
        /// Initializes a new instance of the TestRunner class
        /// </summary>
        /// <param name="configuration">Test configuration</param>
        public TestRunner(TestConfiguration configuration = null)
        {
            _configuration = configuration ?? new TestConfiguration();
            _results = new TestResults();
        }

        /// <summary>
        /// Main entry point for the test runner
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static async Task Main(string[] args)
        {
            var configuration = ParseCommandLineArguments(args);
            var runner = new TestRunner(configuration);
            
            Console.WriteLine("Xlsx Test Runner");
            Console.WriteLine("================");
            Console.WriteLine();
            
            var success = await runner.RunAllTestsAsync();
            
            Console.WriteLine();
            Console.WriteLine("Test Run Complete");
            Console.WriteLine($"Overall Result: {(success ? "PASSED" : "FAILED")}");
            
            Environment.Exit(success ? 0 : 1);
        }

        /// <summary>
        /// Runs all tests asynchronously
        /// </summary>
        /// <returns>True if all tests passed, false otherwise</returns>
        public async Task<bool> RunAllTestsAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                // Run unit tests
                Console.WriteLine("Running Unit Tests...");
                var unitTestResult = await RunTestCategoryAsync("Unit");
                
                // Run integration tests
                Console.WriteLine("Running Integration Tests...");
                var integrationTestResult = await RunTestCategoryAsync("Integration");
                
                // Run performance tests
                Console.WriteLine("Running Performance Tests...");
                var performanceTestResult = await RunTestCategoryAsync("Performance");
                
                // Calculate overall result
                var overallSuccess = unitTestResult && integrationTestResult && performanceTestResult;
                
                stopwatch.Stop();
                _results.TotalExecutionTime = stopwatch.Elapsed;
                _results.OverallSuccess = overallSuccess;
                
                // Generate reports
                GenerateReports();
                
                return overallSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running tests: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Runs tests for a specific category
        /// </summary>
        /// <param name="category">Test category</param>
        /// <returns>True if all tests in category passed, false otherwise</returns>
        public async Task<bool> RunTestCategoryAsync(string category)
        {
            var stopwatch = Stopwatch.StartNew();
            var categoryResults = new List<TestResult>();
            
            try
            {
                // Get test assembly
                var assembly = Assembly.GetExecutingAssembly();
                
                // Get test classes for the category
                var testClasses = GetTestClassesForCategory(assembly, category);
                
                foreach (var testClass in testClasses)
                {
                    var classResults = await RunTestClassAsync(testClass);
                    categoryResults.AddRange(classResults);
                }
                
                stopwatch.Stop();
                
                // Update results
                _results.CategoryResults[category] = new CategoryResult
                {
                    Category = category,
                    ExecutionTime = stopwatch.Elapsed,
                    TestResults = categoryResults,
                    Success = categoryResults.All(r => r.Success)
                };
                
                // Print category summary
                PrintCategorySummary(category, categoryResults, stopwatch.Elapsed);
                
                return categoryResults.All(r => r.Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running {category} tests: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Runs tests for a specific test class
        /// </summary>
        /// <param name="testClass">Test class type</param>
        /// <returns>List of test results</returns>
        private async Task<List<TestResult>> RunTestClassAsync(Type testClass)
        {
            var results = new List<TestResult>();
            
            try
            {
                // Create test instance
                var testInstance = Activator.CreateInstance(testClass);
                
                // Get test methods
                var testMethods = GetTestMethods(testClass);
                
                foreach (var testMethod in testMethods)
                {
                    var testResult = await RunTestMethodAsync(testInstance, testMethod);
                    results.Add(testResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running test class {testClass.Name}: {ex.Message}");
            }
            
            return results;
        }

        /// <summary>
        /// Runs a specific test method
        /// </summary>
        /// <param name="testInstance">Test class instance</param>
        /// <param name="testMethod">Test method info</param>
        /// <returns>Test result</returns>
        private async Task<TestResult> RunTestMethodAsync(object testInstance, MethodInfo testMethod)
        {
            var stopwatch = Stopwatch.StartNew();
            var testResult = new TestResult
            {
                TestName = $"{testInstance.GetType().Name}.{testMethod.Name}",
                StartTime = DateTime.Now
            };
            
            try
            {
                // Setup
                if (testInstance is IUnitTestBase unitTest)
                {
                    unitTest.SetUp();
                }
                
                // Run test
                if (testMethod.ReturnType == typeof(Task))
                {
                    await (Task)testMethod.Invoke(testInstance, null);
                }
                else
                {
                    testMethod.Invoke(testInstance, null);
                }
                
                testResult.Success = true;
                testResult.Message = "Test passed";
            }
            catch (Exception ex)
            {
                testResult.Success = false;
                testResult.Message = ex.Message;
                testResult.Exception = ex;
            }
            finally
            {
                // Teardown
                if (testInstance is IUnitTestBase unitTest)
                {
                    unitTest.TearDown();
                }
                
                stopwatch.Stop();
                testResult.ExecutionTime = stopwatch.Elapsed;
                testResult.EndTime = DateTime.Now;
            }
            
            return testResult;
        }

        /// <summary>
        /// Gets test classes for a specific category
        /// </summary>
        /// <param name="assembly">Test assembly</param>
        /// <param name="category">Test category</param>
        /// <returns>List of test class types</returns>
        private List<Type> GetTestClassesForCategory(Assembly assembly, string category)
        {
            return assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<TestFixtureAttribute>() != null)
                .Where(t => t.Namespace.Contains(category))
                .ToList();
        }

        /// <summary>
        /// Gets test methods from a test class
        /// </summary>
        /// <param name="testClass">Test class type</param>
        /// <returns>List of test method infos</returns>
        private List<MethodInfo> GetTestMethods(Type testClass)
        {
            return testClass.GetMethods()
                .Where(m => m.GetCustomAttribute<TestAttribute>() != null)
                .ToList();
        }

        /// <summary>
        /// Prints category summary
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="results">Test results</param>
        /// <param name="executionTime">Execution time</param>
        private void PrintCategorySummary(string category, List<TestResult> results, TimeSpan executionTime)
        {
            var passed = results.Count(r => r.Success);
            var failed = results.Count - passed;
            
            Console.WriteLine($"  {category} Tests: {passed} passed, {failed} failed in {executionTime.TotalMilliseconds:F0}ms");
            
            if (failed > 0)
            {
                Console.WriteLine("  Failed Tests:");
                foreach (var result in results.Where(r => !r.Success))
                {
                    Console.WriteLine($"    - {result.TestName}: {result.Message}");
                }
            }
        }

        /// <summary>
        /// Generates test reports
        /// </summary>
        private void GenerateReports()
        {
            // Generate console report
            GenerateConsoleReport();
            
            // Generate file reports if configured
            if (_configuration.GenerateFileReports)
            {
                GenerateFileReports();
            }
        }

        /// <summary>
        /// Generates console report
        /// </summary>
        private void GenerateConsoleReport()
        {
            Console.WriteLine();
            Console.WriteLine("Test Summary");
            Console.WriteLine("============");
            Console.WriteLine($"Total Execution Time: {_results.TotalExecutionTime.TotalMilliseconds:F0}ms");
            Console.WriteLine($"Overall Result: {(_results.OverallSuccess ? "PASSED" : "FAILED")}");
            Console.WriteLine();
            
            foreach (var categoryResult in _results.CategoryResults.Values)
            {
                var passed = categoryResult.TestResults.Count(r => r.Success);
                var failed = categoryResult.TestResults.Count - passed;
                
                Console.WriteLine($"{categoryResult.Category} Tests:");
                Console.WriteLine($"  Passed: {passed}");
                Console.WriteLine($"  Failed: {failed}");
                Console.WriteLine($"  Execution Time: {categoryResult.ExecutionTime.TotalMilliseconds:F0}ms");
                Console.WriteLine();
            }
            
            // Performance metrics
            if (_results.CategoryResults.ContainsKey("Performance"))
            {
                GeneratePerformanceReport();
            }
        }

        /// <summary>
        /// Generates performance report
        /// </summary>
        private void GeneratePerformanceReport()
        {
            Console.WriteLine("Performance Metrics");
            Console.WriteLine("===================");
            
            var performanceResults = _results.CategoryResults["Performance"].TestResults;
            
            // Group by test type
            var testGroups = performanceResults.GroupBy(r => r.TestName.Split('.')[0]);
            
            foreach (var group in testGroups)
            {
                Console.WriteLine($"{group.Key}:");
                var times = group.Select(r => r.ExecutionTime.TotalMilliseconds).ToList();
                Console.WriteLine($"  Average Time: {times.Average():F2}ms");
                Console.WriteLine($"  Min Time: {times.Min():F2}ms");
                Console.WriteLine($"  Max Time: {times.Max():F2}ms");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Generates file reports
        /// </summary>
        private void GenerateFileReports()
        {
            try
            {
                // Ensure output directory exists
                Directory.CreateDirectory(_configuration.OutputDirectory);
                
                // Generate JSON report
                GenerateJsonReport();
                
                // Generate HTML report
                GenerateHtmlReport();
                
                // Generate CSV report
                GenerateCsvReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating file reports: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates JSON report
        /// </summary>
        private void GenerateJsonReport()
        {
            var jsonPath = Path.Combine(_configuration.OutputDirectory, "test-results.json");
            var json = System.Text.Json.JsonSerializer.Serialize(_results, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, json);
            Console.WriteLine($"JSON report generated: {jsonPath}");
        }

        /// <summary>
        /// Generates HTML report
        /// </summary>
        private void GenerateHtmlReport()
        {
            var htmlPath = Path.Combine(_configuration.OutputDirectory, "test-results.html");
            var html = GenerateHtmlContent();
            File.WriteAllText(htmlPath, html);
            Console.WriteLine($"HTML report generated: {htmlPath}");
        }

        /// <summary>
        /// Generates CSV report
        /// </summary>
        private void GenerateCsvReport()
        {
            var csvPath = Path.Combine(_configuration.OutputDirectory, "test-results.csv");
            using var writer = new StreamWriter(csvPath);
            
            // Write header
            writer.WriteLine("TestName,Category,Success,ExecutionTime,Message");
            
            // Write data
            foreach (var categoryResult in _results.CategoryResults.Values)
            {
                foreach (var testResult in categoryResult.TestResults)
                {
                    writer.WriteLine($"{testResult.TestName},{categoryResult.Category},{testResult.Success},{testResult.ExecutionTime.TotalMilliseconds},{testResult.Message}");
                }
            }
            
            Console.WriteLine($"CSV report generated: {csvPath}");
        }

        /// <summary>
        /// Generates HTML content
        /// </summary>
        /// <returns>HTML content string</returns>
        private string GenerateHtmlContent()
        {
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Xlsx Test Results</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ background-color: #f0f0f0; padding: 20px; border-radius: 5px; }}
        .summary {{ margin: 20px 0; }}
        .category {{ margin: 20px 0; border: 1px solid #ccc; border-radius: 5px; }}
        .category-header {{ background-color: #e0e0e0; padding: 10px; font-weight: bold; }}
        .category-content {{ padding: 10px; }}
        .test {{ margin: 5px 0; padding: 5px; border-radius: 3px; }}
        .test.passed {{ background-color: #d4edda; }}
        .test.failed {{ background-color: #f8d7da; }}
        .performance {{ background-color: #fff3cd; padding: 10px; border-radius: 5px; margin: 10px 0; }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>Xlsx Test Results</h1>
        <p>Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
        <p>Total Execution Time: {_results.TotalExecutionTime.TotalMilliseconds:F0}ms</p>
        <p>Overall Result: <strong>{(_results.OverallSuccess ? "PASSED" : "FAILED")}</strong></p>
    </div>
    
    <div class=""summary"">
        <h2>Summary</h2>
        <p>Total Tests: {_results.CategoryResults.Values.Sum(c => c.TestResults.Count)}</p>
        <p>Passed: {_results.CategoryResults.Values.Sum(c => c.TestResults.Count(r => r.Success))}</p>
        <p>Failed: {_results.CategoryResults.Values.Sum(c => c.TestResults.Count(r => !r.Success))}</p>
    </div>
";

            foreach (var categoryResult in _results.CategoryResults.Values)
            {
                html += $@"
    <div class=""category"">
        <div class=""category-header"">
            {categoryResult.Category} Tests ({categoryResult.TestResults.Count(r => r.Success)}/{categoryResult.TestResults.Count} passed)
        </div>
        <div class=""category-content"">
";

                foreach (var testResult in categoryResult.TestResults)
                {
                    var cssClass = testResult.Success ? "passed" : "failed";
                    html += $@"
            <div class=""test {cssClass}"">
                <strong>{testResult.TestName}</strong> - {testResult.ExecutionTime.TotalMilliseconds:F0}ms
                {(!testResult.Success ? $"<br/>Error: {testResult.Message}" : "")}
            </div>
";
                }

                html += @"
        </div>
    </div>
";
            }

            html += @"
</body>
</html>";

            return html;
        }

        /// <summary>
        /// Parses command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Test configuration</returns>
        private static TestConfiguration ParseCommandLineArguments(string[] args)
        {
            var configuration = new TestConfiguration();
            
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--output":
                    case "-o":
                        if (i + 1 < args.Length)
                        {
                            configuration.OutputDirectory = args[++i];
                        }
                        break;
                        
                    case "--categories":
                    case "-c":
                        if (i + 1 < args.Length)
                        {
                            configuration.TestCategories = args[++i].Split(',').ToList();
                        }
                        break;
                        
                    case "--generate-reports":
                    case "-r":
                        configuration.GenerateFileReports = true;
                        break;
                        
                    case "--help":
                    case "-h":
                        PrintHelp();
                        Environment.Exit(0);
                        break;
                }
            }
            
            return configuration;
        }

        /// <summary>
        /// Prints help information
        /// </summary>
        private static void PrintHelp()
        {
            Console.WriteLine("Xlsx Test Runner");
            Console.WriteLine("================");
            Console.WriteLine();
            Console.WriteLine("Usage: TestRunner [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -o, --output <directory>    Output directory for reports");
            Console.WriteLine("  -c, --categories <list>     Comma-separated list of test categories");
            Console.WriteLine("  -r, --generate-reports      Generate file reports (JSON, HTML, CSV)");
            Console.WriteLine("  -h, --help                  Show this help message");
            Console.WriteLine();
        }
    }

    #region Helper Interfaces and Classes

    /// <summary>
    /// Interface for test base classes
    /// </summary>
    public interface IUnitTestBase
    {
        void SetUp();
        void TearDown();
    }

    /// <summary>
    /// Test result for individual tests
    /// </summary>
    public class TestResult
    {
        public string TestName { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan ExecutionTime { get; set; }
    }

    /// <summary>
    /// Category result for test categories
    /// </summary>
    public class CategoryResult
    {
        public string Category { get; set; }
        public bool Success { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public List<TestResult> TestResults { get; set; } = new List<TestResult>();
    }

    /// <summary>
    /// Overall test results
    /// </summary>
    public class TestResults
    {
        public bool OverallSuccess { get; set; }
        public TimeSpan TotalExecutionTime { get; set; }
        public Dictionary<string, CategoryResult> CategoryResults { get; set; } = new Dictionary<string, CategoryResult>();
    }

    #endregion
}
