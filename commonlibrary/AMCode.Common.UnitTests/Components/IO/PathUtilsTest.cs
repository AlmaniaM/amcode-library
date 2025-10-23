using System.IO;
using AMCode.Common.IO;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.PathUtilsTests
{
    [TestFixture]
    public class PathUtilsTest
    {
        [Test]
        public void ShouldCombinePathsWithoutEndingSlashes()
        {
            var path = PathUtils.CombinePaths("C:\\temp", "folder\\for", "testing", "path\\utils\\class");
            var expected = Path.Combine("C:", "temp", "folder", "for", "testing", "path", "utils", "class");
            Assert.AreEqual(expected, path);
        }

        [Test]
        public void ShouldCombinePathsWithEndingSlashes()
        {
            var path = PathUtils.CombinePaths("C:\\temp\\", "\\folder\\for\\", "\\testing", "\\path\\utils\\class\\");
            var expected = Path.Combine("C:", "temp", "folder", "for", "testing", "path", "utils", "class");
            Assert.AreEqual(expected, path);
        }
    }
}