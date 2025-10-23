using AMCode.Common.Testing.IO;
using DlCommonTestingLibrary.UnitTests.IO.CsvDataReaderTests.Models;
using NUnit.Framework;

namespace DlCommonTestingLibrary.UnitTests.IO.CsvDataReaderTests
{
    [TestFixture]
    public class CSVDataReaderTest
    {
        [TestCase("CSVDataReaderTest_Comma_Delimited_Five_Rows.csv", ',')]
        [TestCase("CSVDataReaderTest_Tab_Delimited_Five_Rows.csv", '\t')]
        [TestCase("CSVDataReaderTest_Pipe_Delimited_Five_Rows.csv", '|')]
        public void ShouldReadHeaders(string fileName, char delimiter)
        {
            var filePath = LocalTestHelpers.GetFilePath(fileName, TestContext.CurrentContext);
            var dataReader = new CSVDataReader(filePath, delimiter);

            Assert.AreEqual("HeaderOne", dataReader.Header[0]);
            Assert.AreEqual("HeaderTwo", dataReader.Header[1]);
            Assert.AreEqual("HeaderThree", dataReader.Header[2]);
        }

        [TestCase("CSVDataReaderTest_Comma_Delimited_Five_Rows.csv", ',')]
        [TestCase("CSVDataReaderTest_Tab_Delimited_Five_Rows.csv", '\t')]
        [TestCase("CSVDataReaderTest_Pipe_Delimited_Five_Rows.csv", '|')]
        public void ShouldCreateIDataReader(string fileName, char delimiter)
        {
            var filePath = LocalTestHelpers.GetFilePath(fileName, TestContext.CurrentContext);
            var dataReader = new CSVDataReader(filePath, delimiter);

            var i = 0;
            while (dataReader.Read())
            {
                var row = dataReader.Line;

                Assert.AreEqual(getValue(i, 1, 3), row[0]);
                Assert.AreEqual(getValue(i, 2, 3), row[1]);
                Assert.AreEqual(getValue(i, 3, 3), row[2]);

                i++;
            }
        }

        [Test]
        public void ShouleReadHeadersWithSpacesInTheNames()
        {
            var filePath = LocalTestHelpers.GetFilePath("CSVDataReaderTest_Five_Rows_Space_In_Headers.csv", TestContext.CurrentContext);
            var dataReader = new CSVDataReader(filePath);

            Assert.AreEqual("Header One", dataReader.Header[0]);
            Assert.AreEqual("Header Two", dataReader.Header[1]);
            Assert.AreEqual("Header Three", dataReader.Header[2]);
        }

        [Test]
        public void ShouleReadFileWithNoHeaders()
        {
            var filePath = LocalTestHelpers.GetFilePath("CSVDataReaderTest_Five_Rows_No_Headers.csv", TestContext.CurrentContext);
            var dataReader = new CSVDataReader(filePath, ',', false);

            Assert.AreEqual("COL_0", dataReader.Header[0]);
            Assert.AreEqual("COL_1", dataReader.Header[1]);
            Assert.AreEqual("COL_2", dataReader.Header[2]);
        }

        [Test]
        public void ShouldReadEmptyValuesAsNulls()
        {
            var filePath = LocalTestHelpers.GetFilePath("CSVDataReaderTest_Comma_Delimited_Five_Rows_With_Empty_Values.csv", TestContext.CurrentContext);
            var dataReader = new CSVDataReader(filePath, ',', true, true);

            var i = 0;
            while (dataReader.Read())
            {
                var row = dataReader.Line;

                var colOneValue = i % 3 == 0 ? null : getValue(i, 1, 3);
                var colTwoValue = i % 3 == 1 ? null : getValue(i, 2, 3);
                var colThreeValue = i % 3 == 2 ? null : getValue(i, 3, 3);

                Assert.AreEqual(colOneValue, row[0]);
                Assert.AreEqual(colTwoValue, row[1]);
                Assert.AreEqual(colThreeValue, row[2]);

                ++i;
            }
        }

        /// <summary>
        /// This will create the value for each row/column. First row must start with zero (0).
        /// NOTE: the column values must be sequencial such as  row #1: 1, 2, 3, row #2: 4, 5, 6, etc...
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        private string getValue(int row, int column, int columnCount) => $"{(row * columnCount) + column}";
    }
}