using System;
using System.Collections.Generic;
using System.Text;
using AMCode.Data.Extensions;
using AMCode.Data.UnitTests.Extensions.Models;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Extensions.ValueParserTests
{
    [TestFixture]
    public class ValueParserTest
    {
        private ValueParser valueParser;

        [SetUp]
        public void SetUp() => valueParser = new ValueParser();

        [Test]
        public void ShouldParseDateTimeString()
        {
            static string addZeroIfNeeded(int value)
                => value < 10 ? $"0{value}" : value.ToString();

            var expectedDateTime = DateTime.Now;

            var dateTimeStringToParse = new StringBuilder()
                .Append(addZeroIfNeeded(expectedDateTime.Month))
                .Append('/')
                .Append(addZeroIfNeeded(expectedDateTime.Day))
                .Append('/')
                .Append(expectedDateTime.Year)
                .Append(' ')
                .Append(addZeroIfNeeded(expectedDateTime.Hour))
                .Append(':')
                .Append(addZeroIfNeeded(expectedDateTime.Minute))
                .Append(':')
                .Append(addZeroIfNeeded(expectedDateTime.Second))
                .Append('.')
                .Append(expectedDateTime.Millisecond)
                .ToString();

            var parsedDateTime = (DateTime)valueParser.Parse(dateTimeStringToParse, typeof(DateTime));

            Assert.AreEqual(expectedDateTime.ToLongTimeString(), parsedDateTime.ToLongTimeString());
        }

        [Test]
        public void ShouldParseStringToObject()
        {
            var objectString = new StringBuilder()
                .Append("{")
                .Append("\"TestProperty\": \"Test Value\"")
                .Append("}")
                .ToString();

            var testPropertyMock = (TestPropertyMock)valueParser.Parse(objectString, typeof(TestPropertyMock));

            Assert.AreEqual("Test Value", testPropertyMock.TestProperty);
        }

        [Test]
        public void ShouldParseStringToList()
        {
            var objectString = new StringBuilder()
                .Append("[")
                .Append("1,2,3,4,5")
                .Append("]")
                .ToString();

            var integerList = (List<int>)valueParser.Parse(objectString, typeof(List<int>));

            Assert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, integerList);
        }

        [TestCase("1", typeof(int), 1)]
        [TestCase(1, typeof(int), 1)]
        [TestCase("Test Value", typeof(string), "Test Value")]
        [TestCase("a", typeof(char), 'a')]
        [TestCase('a', typeof(char), 'a')]
        [TestCase("9223372036854775807", typeof(long), long.MaxValue)]
        [TestCase(long.MaxValue, typeof(long), long.MaxValue)]
        [TestCase("1.7976931348623157E+308", typeof(double), double.MaxValue)]
        [TestCase(double.MaxValue, typeof(double), double.MaxValue)]
        public void ShouldParseSimpleValues(object valueToParse, Type typeToParseTo, object expectedValue)
            => Assert.AreEqual(expectedValue, valueParser.Parse(valueToParse, typeToParseTo));

        [TestCase("System.Boolean", typeof(bool))]
        [TestCase("System.Char", typeof(char))]
        [TestCase("System.Decimal", typeof(decimal))]
        [TestCase("System.Double", typeof(double))]
        [TestCase("System.Single", typeof(float))]
        [TestCase("System.Int32", typeof(int))]
        [TestCase("System.Int64", typeof(long))]
        [TestCase("System.String", typeof(string))]
        public void ShouldParseValueToType(object valueToParser, Type expectedType)
            => Assert.AreEqual(expectedType, valueParser.Parse(valueToParser, typeof(Type)));
    }
}