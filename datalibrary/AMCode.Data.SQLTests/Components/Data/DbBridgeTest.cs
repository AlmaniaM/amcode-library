using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AMCode.Data.SQLTests.Globals;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Data.DbBridgeTests
{
    [TestFixture]
    public class DbBridgeTest
    {
        [Test]
        public async Task ShouldExecuteQuery()
        {
            using var dbBridge = MoqProvider.CreateDbBridge();

            var uuid = Regex.Replace(Guid.NewGuid().ToString(), "-", "");

            var tableName = $"tbl_{uuid}";

            dbBridge.Connect(true);

            await dbBridge.ExecuteAsync($@"
				drop table if exists AMCode.Data.{tableName};
				create table AMCode.Data.{tableName}
				(
					ColumnOne varchar(100)
				);

				insert into AMCode.Data.{tableName} (ColumnOne) Values ('Test Value 1');
			");

            using var results = await dbBridge.QueryAsync($"select * from AMCode.Data.{tableName};");

            results.Read();

            await dbBridge.ExecuteAsync($"drop table if exists AMCode.Data.{tableName};");

            Assert.AreEqual("Test Value 1", results.GetValue(0));
        }

        [Test]
        public async Task ShouldExecuteQueryWithCustomConnection()
        {
            using var connection = MoqProvider.CreateDbConnection();
            using var dbBridge = MoqProvider.CreateDbBridge();

            var uuid = Regex.Replace(Guid.NewGuid().ToString(), "-", "");

            var tableName = $"tbl_{uuid}";

            connection.Open();

            await dbBridge.ExecuteAsync($@"
				drop table if exists AMCode.Data.{tableName};
				create table AMCode.Data.{tableName}
				(
					ColumnOne varchar(100)
				);

				insert into AMCode.Data.{tableName} (ColumnOne) Values ('Test Value 1');",
                connection
            );

            using var results = await dbBridge.QueryAsync($"select * from AMCode.Data.{tableName};", connection);

            results.Read();

            await dbBridge.ExecuteAsync($"drop table if exists AMCode.Data.{tableName};", connection);

            Assert.AreEqual("Test Value 1", results.GetValue(0));
        }
    }
}