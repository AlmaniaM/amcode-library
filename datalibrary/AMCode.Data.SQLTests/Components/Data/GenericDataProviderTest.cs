using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Data.SQLTests.Data.DataProviderTests.MockFiles;
using AMCode.Data.SQLTests.Data.GenericDataProviderTests.Models;
using AMCode.Data.SQLTests.Globals;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Data.GenericDataProviderTests
{
    [TestFixture]
    public class GenericDataProviderTest
    {
        [Test]
        public async Task ShouldRunMoreThanOneQueryOnOneConnection()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            for (var i = 0; i < 5; i++)
            {
                var objectList = await dataProvider.GetListOfAsync<HeaderNumbersObject>("SELECT * FROM AMCode.Data.tbl_DataProvider_Static_Data order by HeaderOne;");
                Assert.AreEqual(5, objectList.Count);
            }
        }

        [Test]
        public async Task ShouldRunMoreThanOneQueryOnOneConnectionWithCustomConnection()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            using var connection = MoqProvider.CreateDbConnection();

            for (var i = 0; i < 5; i++)
            {
                var objectList = await dataProvider.GetListOfAsync<HeaderNumbersObject>("SELECT * FROM AMCode.Data.tbl_DataProvider_Static_Data order by HeaderOne;", connection);
                Assert.AreEqual(5, objectList.Count);
            }
        }

        [Test]
        public async Task ShouldGetListOfTypeWithCorrectData()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            var objectList = await dataProvider.GetListOfAsync<HeaderNumbersObject>("SELECT * FROM AMCode.Data.tbl_DataProvider_Static_Data order by HeaderOne;");

            Assert.AreEqual(5, objectList.Count);

            static string createCellValue(int row, int column) => $"Row {row} Value {column}";

            objectList.ForEach((HeaderNumbersObject obj, int i) =>
            {
                var row = i + 1;

                Assert.AreEqual(createCellValue(row, 1), obj.HeaderOne);
                Assert.AreEqual(createCellValue(row, 2), obj.HeaderTwo);
                Assert.AreEqual(createCellValue(row, 3), obj.HeaderThree);
                Assert.AreEqual(createCellValue(row, 4), obj.HeaderFour);
                Assert.AreEqual(createCellValue(row, 5), obj.HeaderFive);
            });
        }

        [Test]
        public async Task ShouldGetListOfTypeWithNulls()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            var objectList = await dataProvider.GetListOfAsync<HeaderNumbersObject>("SELECT * FROM AMCode.Data.tbl_DataProvider_Static_Data_With_Nulls order by HeaderFive;");

            Assert.AreEqual(5, objectList.Count);

            static string createCellValue(int row, int column) => $"Row {row} Value {column}";

            var columnCount = 5;

            objectList.ForEach((HeaderNumbersObject obj, int i) =>
            {
                var row = i + 1;

                var colOneValue = i % columnCount == 0 ? null : createCellValue(row, 1);
                var colTwoValue = i % columnCount == 1 ? null : createCellValue(row, 2);
                var colThreeValue = i % columnCount == 2 ? null : createCellValue(row, 3);
                var colFourValue = i % columnCount == 3 ? null : createCellValue(row, 4);
                var colFiveValue = i % columnCount == 4 ? null : createCellValue(row, 5);

                Assert.AreEqual(colOneValue, obj.HeaderOne);
                Assert.AreEqual(colTwoValue, obj.HeaderTwo);
                Assert.AreEqual(colThreeValue, obj.HeaderThree);
                Assert.AreEqual(colFourValue, obj.HeaderFour);
                Assert.AreEqual(colFiveValue, obj.HeaderFive);
            });
        }

        [Test]
        public async Task ShouldGetListOfTypeWithDifferentDataTypes()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            var objectList = await dataProvider.GetListOfAsync<HeaderNamesObject>("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderInteger;");

            Assert.AreEqual(7, objectList.Count);

            var expected = ExpectedMockData.GetDifferentDataTypes();

            var colBoolList = expected.GetValue("HeaderBoolean");
            var colCharList = expected.GetValue("HeaderChar");
            var colDateList = expected.GetValue("HeaderDate");
            var colIntegerList = expected.GetValue("HeaderInteger");
            var colNumericList = expected.GetValue("HeaderNumeric");
            var colStringList = expected.GetValue("HeaderString");
            var colTimeStampList = expected.GetValue("HeaderTimeStamp");

            objectList.ForEach((HeaderNamesObject obj, int i) =>
            {
                Assert.AreEqual(colBoolList[i], obj.HeaderBoolean);
                Assert.AreEqual(colCharList[i], obj.HeaderChar);
                Assert.AreEqual(colDateList[i], obj.HeaderDate);
                Assert.AreEqual(colIntegerList[i], obj.HeaderInteger);
                Assert.AreEqual(colNumericList[i], obj.HeaderNumeric);
                Assert.AreEqual(colStringList[i], obj.HeaderString);
                Assert.AreEqual(colTimeStampList[i], obj.HeaderTimeStamp);
            });
        }

        [Test]
        public async Task ShouldGetListOfTypeWithDifferentDataTypesWithNulls()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            var objectList = await dataProvider.GetListOfAsync<HeaderNamesObjectOptional>("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes_With_Nulls order by HeaderSort;");

            Assert.AreEqual(7, objectList.Count);

            var expected = ExpectedMockData.GetDifferentDataTypesWithNulls();

            var colBoolList = expected.GetValue("HeaderBoolean");
            var colCharList = expected.GetValue("HeaderChar");
            var colDateList = expected.GetValue("HeaderDate");
            var colIntegerList = expected.GetValue("HeaderInteger");
            var colNumericList = expected.GetValue("HeaderNumeric");
            var colStringList = expected.GetValue("HeaderString");
            var colTimeStampList = expected.GetValue("HeaderTimeStamp");

            objectList.ForEach((HeaderNamesObjectOptional obj, int i) =>
            {
                Assert.AreEqual(colBoolList[i], obj.HeaderBoolean);
                Assert.AreEqual(colCharList[i], obj.HeaderChar);
                Assert.AreEqual(colDateList[i], obj.HeaderDate);
                Assert.AreEqual(colIntegerList[i], obj.HeaderInteger);
                Assert.AreEqual(colNumericList[i], obj.HeaderNumeric);
                Assert.AreEqual(colStringList[i], obj.HeaderString);
                Assert.AreEqual(colTimeStampList[i], obj.HeaderTimeStamp);
            });
        }

        [Test]
        public async Task ShouldNotGetListOfTypeWithCancellationToken()
        {
            var cts = new CancellationTokenSource();
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            cts.Cancel();

            Assert.ThrowsAsync<TaskCanceledException>(async
                () => await dataProvider.GetListOfAsync<HeaderNamesObjectOptional>(
                    "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes_With_Nulls order by HeaderSort;",
                    cts.Token)
            );

            await Task.FromResult(0);
        }

        [Test]
        public async Task ShouldGetListOfTypeWithDifferentDataTypesWithCustomConnection()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var dataProvider = MoqProvider.CreateGenericDataProvider();

            connection.Open();

            var objectList = await dataProvider.GetListOfAsync<HeaderNamesObject>(
                "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                connection
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();

            Assert.AreEqual(1, objectList.Count);
        }

        [Test]
        public async Task ShouldCloseCustomConnectionRegardlessOfRequestIfOperationCancelledException()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var cts = new CancellationTokenSource();

            var dataProvider = MoqProvider.CreateGenericDataProvider();

            cts.Cancel();

            Assert.ThrowsAsync<TaskCanceledException>(async
                () => await dataProvider.GetListOfAsync<HeaderNamesObject>(
                    "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                    connection,
                    cts.Token)
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);

            await Task.FromResult(0);
        }

        [Test]
        public async Task ShouldGetIntValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var integerValue = await dataProvider.GetValueOfAsync<int>("HeaderInteger", "select HeaderInteger from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderInteger = 1;");
            Assert.AreEqual(1, integerValue);
        }

        [TestCase(1, true)]
        [TestCase(70000000, true)]
        [TestCase(100, false)]
        [TestCase(int.MaxValue, false)]
        public async Task ShouldGetNullableIntValue(int expectedInt, bool exists)
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var nullableInt = await dataProvider.GetValueOfAsync<int?>("HeaderInteger", $"select HeaderInteger from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderInteger = {expectedInt};");
            if (exists)
            {
                Assert.AreEqual(expectedInt, nullableInt.Value);
            }
            else
            {
                Assert.IsNull(nullableInt);
            }
        }

        [Test]
        public async Task ShouldGetDecimalValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var decimalValue = await dataProvider.GetValueOfAsync<decimal>("HeaderNumeric", "select HeaderNumeric from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderNumeric = 300.30;");
            Assert.AreEqual(new decimal(300.30), decimalValue);
        }

        [TestCase(6000000.60, true)]
        [TestCase(1234.5678, false)]
        public async Task ShouldGetNullableDecimalValue(decimal expectedDecimal, bool exists)
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var decimalValue = await dataProvider.GetValueOfAsync<decimal?>("HeaderNumeric", $"select HeaderNumeric from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderNumeric = {expectedDecimal};");
            if (exists)
            {
                Assert.AreEqual(expectedDecimal, decimalValue.Value);
            }
            else
            {
                Assert.IsNull(decimalValue);
            }
        }

        [Test]
        public async Task ShouldGetDoubleValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var doubleValue = await dataProvider.GetValueOfAsync<double>("HeaderNumeric", "select HeaderNumeric from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderNumeric = 300.30;");
            Assert.AreEqual(300.30, doubleValue);
        }

        [TestCase(6000000.60, true)]
        [TestCase(double.MaxValue, false)]
        public async Task ShouldGetNullableDoubleValue(double expectedDouble, bool exists)
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var charValue = await dataProvider.GetValueOfAsync<double?>("HeaderNumeric", $"select HeaderNumeric from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderNumeric = {expectedDouble};");
            if (exists)
            {
                Assert.AreEqual(expectedDouble, charValue.Value);
            }
            else
            {
                Assert.IsNull(charValue);
            }
        }

        [Test]
        public async Task ShouldGetStringValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var stringValue = await dataProvider.GetValueOfAsync<string>("HeaderString", "select HeaderString from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderString = 'Row 1 Value 6';");
            Assert.AreEqual("Row 1 Value 6", stringValue);
        }

        [Test]
        public async Task ShouldGetDateTimeValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var dateValue = await dataProvider.GetValueOfAsync<DateTime>("HeaderDate", "select HeaderDate from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderDate = '07-17-2070';");
            var expectedDate = new DateTime(2070, 7, 17);
            Assert.AreEqual(expectedDate, dateValue);
        }

        [Test]
        public async Task ShouldGetTimeStampValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var dateValue = await dataProvider.GetValueOfAsync<DateTime>("HeaderTimeStamp", "select HeaderTimeStamp from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderTimeStamp = '07-17-2070 07:17:17';");
            var expectedTimeStamp = new DateTime(2070, 7, 17, 7, 17, 17);
            Assert.AreEqual(expectedTimeStamp, dateValue);
        }

        [Test]
        public async Task ShouldGetCharValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var charValue = await dataProvider.GetValueOfAsync<char>("HeaderChar", "select HeaderChar from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderChar = 'a';");
            Assert.AreEqual('a', charValue);
        }

        [TestCase('b', true)]
        [TestCase('z', false)]
        public async Task ShouldGetNullableCharValue(char expectedChar, bool exists)
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var charValue = await dataProvider.GetValueOfAsync<char?>("HeaderChar", $"select HeaderChar from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderChar = '{expectedChar}';");
            if (exists)
            {
                Assert.AreEqual(expectedChar, charValue.Value);
            }
            else
            {
                Assert.IsNull(charValue);
            }
        }

        [Test]
        public async Task ShouldGetBooleanValue()
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var booleanValue = await dataProvider.GetValueOfAsync<bool>("HeaderBoolean", "select HeaderBoolean from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderBoolean = true;");
            Assert.AreEqual(true, booleanValue);
        }

        [TestCase(true, true, 7)]
        [TestCase(false, false, 100)]
        public async Task ShouldGetNullableBooleanValue(bool expectedBoolean, bool exists, int headerSortValue)
        {
            using var connection = MoqProvider.CreateDbConnection();
            var dataProvider = MoqProvider.CreateGenericDataProvider();
            var booleanValue = await dataProvider.GetValueOfAsync<bool?>("HeaderBoolean", $"select HeaderBoolean from AMCode.Data.tbl_DataProvider_Different_DataTypes where HeaderSort = {headerSortValue};");
            if (exists)
            {
                Assert.AreEqual(expectedBoolean, booleanValue.Value);
            }
            else
            {
                Assert.IsNull(booleanValue);
            }
        }
    }
}