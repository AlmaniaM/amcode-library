using System.IO;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Extensions.Models
{
    class LocalTestHelpers
    {
        /// <summary>
        /// Get the directory of the mock files for the ReturnAuthorization test environment.
        /// </summary>
        /// <param name="testContext">The <see cref="TestContext"/> from you test class.</param>
        /// <returns>An absolute <see cref="string"/> path to the ReturnAuthorization mock files directory.</returns>
        public static string GetMockFilesPath(TestContext testContext) => Path.Combine(GetTestDirectoryPath(testContext), "MockFiles");

        /// <summary>
        /// Get the directory of the ReturnAuthorization test environment.
        /// </summary>
        /// <param name="testContext">The <see cref="TestContext"/> from you test class.</param>
        /// <returns>An absolute <see cref="string"/> path to the ReturnAuthorization test directory.</returns>
        public static string GetTestDirectoryPath(TestContext testContext) => Path.Combine(testContext.TestDirectory, "Components", "Extensions");

        /// <summary>
        /// Get the resource files directory for the ReturnAuthorization test environment.
        /// </summary>
        /// <param name="testContext">The <see cref="TestContext"/> from you test class.</param>
        /// <returns>An absolute <see cref="string"/> path to the ReturnAuthorization resource files directory.</returns>
        public static string GetTestWorkDirectoryPath(TestContext testContext) => Path.Combine(GetTestDirectoryPath(testContext), "TestWorkDirectory");

        /// <summary>
        /// Gets an absolute <see cref="string"/> path to a mock file in the ReturnAuthorization test directory.
        /// </summary>
        /// <param name="fileName">A <see cref="string"/> file name of the file you want.</param>
        /// <param name="testContext">The <see cref="TestContext"/> of your test class.</param>
        /// <returns>A <see cref="string"/> absolute patht the the mock file in the ReturnAuthorization test directory.</returns>
        public static string GetFilePath(string fileName, TestContext testContext) => Path.Combine(GetMockFilesPath(testContext), fileName);
    }
}