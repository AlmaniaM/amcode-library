using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using AMCode.Columns.Builder;
using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.UnitTests.Dynamic.Models;
using AMCode.Data.Extensions;
using AMCode.Data.UnitTests.Extensions.Models;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Extensions.DataReaderExtensionsTests
{
    [TestFixture]
    public class DataReaderExtensionsTest
    {
        private IList<IDataTransformColumnDefinition> columns;
        private readonly IList<IColumnName> columnNames = new List<IColumnName>
        {
            new ColumnName { DisplayName = "Header One", FieldName = "HeaderOne" },
            new ColumnName { DisplayName = "Header Three", FieldName = "HeaderThree" },
            new ColumnName { DisplayName = "Header Two", FieldName = "HeaderTwo" }
        };

        private IList<IDataTransformColumnDefinition> columnsWithProperties;
        private readonly IList<ColumnNameWithProperty> columnNamesWithProperties = new List<ColumnNameWithProperty>
        {
            new ColumnNameWithProperty { DisplayName = "Header One", FieldName = "HeaderOne", PropertyName = "ColumnOne" },
            new ColumnNameWithProperty { DisplayName = "Header Three", FieldName = "HeaderThree", PropertyName = "ColumnThree" },
            new ColumnNameWithProperty { DisplayName = "Header Two", FieldName = "HeaderTwo", PropertyName = "ColumnTwo" }
        };

        [SetUp]
        public void SetUp()
        {
            columns = columnNames.Select((IColumnName columnName, int index) =>
            {
                var prefix = string.Empty;

                if (index == 0)
                {
                    prefix = "$";
                }
                else if (index == 2) // HeaderTwo in the list
                {
                    prefix = "%";
                }

                return new ColumnFactoryBuilder()
                    .ColumnName(columnName)
                    .DataTransformation()
                    .ValueFormatter(new MockValueFormatter(value => $"{prefix}{value}"))
                    .Build();
            }).ToList();

            columnsWithProperties = columnNamesWithProperties.Select((ColumnNameWithProperty columnName, int index) => new ColumnFactoryBuilder()
                    .ColumnName(columnName)
                    .DataTransformation()
                    .PropertyName(columnName.PropertyName)
                    .Build()).ToList();
        }

        [TestCase("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", false, ',')]
        [TestCase("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers_Pipe_Delimited.csv", false, ',')]
        [TestCase("DataReaderExtensionsTest_Five_Rows_Space_In_Headers.csv", true, ',')]
        [TestCase("DataReaderExtensionsTest_Five_Rows_Space_In_Headers_Pipe_Delimited.csv", true, ',')]
        public void ShouldCreateExpandoObjectHeaders(string fileName, bool withSpaces, char delimiter)
        {
            var filePath = LocalTestHelpers.GetFilePath(fileName, TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath, delimiter);

            var expandoList = dataReader.ToExpandoList();

            var space = withSpaces ? " " : string.Empty;

            var expectedHeaders = new List<string> { $"Header{space}One", $"Header{space}Two", $"Header{space}Three" };

            var expandoObjectOne = expandoList[0];

            expectedHeaders.ForEach(expectedHeader => Assert.IsNotNull(expandoObjectOne.GetValue<string>(expectedHeader)));
        }

        [TestCase("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", false, ',')]
        [TestCase("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers_Pipe_Delimited.csv", false, ',')]
        [TestCase("DataReaderExtensionsTest_Five_Rows_Space_In_Headers.csv", true, ',')]
        [TestCase("DataReaderExtensionsTest_Five_Rows_Space_In_Headers_Pipe_Delimited.csv", true, ',')]
        public void ShouldCreateExpandoObjectList(string fileName, bool withSpaces, char delimiter)
        {
            var filePath = LocalTestHelpers.GetFilePath(fileName, TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath, delimiter);

            var expandoList = dataReader.ToExpandoList();

            Assert.AreEqual(5, expandoList.Count);

            var space = withSpaces ? " " : string.Empty;

            var expectedHeaders = new List<string> { $"Header{space}One", $"Header{space}Two", $"Header{space}Three" };

            for (var i = 0; i < expandoList.Count; i++)
            {
                var expando = expandoList[i];

                Assert.AreEqual($"{getValue(i, 1, 3)}", expando.GetValue<string>(expectedHeaders[0]));
                Assert.AreEqual($"{getValue(i, 2, 3)}", expando.GetValue<string>(expectedHeaders[1]));
                Assert.AreEqual($"{getValue(i, 3, 3)}", expando.GetValue<string>(expectedHeaders[2]));
            }
        }

        [Test]
        public void ShouldCreateExpandoObjectListWithFormattingColumns()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var expandoList = dataReader.ToExpandoList(columns);

            Assert.AreEqual(5, expandoList.Count);

            var expectedHeaders = new List<string> { $"HeaderOne", $"HeaderTwo", $"HeaderThree" };

            for (var i = 0; i < expandoList.Count; i++)
            {
                var expando = expandoList[i];

                Assert.AreEqual($"${getValue(i, 1, 3)}", expando.GetValue<string>(expectedHeaders[0]));
                Assert.AreEqual($"%{getValue(i, 2, 3)}", expando.GetValue<string>(expectedHeaders[1]));
                Assert.AreEqual($"{getValue(i, 3, 3)}", expando.GetValue<string>(expectedHeaders[2]));
            }
        }

        [Test]
        public void ShouldCreateExpandoObjectListWithDifferentPropertyNames()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var expandoList = dataReader.ToExpandoList(columnsWithProperties);

            Assert.AreEqual(5, expandoList.Count);

            var expectedHeaders = new List<string> { $"ColumnOne", $"ColumnTwo", $"ColumnThree" };

            for (var i = 0; i < expandoList.Count; i++)
            {
                var expando = expandoList[i];

                Assert.AreEqual(getValue(i, 1, 3), expando.GetValue<string>(expectedHeaders[0]));
                Assert.AreEqual(getValue(i, 2, 3), expando.GetValue<string>(expectedHeaders[1]));
                Assert.AreEqual(getValue(i, 3, 3), expando.GetValue<string>(expectedHeaders[2]));
            }
        }

        [Test]
        public void ShouldThrowOperationCancelledExceptionInExpandoListWithColumns()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            Assert.Throws<OperationCanceledException>(() => dataReader.ToExpandoList(columns, tokenSource.Token));
        }

        [Test]
        public void ShouldThrowOperationCancelledExceptionExpandoList()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            Assert.Throws<OperationCanceledException>(() => dataReader.ToExpandoList(tokenSource.Token));
        }

        [Test]
        public void ShouldCreateObjectTypeList()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var objectList = dataReader.ToList<DataReaderExtensionsMock>();

            Assert.AreEqual(5, objectList.Count);

            for (var i = 0; i < objectList.Count; i++)
            {
                var obj = objectList[i];

                Assert.AreEqual($"{getValue(i, 1, 3)}", obj.HeaderOne);
                Assert.AreEqual($"{getValue(i, 2, 3)}", obj.HeaderTwo);
                Assert.AreEqual($"{getValue(i, 3, 3)}", obj.HeaderThree);
            }
        }

        [Test]
        public void ShouldCreateObjectTypeListWithFormattingColumns()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var objectList = dataReader.ToList<DataReaderExtensionsMock>(columns);

            Assert.AreEqual(5, objectList.Count);

            for (var i = 0; i < objectList.Count; i++)
            {
                var obj = objectList[i];

                Assert.AreEqual($"${getValue(i, 1, 3)}", obj.HeaderOne);
                Assert.AreEqual($"%{getValue(i, 2, 3)}", obj.HeaderTwo);
                Assert.AreEqual($"{getValue(i, 3, 3)}", obj.HeaderThree);
            }
        }

        [Test]
        public void ShouldCreateObjectTypeListWithPropertyNameColumns()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var objectList = dataReader.ToList<DataReaderExtensionsColumnPropertyMock>(columnsWithProperties);

            Assert.AreEqual(5, objectList.Count);

            for (var i = 0; i < objectList.Count; i++)
            {
                var obj = objectList[i];

                Assert.AreEqual(getValue(i, 1, 3), obj.ColumnOne);
                Assert.AreEqual(getValue(i, 2, 3), obj.ColumnTwo);
                Assert.AreEqual(getValue(i, 3, 3), obj.ColumnThree);
            }
        }

        [Test]
        public void ShouldThrowOperationCancelledExceptionInToListWithColumns()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            Assert.Throws<OperationCanceledException>(() => dataReader.ToList<DataReaderExtensionsMock>(columns, tokenSource.Token));
        }

        [Test]
        public void ShouldThrowOperationCancelledExceptionToList()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Five_Rows_No_Space_In_Headers.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            Assert.Throws<OperationCanceledException>(() => dataReader.ToList<DataReaderExtensionsMock>(tokenSource.Token));
        }

        [Test]
        public void ShouldCreateObjectTypeListWithObjectProperties()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_One_Row_Object_Values.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath, '|');

            var objectList = dataReader.ToList<DataReaderExtensionsColumnPropertyObjecMock>(columnsWithProperties);

            Assert.AreEqual(1, objectList.Count);

            var obj = objectList[0];

            Assert.AreEqual("1", obj.ColumnOne.Value);

            obj.ColumnTwo.ForEach((int value, int index) => Assert.AreEqual(index + 1, value));

            obj.ColumnThree.ForEach((string value, int index) => Assert.AreEqual($"Test Value {index + 1}", value));
        }

        [Test]
        public void ShouldCreateObjectTypeListWithTypeProperty()
        {
            var filePath = LocalTestHelpers.GetFilePath("DataReaderExtensionsTest_Eight_Rows_Type_Values.csv", TestContext.CurrentContext);
            IDataReader dataReader = new CSVDataReader(filePath);

            var objectList = dataReader.ToList<DataReaderExtensionsColumnTypePropertyMock>(columnsWithProperties);

            Assert.AreEqual(8, objectList.Count);

            var booleanRow = objectList.ElementAt(0);
            var charRow = objectList.ElementAt(1);
            var decimalRow = objectList.ElementAt(2);
            var doubleRow = objectList.ElementAt(3);
            var floatRow = objectList.ElementAt(4);
            var intRow = objectList.ElementAt(5);
            var longRow = objectList.ElementAt(6);
            var stringRow = objectList.ElementAt(7);

            Assert.AreEqual(typeof(bool), booleanRow.DataType);
            Assert.AreEqual(typeof(char), charRow.DataType);
            Assert.AreEqual(typeof(decimal), decimalRow.DataType);
            Assert.AreEqual(typeof(double), doubleRow.DataType);
            Assert.AreEqual(typeof(float), floatRow.DataType);
            Assert.AreEqual(typeof(int), intRow.DataType);
            Assert.AreEqual(typeof(long), longRow.DataType);
            Assert.AreEqual(typeof(string), stringRow.DataType);
        }

        /// <summary>
        /// This will create the value for each row/column. First row must start with zero (0).
        /// NOTE: the column values must be sequential such as  row #1: 1, 2, 3, row #2: 4, 5, 6, etc...
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        private string getValue(int row, int column, int columnCount) => $"{(row * columnCount) + column}";
    }

    /// <summary>
    /// Mock value formatter for testing.
    /// </summary>
    public class MockValueFormatter : IValueFormatter
    {
        private readonly Func<string, string> _formatter;

        public MockValueFormatter(Func<string, string> formatter)
        {
            _formatter = formatter;
        }

        public string Format(object value)
        {
            return _formatter(value?.ToString() ?? string.Empty);
        }

        public object FormatToObject(object value)
        {
            return _formatter(value?.ToString() ?? string.Empty);
        }
    }
}