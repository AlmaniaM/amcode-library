using System.IO;
using System.Text;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Streams;
using AMCode.Common.Extensions.Strings;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.StreamExtensionsTests
{
    [TestFixture]
    public class StreamExtensionsTest
    {
        private Stream stream;
        private readonly string testText = "Testing text";

        [SetUp]
        public void SetUp() => stream = testText.GetStream();

        [TearDown]
        public void TearDown() => stream?.Dispose();

        [Test]
        public async Task ShouldTest()
        {
            var bytes = await stream.ToByteArrayAsync();
            var text = Encoding.UTF8.GetString(bytes);
            Assert.That(text, Is.EqualTo(testText));
        }
    }
}