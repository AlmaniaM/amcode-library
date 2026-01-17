using System.IO;
using AMCode.Exports.SharedTestLibrary.Global;
using NUnit.Framework;

namespace AMCode.Exports.IntegrationTests.BookBuilder.BookCompilerTests
{
    internal class TestHelper : TestHelperBase
    {
        /// <summary>
        /// Get the directory of the IO.Zip test environment.
        /// </summary>
        /// <param name="testContext">The <see cref="TestContext"/> from you test class.</param>
        /// <returns>An absolute <see cref="string"/> path to the ReturnAuthorization test directory.</returns>
        public override string GetTestDirectoryPath(TestContext testContext) => Path.Combine(testContext.TestDirectory, "Components", "BookBuilder", "BookCompiler");
    }
}