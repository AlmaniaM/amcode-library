using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.Builder;
using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Data.SQLTests.Globals;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Data.ExpandoObjectDataProviderTests
{
    [TestFixture]
    public class ExpandoObjectDataProviderWithColumnsTest
    {
        private IDictionary<string, IDataTransformColumnDefinition> columnsMap;

        private DateTime formatterDate;

        [SetUp]
        public void SetUp()
        {
            formatterDate = DateTime.Now;

            columnsMap = new Dictionary<string, IDataTransformColumnDefinition>
            {
                ["HeaderBoolean"] = createColumn<bool>(new ColumnName { DisplayName = "Header Boolean", FieldName = "HeaderBoolean" }, "BooleanColumn", value => true),
                ["HeaderChar"] = createColumn<char>(new ColumnName { DisplayName = "Header Char", FieldName = "HeaderChar" }, "CharColumn", value => 'z'),
                ["HeaderDate"] = createColumn<DateTime>(new ColumnName { DisplayName = "Header Date", FieldName = "HeaderDate" }, "DateColumn", value => formatterDate),
                ["HeaderInteger"] = createColumn<int>(new ColumnName { DisplayName = "Header Integer", FieldName = "HeaderInteger" }, "IntegerColumn", value => int.MaxValue),
                ["HeaderNumeric"] = createColumn<double>(new ColumnName { DisplayName = "Header Numeric", FieldName = "HeaderNumeric" }, "NumericColumn", value => double.MaxValue),
                ["HeaderString"] = createColumn<string>(new ColumnName { DisplayName = "Header String", FieldName = "HeaderString" }, "StringColumn", value => "Test Value"),
                ["HeaderTimeStamp"] = createColumn<DateTime?>(new ColumnName { DisplayName = "Header TimeStamp", FieldName = "HeaderTimeStamp" }, "TimeStampColumn", value => default(DateTime?)),
            };
        }

        [Test]
        public async Task ShouldGetExpandoListWithDifferentDataTypes()
        {
            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            var expandoList = await dataProvider.GetExpandoListAsync("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;", columnsMap.Values.ToList());

            Assert.AreEqual(1, expandoList.Count);

            var expando = expandoList[0];

            Assert.AreEqual(true, expando.GetValue<bool?>("BooleanColumn"));
            Assert.AreEqual('z', expando.GetValue<char?>("CharColumn"));
            Assert.AreEqual(formatterDate, expando.GetValue<DateTime?>("DateColumn"));
            Assert.AreEqual(int.MaxValue, expando.GetValue<int?>("IntegerColumn"));
            Assert.AreEqual(double.MaxValue, expando.GetValue<double?>("NumericColumn"));
            Assert.AreEqual("Test Value", expando.GetValue<string>("StringColumn"));
            Assert.AreEqual(default(DateTime?), expando.GetValue<DateTime?>("TimeStampColumn"));
        }

        [Test]
        public async Task ShouldGetExpandoListWithDifferentDataTypesWithCustomConnection()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            connection.Open();

            var expandoList = await dataProvider.GetExpandoListAsync(
                "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                columnsMap.Values.ToList(),
                connection
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);

            Assert.AreEqual(1, expandoList.Count);
        }

        [Test]
        public void ShouldNotGetExpandoListWithCancellationToken()
        {
            var cts = new CancellationTokenSource();

            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            cts.Cancel();

            Assert.Throws<TaskCanceledException>(
                () => dataProvider.GetExpandoListAsync(
                    "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                    columnsMap.Values.ToList(), cts.Token).GetAwaiter().GetResult()
            );
        }

        [Test]
        public void ShouldCloseCustomConnectionRegardlessOfRequestIfOperationCancelledException()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var cts = new CancellationTokenSource();

            var dataProvider = MoqProvider.CreateExpandoObjectDataProvider();

            cts.Cancel();

            Assert.Throws<TaskCanceledException>(
                () => dataProvider.GetExpandoListAsync(
                    "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                    columnsMap.Values.ToList(),
                    connection,
                    cts.Token).GetAwaiter().GetResult()
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();
        }

        private IDataTransformColumnDefinition createColumn<T>(ColumnName columnName, string propertyName, DataTransformFunction<T> dataTransform)
        {
            return new ColumnFactoryBuilder()
                    .ColumnName(columnName)
                    .PropertyName(propertyName)
                    .DataTransformation(dataTransform)
                    .Build();
        }
    }
}