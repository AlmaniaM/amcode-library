using AMCode.Sql.Extensions.StringExtensions.Vertica;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Extensions.StringExtensionsVerticaTests
{
    [TestFixture]
    public class StringExtensionsVerticaTest
    {
        [Test]
        public void ShouldReturnAnEmptyStringWhenStringIsNull() => Assert.AreEqual(string.Empty, ((string)null).Sanitize());

        [Test]
        public void ShouldReturnAnEmptyStringWhenStringIsEmpty() => Assert.AreEqual(string.Empty, string.Empty.Sanitize());

        [TestCase("TestString")]
        [TestCase(" ")]
        [TestCase(" 100 ")]
        [TestCase("!! @@ % & () {} + _ - = *")]
        public void ShouldSanitizeStringWithDoubleDollarSignsOnBothSides(string testString) => Assert.AreEqual($"$${testString}$$", testString.Sanitize());
    }
}