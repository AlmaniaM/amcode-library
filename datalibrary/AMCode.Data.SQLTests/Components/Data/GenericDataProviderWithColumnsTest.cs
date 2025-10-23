using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.Builder;
using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;
using AMCode.Data.SQLTests.Data.GenericDataProviderTests.Models;
using AMCode.Data.SQLTests.Globals;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Data.GenericDataProviderTests
{
    [TestFixture]
    public class GenericDataProviderWithColumnsTest
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
        public async Task ShouldGetListOfTypeWithDifferentDataTypes()
        {
            var dataProvider = MoqProvider.CreateGenericDataProvider();

            var objectList = await dataProvider.GetListOfAsync<ColumnNamesObjectOptional>(
                "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                columnsMap.Values.ToList()
            );

            Assert.AreEqual(1, objectList.Count);

            var obj = objectList[0];

            Assert.AreEqual(true, obj.BooleanColumn);
            Assert.AreEqual('z', obj.CharColumn);
            Assert.AreEqual(formatterDate, obj.DateColumn);
            Assert.AreEqual(int.MaxValue, obj.IntegerColumn);
            Assert.AreEqual(double.MaxValue, obj.NumericColumn);
            Assert.AreEqual("Test Value", obj.StringColumn);
            Assert.AreEqual(default(DateTime?), obj.TimeStampColumn);
        }

        [Test]
        public async Task ShouldGetListOfTypeWithDifferentDataTypesWithCustomConnection()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var dataProvider = MoqProvider.CreateGenericDataProvider();

            connection.Open();

            var objectList = await dataProvider.GetListOfAsync<ColumnNamesObjectOptional>(
                "SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                columnsMap.Values.ToList(),
                connection
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();

            Assert.AreEqual(1, objectList.Count);
        }

        [Test]
        public void ShouldNotGetListOfTypeWithCancellationToken()
        {
            var cts = new CancellationTokenSource();

            var dataProvider = MoqProvider.CreateGenericDataProvider();

            cts.Cancel();

            Assert.Throws<TaskCanceledException>(
                () => dataProvider.GetListOfAsync<ColumnNamesObjectOptional>("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                columnsMap.Values.ToList(), cts.Token).GetAwaiter().GetResult()
            );
        }

        [Test]
        public void ShouldCloseCustomConnectionRegardlessOfRequestIfOperationCancelledException()
        {
            using var connection = MoqProvider.CreateDbConnection();

            var cts = new CancellationTokenSource();

            var dataProvider = MoqProvider.CreateGenericDataProvider();

            cts.Cancel();

            Assert.Throws<TaskCanceledException>(
                () => dataProvider.GetListOfAsync<ColumnNamesObjectOptional>("SELECT * FROM AMCode.Data.tbl_DataProvider_Different_DataTypes order by HeaderSort limit 1;",
                columnsMap.Values.ToList(), connection, cts.Token).GetAwaiter().GetResult()
            );

            Assert.AreEqual(ConnectionState.Open, connection.State);
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