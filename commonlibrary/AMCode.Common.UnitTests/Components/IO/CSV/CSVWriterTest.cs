using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO;
using AMCode.Common.IO.CSV;
using AMCode.Common.IO.CSV.Models;
using AMCode.Common.UnitTests.IO.CSV.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.CSV.CsvWriterTests
{
    [TestFixture]
    public class CSVWriterTest
    {
        private TestHelper testHelper;

        [SetUp]
        public void SetUp() => testHelper = new TestHelper();

        [TearDown]
        public void Cleanup()
        {
            var workDirectoryPath = testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext);

            if (Directory.Exists(workDirectoryPath))
            {
                var files = Directory.GetFiles(workDirectoryPath);
                files
                    .Where(filePath => !Path.GetFileName(new FileInfo(filePath).Name).Equals("tmp.txt"))
                    .ToList()
                    .ForEach(filePath => File.Delete(filePath));
            }
        }

        [Test]
        public void ShouldCreateCsvFile()
        {
            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CrateCSVFile.csv");
            new CSVWriter().CreateCsv(expandoList, saveFilePath);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileWithDifferentHeaders()
        {
            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileWithDifferentHeaders.csv");
            new CSVWriter().CreateCsv(expandoList, saveFilePath, new List<string> { "Header One", "Header Two", "Header Three" });

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("Header One"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("Header One"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("Header Two"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("Header Two"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("Header Three"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("Header Three"));
        }

        [TestCase(",")]
        [TestCase("|")]
        [TestCase("&")]
        [TestCase("#")]
        [TestCase("*")]
        [TestCase("%")]
        [TestCase("!")]
        [TestCase("delimiter")]
        public void ShouldCreateCsvFileWithDifferentDelimiters(string delimiter)
        {
            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileWithDifferentDelimiters.csv");
            new CSVWriter().CreateCsv(expandoList, saveFilePath, delimiter);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath, delimiter);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileWithQuotes()
        {
            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileWithQuotes.csv");
            new CSVWriter().CreateCsv(expandoList, saveFilePath, ",", QuoteOption.AddQuotes);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath, ",", false);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("\"1\"", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("\"4\"", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("\"2\"", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("\"5\"", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("\"3\"", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("\"6\"", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileWithAutoQuotes()
        {
            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows_Delimiters_In_Cells.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath, ",", true);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileWithAutoQuotes.csv");
            new CSVWriter().CreateCsv(expandoList, saveFilePath, ",", QuoteOption.Auto);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath, ",", true);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1, 2", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("5,6,7", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual(string.Empty, savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileWithColumnMappings()
        {
            var headerNamesCaseSesitive = new Dictionary<string, string>
            {
                ["HeaderOne"] = "Header One",
                ["HeaderTwo"] = "Header Two",
                ["HeaderThree"] = "Header Three"
            };

            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileWithColumnMappings.csv");
            new CSVWriter().CreateCsv(expandoList, saveFilePath, ",", QuoteOption.Auto, headerNamesCaseSesitive);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("Header One"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("Header One"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("Header Two"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("Header Two"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("Header Three"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("Header Three"));
        }

        [Test]
        public void ShouldCreateCsvFileWithColumnMappingsWithCustomHeaders()
        {
            var headerNamesCaseSesitive = new Dictionary<string, string>
            {
                ["Header1"] = "Header One",
                ["Header2"] = "Header Two",
                ["Header3"] = "Header Three"
            };

            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(
                testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext),
                "CSVWriterTest_CreateCsvFileWithColumnMappingsWithCustomHeaders.csv"
            );
            new CSVWriter().CreateCsv(expandoList, saveFilePath, ",", QuoteOption.Auto, headerNamesCaseSesitive, new List<string>() { "Header1", "Header2", "Header3" });

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("Header One"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("Header One"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("Header Two"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("Header Two"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("Header Three"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("Header Three"));
        }

        [Test]
        public void ShouldCreateCsvFileWithCustomHeaders()
        {
            var filePath = testHelper.GetFilePath("CSVWriterTest_Two_Rows.csv", TestContext.CurrentContext);
            var expandoList = new CSVReader().GetExpandoList(filePath);

            Assert.AreEqual(2, expandoList.Count);

            var saveFilePath = Path.Combine(
                testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext),
                "CSVWriterTest_CreateCsvFileWithColumnMappingsWithCustomHeaders.csv"
            );
            new CSVWriter().CreateCsv(expandoList, saveFilePath, ",", QuoteOption.Auto, null, new List<string>() { "Header1", "Header2", "Header3" });

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("Header1"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("Header1"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("Header2"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("Header2"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("Header3"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("Header3"));
        }

        [Test]
        public void ShouldCreateCsvFileFromObjectTypes()
        {
            var stringMockObjectList = new List<FileOneStringMock>
            {
                new FileOneStringMock
                {
                    HeaderOne = "1",
                    HeaderTwo = "2",
                    HeaderThree = "3"
                },
                new FileOneStringMock
                {
                    HeaderOne = "4",
                    HeaderTwo = "5",
                    HeaderThree = "6"
                }
            };

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileFromObjectTypes.csv");
            new CSVWriter().CreateCsv(stringMockObjectList, saveFilePath);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [TestCase(",")]
        [TestCase("|")]
        [TestCase("&")]
        [TestCase("#")]
        [TestCase("*")]
        [TestCase("%")]
        [TestCase("!")]
        [TestCase("delimiter")]
        public void ShouldCreateCsvFileFromObjectTypesWithCustomDelimiters(string delimiter)
        {
            var stringMockObjectList = new List<FileOneStringMock>
            {
                new FileOneStringMock
                {
                    HeaderOne = "1",
                    HeaderTwo = "2",
                    HeaderThree = "3"
                },
                new FileOneStringMock
                {
                    HeaderOne = "4",
                    HeaderTwo = "5",
                    HeaderThree = "6"
                }
            };

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileFromObjectTypesWithCustomDelimiters.csv");
            new CSVWriter().CreateCsv(stringMockObjectList, saveFilePath, delimiter);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath, delimiter);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("6", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileFromObjectTypesWithAddQuoteOption()
        {
            var stringMockObjectList = new List<FileOneStringMock>
            {
                new FileOneStringMock
                {
                    HeaderOne = "1",
                    HeaderTwo = "2",
                    HeaderThree = "3"
                },
                new FileOneStringMock
                {
                    HeaderOne = "4",
                    HeaderTwo = "5",
                    HeaderThree = "6"
                }
            };

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileFromObjectTypesWithAddQuoteOption.csv");
            new CSVWriter().CreateCsv(stringMockObjectList, saveFilePath, ",", QuoteOption.AddQuotes);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath, ",", false);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("\"1\"", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("\"4\"", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("\"2\"", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("\"5\"", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("\"3\"", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("\"6\"", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileFromObjectTypesWithAutoQuoteOption()
        {
            var stringMockObjectList = new List<FileOneStringMock>
            {
                new FileOneStringMock
                {
                    HeaderOne = "1,2",
                    HeaderTwo = "2",
                    HeaderThree = "3"
                },
                new FileOneStringMock
                {
                    HeaderOne = "4",
                    HeaderTwo = "5",
                    HeaderThree = "6,7,8"
                }
            };

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileFromObjectTypesWithAutoQuoteOption.csv");
            new CSVWriter().CreateCsv(stringMockObjectList, saveFilePath, ",", QuoteOption.Auto);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1,2", savedExpandoList[0].GetValue<string>("HeaderOne"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("HeaderOne"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("HeaderTwo"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("HeaderThree"));
            Assert.AreEqual("6,7,8", savedExpandoList[1].GetValue<string>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateCsvFileFromObjectTypesAlternateHeaders()
        {
            var stringMockObjectList = new List<FileOneStringMock>
            {
                new FileOneStringMock
                {
                    HeaderOne = "1,2",
                    HeaderTwo = "2",
                    HeaderThree = "3"
                },
                new FileOneStringMock
                {
                    HeaderOne = "4",
                    HeaderTwo = "5",
                    HeaderThree = "6,7,8"
                }
            };
            var headerNamesCaseSesitive = new Dictionary<string, string>
            {
                ["HeaderOne"] = "Header One",
                ["HeaderTwo"] = "Header Two",
                ["HeaderThree"] = "Header Three"
            };

            var saveFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "CSVWriterTest_CreateCsvFileFromObjectTypesAlternateHeaders.csv");
            new CSVWriter().CreateCsv(stringMockObjectList, saveFilePath, ",", QuoteOption.Auto, headerNamesCaseSesitive);

            var savedExpandoList = new CSVReader().GetExpandoList(saveFilePath);

            Assert.AreEqual(2, savedExpandoList.Count);
            Assert.AreEqual("1,2", savedExpandoList[0].GetValue<string>("Header One"));
            Assert.AreEqual("4", savedExpandoList[1].GetValue<string>("Header One"));
            Assert.AreEqual("2", savedExpandoList[0].GetValue<string>("Header Two"));
            Assert.AreEqual("5", savedExpandoList[1].GetValue<string>("Header Two"));
            Assert.AreEqual("3", savedExpandoList[0].GetValue<string>("Header Three"));
            Assert.AreEqual("6,7,8", savedExpandoList[1].GetValue<string>("Header Three"));
        }
    }
}