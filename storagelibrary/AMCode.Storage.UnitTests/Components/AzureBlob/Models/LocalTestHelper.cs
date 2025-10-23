using System.IO;
using AMCode.Storage.UnitTests.Globals;

namespace AMCode.Storage.UnitTests.AzureBlob
{
    public class LocalTestHelper : TestHelper
    {
        public LocalTestHelper()
            : base(Path.Combine("Components", "AzureBlob"))
        { }
    }
}