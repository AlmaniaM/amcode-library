using System.IO;
using NUnit.Framework;

namespace DlStorageLibrary.UnitTests.Globals
{
    /// <summary>
    /// A class designed to provide some helper methods for test environments.
    /// </summary>
    public class TestHelper
    {
        private readonly string testDirectory;

        /// <summary>
        /// Create an instance of the <see cref="TestHelper"/> class.
        /// </summary>
        /// <param name="testDirectory">Provide a relative <see cref="string"/> path starting from the
        /// project root up to your test directory. Each directory should be separated by a double back-slash (\\).</param>
        public TestHelper(string testDirectory)
        {
            this.testDirectory = testDirectory;
        }

        /// <summary>
        /// Get the directory of the mock files for the TestFixture test environment.
        /// </summary>
        /// <param name="testContext">Provide a <see cref="TestContext"/> to get test work directory.</param>
        /// <returns>An absolute <see cref="string"/> path to the UploadValidator mock files directory.</returns>
        public string GetMockFilesDirectoryPath(TestContext testContext)
            => Path.Combine(GetTestDirectoryPath(testContext), "Mocks");

        /// <summary>
        /// Get the directory of the TestFixture test environment.
        /// </summary>
        /// <param name="testContext">Provide a <see cref="TestContext"/> to get test work directory.</param>
        /// <returns>An absolute <see cref="string"/> path to the UploadValidator test directory.</returns>
        public string GetTestDirectoryPath(TestContext testContext)
            => Path.Combine(testContext.WorkDirectory, testDirectory);

        /// <summary>
        /// Get the resource files directory for the TestFixture test environment.
        /// </summary>
        /// <param name="testContext">Provide a <see cref="TestContext"/> to get test work directory.</param>
        /// <returns>An absolute <see cref="string"/> path to the UploadValidator resource files directory.</returns>
        public string GetTestWorkDirectoryPath(TestContext testContext)
            => Path.Combine(GetTestDirectoryPath(testContext), "TestWorkDirectory");

        /// <summary>
        /// Gets an absolute <see cref="string"/> path to a mock file in the TestFixture test directory.
        /// </summary>
        /// <param name="testContext">Provide a <see cref="TestContext"/> to get test work directory.</param>
        /// <param name="fileName">A <see cref="string"/> file name of the file you want.</param>
        /// <returns>A <see cref="string"/> absolute path the mock file in the UploadValidator test directory.</returns>
        public string GetMockFilePath(TestContext testContext, string fileName)
            => Path.Combine(GetMockFilesDirectoryPath(testContext), fileName);
    }
}