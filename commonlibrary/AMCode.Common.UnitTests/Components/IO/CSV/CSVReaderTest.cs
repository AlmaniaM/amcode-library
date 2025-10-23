using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV;
using AMCode.Common.IO.CSV.Exceptions;
using AMCode.Common.UnitTests.IO.CSV.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.CSV.CsvReaderTests
{
    [TestFixture]
    public class CSVReaderTest
    {
        private TestHelper testHelper;

        [SetUp]
        public void SetUp() => testHelper = new TestHelper();

        [Test]
        public void ShouldReadEmptyCsvFile()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_No_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);
            Assert.AreEqual(0, expandoList.Count);
        }

        [Test]
        public void ShouldReadCsvFileIntoExpandoList()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_Space_In_Headers.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(1, expandoList.Count);

            Assert.AreEqual("1", expandoList[0].GetValue<string>("Header One"));
            Assert.AreEqual("2", expandoList[0].GetValue<string>("Header Two"));
            Assert.AreEqual("3", expandoList[0].GetValue<string>("Header Three"));

            Assert.AreEqual(1, expandoList[0].GetValue<int>("Header One"));
            Assert.AreEqual(2, expandoList[0].GetValue<int>("Header Two"));
            Assert.AreEqual(3, expandoList[0].GetValue<int>("Header Three"));
        }

        [Test]
        public void ShouldReadCsvFileIntoExpandoListWithNowHeaders()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_No_Headers.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader(false).GetExpandoList(filePath);

            Assert.AreEqual(1, expandoList.Count);

            Assert.AreEqual("1", expandoList[0].GetValue<string>("Header1"));
            Assert.AreEqual("2", expandoList[0].GetValue<string>("Header2"));
            Assert.AreEqual("3", expandoList[0].GetValue<string>("Header3"));

            Assert.AreEqual(1, expandoList[0].GetValue<int>("Header1"));
            Assert.AreEqual(2, expandoList[0].GetValue<int>("Header2"));
            Assert.AreEqual(3, expandoList[0].GetValue<int>("Header3"));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldReadCsvFileIntoExpandoListWithCommasInColumns(bool removeQuotes)
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_Space_In_Headers_Commas_In_Data.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath, ",", removeQuotes);

            Assert.AreEqual(1, expandoList.Count);

            var columnValue1 = removeQuotes ? "TestValue1, TestValue2..." : "\"TestValue1, TestValue2...\"";
            var columnValue2 = removeQuotes ? "TestValues,,," : "\"TestValues,,,\"";

            Assert.AreEqual(columnValue1, expandoList[0].GetValue<string>("Header One"));
            Assert.AreEqual("2", expandoList[0].GetValue<string>("Header Two"));
            Assert.AreEqual(columnValue2, expandoList[0].GetValue<string>("Header Three"));
        }

        [Test]
        public void ShouldReadCsvFileIntoExpandoListWithLastElementAsEmpty()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_Space_In_Headers_No_Data_At_End.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(1, expandoList.Count);

            Assert.AreEqual(string.Empty, expandoList[0].GetValue<string>("Header Three"));
        }

        [Test]
        public void ShouldReadCsvFileIntoObjectWithIntPropertyTypes()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_No_Space_In_Headers.csv", TestContext.CurrentContext);
            var intMock = new CSVReader().GetList<FileOneIntMock>(filePath, ",", null, true);

            Assert.AreEqual(1, intMock[0].HeaderOne);
            Assert.AreEqual(2, intMock[0].HeaderTwo);
            Assert.AreEqual(3, intMock[0].HeaderThree);
        }

        [Test]
        public void ShouldReadCsvFileIntoObjectWithStringPropertyTypes()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_No_Space_In_Headers.csv", TestContext.CurrentContext);
            var intMock = new CSVReader().GetList<FileOneStringMock>(filePath, ",", null, true);

            Assert.AreEqual("1", intMock[0].HeaderOne);
            Assert.AreEqual("2", intMock[0].HeaderTwo);
            Assert.AreEqual("3", intMock[0].HeaderThree);
        }

        [Test]
        public void ShouldReadCsvFileWithNoHeadersIntoObjectList()
        {
            var numberNames = new Dictionary<int, string>
            {
                [0] = "One",
                [1] = "Two",
                [2] = "Three"
            };

            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_No_Headers.csv", TestContext.CurrentContext);
            var intMock = new CSVReader(false, (index) => $"Header{numberNames[index]}").GetList<FileOneIntMock>(filePath, ",", null, true);

            Assert.AreEqual(1, intMock[0].HeaderOne);
            Assert.AreEqual(2, intMock[0].HeaderTwo);
            Assert.AreEqual(3, intMock[0].HeaderThree);
        }

        [Test]
        public void ShouldReadCsvFileIntoExpandoObjectListWithLookupColumns()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_Space_In_Headers.csv", TestContext.CurrentContext);
            var headerNamesCaseSesitive = new Dictionary<string, string>
            {
                ["Header One"] = "HeaderOne",
                ["Header Two"] = "HeaderTwo",
                ["Header Three"] = "HeaderThree"
            };

            var expandoList = new CSVReader().GetExpandoList(filePath, ",", false, headerNamesCaseSesitive);

            Assert.AreEqual("1", expandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", expandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", expandoList[0].GetValue<string>("HeaderThree"));

            var headerNamesCaseInSensitive = new Dictionary<string, string>
            {
                ["Header One"] = "HeaderOne",
                ["header two"] = "HeaderTwo",
                ["Header three"] = "HeaderThree"
            };

            var expandoListCaseInsensitive = new CSVReader().GetExpandoList(filePath, ",", false, headerNamesCaseInSensitive, true);

            Assert.AreEqual(1, expandoListCaseInsensitive[0].GetValue<int>("HeaderOne"));
            Assert.AreEqual(2, expandoListCaseInsensitive[0].GetValue<int>("HeaderTwo"));
            Assert.AreEqual(3, expandoListCaseInsensitive[0].GetValue<int>("HeaderThree"));
        }

        [Test]
        public void ShouldReadCsvFileIntoIntPropertiesWithLookupColumns()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_Space_In_Headers.csv", TestContext.CurrentContext);

            var headerNamesCaseSesitive = new Dictionary<string, string>
            {
                ["Header One"] = "HeaderOne",
                ["Header Two"] = "HeaderTwo",
                ["Header Three"] = "HeaderThree"
            };

            var intMockListCaseSensitive = new CSVReader().GetList<FileOneIntMock>(filePath, ",", headerNamesCaseSesitive);

            Assert.AreEqual(1, intMockListCaseSensitive[0].HeaderOne);
            Assert.AreEqual(2, intMockListCaseSensitive[0].HeaderTwo);
            Assert.AreEqual(3, intMockListCaseSensitive[0].HeaderThree);

            var headerNamesCaseInSensitive = new Dictionary<string, string>
            {
                ["Header One"] = "HeaderOne",
                ["header two"] = "HeaderTwo",
                ["Header three"] = "HeaderThree"
            };

            var intMockListCaseInsensitive = new CSVReader().GetList<FileOneStringMock>(filePath, ",", headerNamesCaseInSensitive, true);

            Assert.AreEqual("1", intMockListCaseInsensitive[0].HeaderOne);
            Assert.AreEqual("2", intMockListCaseInsensitive[0].HeaderTwo);
            Assert.AreEqual("3", intMockListCaseInsensitive[0].HeaderThree);
        }

        [Test]
        public void ShouleReadCsvHeaderNames()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_One_Row_Space_In_Headers.csv", TestContext.CurrentContext);
            var headers = new CSVReader().GetHeaders(filePath);

            Assert.AreEqual(3, headers.Count);
            Assert.AreEqual("Header One", headers[0]);
            Assert.AreEqual("Header Two", headers[1]);
            Assert.AreEqual("Header Three", headers[2]);
        }

        [Test]
        public void ShouleReadCsvHeaderNamesAsEmptyList()
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_No_Rows.csv", TestContext.CurrentContext);
            var headers = new CSVReader().GetHeaders(filePath);

            Assert.AreEqual(0, headers.Count);
        }

        [TestCase("HeaderOne", new string[] { "1", "4", "7", "10", "13", "16", "19", "22", "25", "28" })]
        [TestCase("HeaderTwo", new string[] { "2", "5", "8", "11", "14", "17", "20", "23", "26", "29" })]
        [TestCase("HeaderThree", new string[] { "3", "6", "9", "12", "15", "18", "21", "24", "27", "30" })]
        public void ShouldGetColumnValuesForColumn(string columnName, string[] expectedValues)
        {
            var filePath = testHelper.GetFilePath("CSVReaderTest_10_Rows_No_Space_In_Headers_Comma_Delimited.csv", TestContext.CurrentContext);
            var values = new CSVReader().GetColumnValues(filePath, columnName);

            Assert.AreEqual(values.ToArray(), expectedValues);
        }

        [TestCase("CSVReaderTest_10_Rows_No_Space_In_Headers_Comma_Delimited.csv", "HeaderOne", new string[] { "1", "4", "7", "10", "13", "16", "19", "22", "25", "28" }, ",")]
        [TestCase("CSVReaderTest_10_Rows_No_Space_In_Headers_Comma_Delimited.csv", "HeaderTwo", new string[] { "2", "5", "8", "11", "14", "17", "20", "23", "26", "29" }, ",")]
        [TestCase("CSVReaderTest_10_Rows_No_Space_In_Headers_Comma_Delimited.csv", "HeaderThree", new string[] { "3", "6", "9", "12", "15", "18", "21", "24", "27", "30" }, ",")]
        [TestCase("CSVReaderTest_10_Rows_No_Space_In_Headers_Pipe_Delimited.csv", "HeaderOne", new string[] { "1", "4", "7", "10", "13", "16", "19", "22", "25", "28" }, "|")]
        [TestCase("CSVReaderTest_10_Rows_No_Space_In_Headers_Pipe_Delimited.csv", "HeaderTwo", new string[] { "2", "5", "8", "11", "14", "17", "20,30", "23", "26", "29" }, "|")]
        [TestCase("CSVReaderTest_10_Rows_No_Space_In_Headers_Pipe_Delimited.csv", "HeaderThree", new string[] { "3", "6", "9", "12", "15", "18", "21", "24", "27", "30, 20" }, "|")]
        public void ShouldGetColumnValuesForColumnWithDifferentDelimiters(string fileName, string columnName, string[] expectedValues, string delimiter)
        {
            var filePath = testHelper.GetFilePath(fileName, TestContext.CurrentContext);
            var values = new CSVReader().GetColumnValues(filePath, columnName, delimiter);

            Assert.AreEqual(values.ToArray(), expectedValues);
        }

        [Test]
        public void ShouldThrowDelimiterNotProvidedExceptionForGetColumnValues()
        {
            Assert.Throws<DelimiterNotProvidedException>(() => new CSVReader().GetColumnValues(string.Empty, string.Empty, string.Empty));
            Assert.Throws<DelimiterNotProvidedException>(() => new CSVReader().GetColumnValues(string.Empty, string.Empty, null));
        }

        [Test]
        public void ShouldThrowDelimiterNotProvidedExceptionForGetExpandoList()
        {
            Assert.Throws<DelimiterNotProvidedException>(() => new CSVReader().GetExpandoList(string.Empty, string.Empty));
            Assert.Throws<DelimiterNotProvidedException>(() => new CSVReader().GetExpandoList(string.Empty, null));
        }

        [Test]
        public void ShouldThrowDelimiterNotProvidedExceptionForGetList()
        {
            Assert.Throws<DelimiterNotProvidedException>(() => new CSVReader().GetList<FileOneIntMock>(string.Empty, string.Empty));
            Assert.Throws<DelimiterNotProvidedException>(() => new CSVReader().GetList<FileOneIntMock>(string.Empty, null));
        }
    }
}