using System.IO;
using DlStorageLibrary.UnitTests.Globals;

namespace DlStorageLibrary.UnitTests.Memory
{
    public class LocalTestHelper : TestHelper
    {
        public LocalTestHelper()
            : base(Path.Combine("Components", "InMemory"))
        { }
    }
}