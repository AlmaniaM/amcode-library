using System.IO;
using AMCode.Storage.UnitTests.Globals;

namespace AMCode.Storage.UnitTests.Memory
{
    public class LocalTestHelper : TestHelper
    {
        public LocalTestHelper()
            : base(Path.Combine("Components", "InMemory"))
        { }
    }
}