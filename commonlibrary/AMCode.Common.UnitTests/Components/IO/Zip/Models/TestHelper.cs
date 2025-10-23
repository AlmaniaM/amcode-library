using System.IO;
using AMCode.Common.UnitTests.Globals;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.Zip.Models
{
    internal class TestHelper : TestHelperBase
    {
        /// <summary>
        /// Get the directory of the IO.Zip test environment.
        /// </summary>
        /// <param name="testContext">The <see cref="TestContext"/> from you test class.</param>
        /// <returns>An absolute <see cref="string"/> path to the ReturnAuthorization test directory.</returns>
        public override string GetTestDirectoryPath(TestContext testContext) => Path.Combine(testContext.TestDirectory, "Components", "IO", "Zip");
    }
}