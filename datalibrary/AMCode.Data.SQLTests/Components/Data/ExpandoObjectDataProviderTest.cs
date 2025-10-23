using System;
using System.Data;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Data.SQLTests.Data.DataProviderTests.MockFiles;
using AMCode.Data.SQLTests.Globals;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Data.ExpandoObjectDataProviderTests
{
    [TestFixture]
    public class ExpandoObjectDataProviderTest
    {
        [Test]
        public async Task ShouldGetExpandoListWithCorrectData()
        {
            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            var expandoList = await dataProvider.GetExpandoListAsync("SELECT * FROM AMCode.Data.tbl_DataProvider_Static_Data order by HeaderOne;");

            Assert.AreEqual(5, expandoList.Count);

            static string createCellValue(int row, int column) => $"Row {row} Value {column}";

            expandoList.ForEach((ExpandoObject expando, int i) =>
            {
                var row = i + 1;

                Assert.AreEqual(createCellValue(row, 1), expando.GetValue<string>("HeaderOne"));
                Assert.AreEqual(createCellValue(row, 2), expando.GetValue<string>("HeaderTwo"));
                Assert.AreEqual(createCellValue(row, 3), expando.GetValue<string>("HeaderThree"));
                Assert.AreEqual(createCellValue(row, 4), expando.GetValue<string>("HeaderFour"));
                Assert.AreEqual(createCellValue(row, 5), expando.GetValue<string>("HeaderFive"));
            });
        }

        [Test]
        public async Task ShouldGetExpandoListWithNulls()
        {
            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            var expandoList = await dataProvider.GetExpandoListAsync("SELECT * FROM AMCode.Data.tbl_DataProvider_Static_Data_With_Nulls order by HeaderFive;");

            Assert.AreEqual(5, expandoList.Count);

            static string createCellValue(int row, int column) => $"Row {row} Value {column}";

            var columnCount = 5;

            expandoList.ForEach((ExpandoObject expando, int i) =>
            {
                var row = i + 1;

                var colOneValue = i % columnCount == 0 ? null : createCellValue(row, 1);
                var colTwoValue = i % columnCount == 1 ? null : createCellValue(row, 2);
                var colThreeValue = i % columnCount == 2 ? null : createCellValue(row, 3);
                var colFourValue = i % columnCount == 3 ? null : createCellValue(row, 4);
                var colFiveValue = i % columnCount == 4 ? null : createCellValue(row, 5);

                Assert.AreEqual(colOneValue, expando.GetValue<string>("HeaderOne"));
                Assert.AreEqual(colTwoValue, expando.GetValue<string>("HeaderTwo"));
                Assert.AreEqual(colThreeValue, expando.GetValue<string>("HeaderThree"));
                Assert.AreEqual(colFourValue, expando.GetValue<string>("HeaderFour"));
                Assert.AreEqual(colFiveValue, expando.GetValue<string>("HeaderFive"));
            });
        }

        [Test]
        public async Task ShouldGetExpandoListWithDifferentDataTypes()
        {
            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            var expandoList = await dataProvider.GetExpandoListAsync("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderInteger;");

            Assert.AreEqual(7, expandoList.Count);

            var expected = ExpectedMockData.GetDifferentDataTypes();

            var colBoolList = expected.GetValue("HeaderBoolean");
            var colCharList = expected.GetValue("HeaderChar");
            var colDateList = expected.GetValue("HeaderDate");
            var colIntegerList = expected.GetValue("HeaderInteger");
            var colNumericList = expected.GetValue("HeaderNumeric");
            var colStringList = expected.GetValue("HeaderString");
            var colTimeStampList = expected.GetValue("HeaderTimeStamp");

            expandoList.ForEach((ExpandoObject expando, int i) =>
            {
                Assert.AreEqual(colBoolList[i], expando.GetValue<bool>("HeaderBoolean"));
                Assert.AreEqual(colCharList[i], expando.GetValue<char>("HeaderChar"));
                Assert.AreEqual(colDateList[i], Convert.ToDateTime(expando.GetValue<string>("HeaderDate")));
                Assert.AreEqual(colIntegerList[i], expando.GetValue<int>("HeaderInteger"));
                Assert.AreEqual(colNumericList[i], expando.GetValue<double>("HeaderNumeric"));
                Assert.AreEqual(colStringList[i], expando.GetValue<string>("HeaderString"));
                Assert.AreEqual(colTimeStampList[i], Convert.ToDateTime(expando.GetValue<string>("HeaderTimeStamp")));
            });
        }

        [Test]
        public async Task ShouldGetExpandoListWithDifferentDataTypesWithNulls()
        {
            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            var expandoList = await dataProvider.GetExpandoListAsync("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes_With_Nulls order by HeaderSort;");

            Assert.AreEqual(7, expandoList.Count);

            var expected = ExpectedMockData.GetDifferentDataTypesWithNulls();

            var colBoolList = expected.GetValue("HeaderBoolean");
            var colCharList = expected.GetValue("HeaderChar");
            var colDateList = expected.GetValue("HeaderDate");
            var colIntegerList = expected.GetValue("HeaderInteger");
            var colNumericList = expected.GetValue("HeaderNumeric");
            var colStringList = expected.GetValue("HeaderString");
            var colTimeStampList = expected.GetValue("HeaderTimeStamp");

            expandoList.ForEach((ExpandoObject expando, int i) =>
            {
                Assert.AreEqual(colBoolList[i], expando.GetValue<bool?>("HeaderBoolean"));
                Assert.AreEqual(colCharList[i], expando.GetValue<char?>("HeaderChar"));
                Assert.AreEqual(colDateList[i], expando.GetValue<DateTime?>("HeaderDate"));
                Assert.AreEqual(colIntegerList[i], expando.GetValue<int?>("HeaderInteger"));
                Assert.AreEqual(colNumericList[i], expando.GetValue<double?>("HeaderNumeric"));
                Assert.AreEqual(colStringList[i], expando.GetValue<string>("HeaderString"));
                Assert.AreEqual(colTimeStampList[i], expando.GetValue<DateTime?>("HeaderTimeStamp"));
            });
        }

        [Test]
        public void ShouldNotGetExpandoListWithCancellationToken()
        {
            var cts = new CancellationTokenSource();
            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            cts.Cancel();

            Assert.Throws<TaskCanceledException>(
                () => dataProvider.GetExpandoListAsync("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes_With_Nulls order by HeaderSort;", cts.Token).GetAwaiter().GetResult()
            );
        }

        [Test]
        public async Task ShouldGetExpandoListWithDifferentDataTypesWithCustomConnection()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            connection.Open();

            var expandoList = await dataProvider.GetExpandoListAsync(
                "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                connection
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);

            Assert.AreEqual(1, expandoList.Count);
        }

        [Test]
        public void ShouldCloseCustomConnectionRegardlessOfRequestIfOperationCancelledException()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var cts = new CancellationTokenSource();

            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            cts.Cancel();

            connection.Open();

            Assert.ThrowsAsync<TaskCanceledException>(() => dataProvider.GetExpandoListAsync(
                "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                connection,
                cts.Token)
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);
        }
    }
}