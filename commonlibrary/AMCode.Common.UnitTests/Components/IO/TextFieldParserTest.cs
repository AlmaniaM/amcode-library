using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.IO;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.TextFieldParserTests
{
    [TestFixture]
    public class TextFieldParserTest
    {
        [Test]
        [TestCase(",", "comma delimited")]
        [TestCase("\t", "tab delimited")]
        public void ShouldParseDelimted(string delimiter, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Col1{delimiter}Col2{delimiter}Col3");
            sb.AppendLine($"A{delimiter}B{delimiter}C");
            sb.AppendLine($"D{delimiter}E{delimiter}F");

            var parsedRows = getParsedRows(sb.ToString(), delimiter).ToList();

            Assert.AreEqual(3, parsedRows.Count);
            var i = 0;
            var j = 0;
            var row = parsedRows[i];
            Assert.AreEqual(3, row.Length, $"{message}: parsedRows[{i}] length");
            Assert.AreEqual("Col1", row[j], $"{message}: parsedRows[{i}] col[{j}]");
            Assert.AreEqual("Col2", row[++j], $"{message}: parsedRows[{i}] col[{j}]");
            Assert.AreEqual("Col3", row[++j], $"{message}: parsedRows[{i}] col[{j}]");

            j = 0;
            row = parsedRows[++i];
            Assert.AreEqual(3, row.Length, $"{message}: parsedRows[{i}] length");
            Assert.AreEqual("A", row[j], $"{message}: parsedRows[{i}] col[{j}]");
            Assert.AreEqual("B", row[++j], $"{message}: parsedRows[{i}] col[{j}]");
            Assert.AreEqual("C", row[++j], $"{message}: parsedRows[{i}] col[{j}]");

            j = 0;
            row = parsedRows[++i];
            Assert.AreEqual(3, row.Length, $"{message}: parsedRows[{i}] length");
            Assert.AreEqual("D", row[j], $"{message}: parsedRows[{i}] col[{j}]");
            Assert.AreEqual("E", row[++j], $"{message}: parsedRows[{i}] col[{j}]");
            Assert.AreEqual("F", row[++j], $"{message}: parsedRows[{i}] col[{j}]");
        }

        private static IEnumerable<string[]> getParsedRows(string content, string delimiter)
        {
            var parsedRows = new List<string[]>();
            using (var streamReader = new StreamReader(content.GetStream()))
            {
                using var parser = new TextFieldParser(streamReader);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);

                while (!parser.EndOfData)
                {
                    parsedRows.Add(parser.ReadFields());
                }
            }

            return parsedRows;
        }
    }
}