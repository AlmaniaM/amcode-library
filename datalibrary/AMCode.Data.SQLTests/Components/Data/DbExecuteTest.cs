using System;
using System.Threading.Tasks;
using AMCode.Data.SQLTests.Globals;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Data.DbExecuteTests
{
    [TestFixture]
    public class DbExecuteTest
    {
        [Test]
        public async Task ShouldCreateTable()
        {
            var dbExecute = MoqProvider.CreateDbExecute();

            var tableName = $"tbl_{Guid.NewGuid().ToString().Replace("-", "")}";

            var createTableQuery = string.Format($@"
				drop table if exists AMCode.Data.{tableName};
				create table AMCode.Data.{tableName}
				(
					ColumnOne VARCHAR(10),
					ColumnTwo VARCHAR(10)
				);
			");

            await dbExecute.ExecuteAsync(createTableQuery);

            var findTableQuery = $"select table_name as TableName from v_catalog.tables where table_name = '{tableName}' and table_schema = 'AMCode.Data'";

            using var dbBridge = MoqProvider.CreateDbBridge();

            dbBridge.Connect(true);

            using var dr = await dbBridge.QueryAsync(findTableQuery);

            dr.Read();
            var result = dr.GetValue(0);

            Assert.AreEqual(tableName, result);
        }

        [Test]
        public async Task ShouldCreateTableAndInsertData()
        {
            var dbExecute = MoqProvider.CreateDbExecute();

            var tableName = $"tbl_{Guid.NewGuid().ToString().Replace("-", "")}";

            var createTableQuery = string.Format($@"
				drop table if exists AMCode.Data.{tableName};
				create table AMCode.Data.{tableName}
				(
					ColumnOne VARCHAR(10),
					ColumnTwo VARCHAR(10)
				);

				insert into AMCode.Data.{tableName} (ColumnOne, ColumnTwo)
				select 'Value 1', 'Value 2'
				union
				select 'Value 3', 'Value 4';
			");

            await dbExecute.ExecuteAsync(createTableQuery);

            var findTableQuery = $"select count(*) as count from AMCode.Data.'{tableName}';";

            using var dbBridge = MoqProvider.CreateDbBridge();

            dbBridge.Connect(true);

            using var dr = await dbBridge.QueryAsync(findTableQuery);

            dr.Read();
            var result = dr.GetValue(0);

            Assert.AreEqual(2, Convert.ToInt32(result));
        }
    }
}