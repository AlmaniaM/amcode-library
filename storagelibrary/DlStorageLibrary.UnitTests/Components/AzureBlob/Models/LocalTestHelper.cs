using System.IO;
using DlStorageLibrary.UnitTests.Globals;

namespace DlStorageLibrary.UnitTests.AzureBlob
{
    public class LocalTestHelper : TestHelper
    {
        public LocalTestHelper()
            : base(Path.Combine("Components", "AzureBlob"))
        { }
    }
}