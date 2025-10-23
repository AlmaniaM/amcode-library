using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Extensions.EnumerableExtensions.Vertica;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Extensions.EnumerableExtensionsVerticaaTests
{
    [TestFixture]
    public class StringExtensionsVerticaTest
    {
        [Test]
        public void ShouldReturnAnEmptyEnumerableWhenListIsNull() => Assert.AreEqual(Enumerable.Empty<string>(), ((IEnumerable<string>)null).Sanitize());

        [Test]
        public void ShouldReturnAnEmptyStringWhenStringIsEmpty() => Assert.AreEqual(string.Empty, Enumerable.Empty<string>().Sanitize());

        [Test]
        public void ShouldSanitizeStringArrayStringsWithDoubleDollarSignsOnBothSides()
        {
            var stringArray = new string[] { "TestString", "  ", " 5000 ", " ! @ # % ^ & * ( ) _ - + =" };

            var sanitizedStrings = stringArray.Sanitize();

            for (var i = 0; i < stringArray.Length; i++)
            {
                Assert.AreEqual($"$${stringArray[i]}$$", sanitizedStrings.ElementAt(i));
            }
        }

        [Test]
        public void ShouldSanitizeStringListStringsWithDoubleDollarSignsOnBothSides()
        {
            var stringArray = new List<string> { "TestString", "  ", " 5000 ", " ! @ # % ^ & * ( ) _ - + =" };

            var sanitizedStrings = stringArray.Sanitize();

            for (var i = 0; i < stringArray.Count; i++)
            {
                Assert.AreEqual($"$${stringArray[i]}$$", sanitizedStrings.ElementAt(i));
            }
        }
    }
}