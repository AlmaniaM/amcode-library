using System.IO;
using System.Threading.Tasks;
using AMCode.Storage.UnitTests.Globals;

namespace AMCode.Storage.UnitTests.Local
{
    public class LocalTestHelper : TestHelper
    {
        public LocalTestHelper()
            : base(Path.Combine("Components", "Local"))
        { }

        /// <summary>
        /// Copy one file to another location.
        /// </summary>
        /// <param name="originalFilePath">The original file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <returns>A void <see cref="Task"/>.</returns>
        public async Task CopyFiles(string originalFilePath, string destinationFilePath)
        {
            using var originalFileStream = new FileStream(originalFilePath, FileMode.Open);
            using var destinationFileStream = new FileStream(destinationFilePath, FileMode.Create);
            await originalFileStream.CopyToAsync(destinationFileStream);
            await originalFileStream.DisposeAsync();
            await destinationFileStream.DisposeAsync();
        }
    }
}