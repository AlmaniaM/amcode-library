using AMCode.Common.Extensions.Strings;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.StringTests
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void ShouldPassIsNullEmptyOrWhiteSpace()
        {
            Assert.IsTrue("".IsNullEmptyOrWhiteSpace());

            string nullString = null;
            Assert.IsTrue(nullString.IsNullEmptyOrWhiteSpace());

            Assert.IsTrue(" ".IsNullEmptyOrWhiteSpace());
        }

        [Test]
        public void ShouldPassEqualsIgnoreCase()
        {
            Assert.IsTrue("Hello".EqualsIgnoreCase("hello"));
            Assert.IsTrue("my name is c-sharp".EqualsIgnoreCase("MY NAME IS C-SHARP"));
        }

        [TestCase("|")]
        [TestCase("&")]
        [TestCase("#")]
        [TestCase("*")]
        [TestCase("%")]
        [TestCase("!")]
        [TestCase("delimiter")]
        public void ShouldSplitStringWhileIgnoringCommas(string delimiter)
        {
            var splitText = $"1, 2{delimiter}3{delimiter}4,5,6,7,8,9,10".SplitIgnoreComma(delimiter);
            Assert.AreEqual(3, splitText.Length);
            Assert.AreEqual("1, 2", splitText[0]);
            Assert.AreEqual("3", splitText[1]);
            Assert.AreEqual("4,5,6,7,8,9,10", splitText[2]);
        }

        [TestCase("|")]
        [TestCase("&")]
        [TestCase("#")]
        [TestCase("*")]
        [TestCase("%")]
        [TestCase("!")]
        [TestCase("delimiter")]
        public void ShouldSplitStringWithWhitespaceAsNull(string delimiter)
        {
            var splitText = $"1, 2{delimiter}3{delimiter}4{delimiter}{delimiter}".SplitIgnoreComma(delimiter, true);
            Assert.AreEqual(5, splitText.Length);
            Assert.AreEqual("1, 2", splitText[0]);
            Assert.AreEqual("3", splitText[1]);
            Assert.AreEqual("4", splitText[2]);
            Assert.IsNull(splitText[3]);
            Assert.IsNull(splitText[4]);
        }

        [TestCase("")]
        [TestCase("stream from this")]
        [System.Obsolete]
        public void ShouldCreateAStream(string str)
        {
            using var stream = str.GetStream();
            Assert.IsNotNull(stream);
        }

        [Test]
        [System.Obsolete]
        public void ShouldNotCreateAStream()
        {
            string str = null;
            using var stream = str.GetStream();
            Assert.IsNull(stream);
        }

        [TestCase("")]
        [TestCase("stream from this")]
        [System.Obsolete]
        public void ShouldToStreamAString(string str)
        {
            using var stream = str.GetStream();
            Assert.IsNotNull(stream);
        }

        [Test]
        [System.Obsolete]
        public void ShouldNotToStreamAString()
        {
            string str = null;
            using var stream = str.GetStream();
            Assert.IsNull(stream);
        }
    }
}