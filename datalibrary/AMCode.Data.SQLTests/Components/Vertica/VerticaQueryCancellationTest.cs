using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;
using AMCode.Data;
using AMCode.Data.Vertica;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Vertica.VerticaQueryCancellationTests
{
    [TestFixture]
    public class VerticaQueryCancellationTest
    {
        private readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        IDbBridgeProviderFactory dataReaderProviderFactory;

        [SetUp]
        public void SetUp()
        {
            var dbConnectionFactoryMoq = new Mock<IDbConnectionFactory>();
            dbConnectionFactoryMoq.Setup(factory => factory.Create()).Returns(getConnection(connectionString));
            dbConnectionFactoryMoq.Setup(factory => factory.Create(It.IsAny<string>())).Returns((string connStr) => getConnection(connStr));

            var dbCommandFactoryMoq = new Mock<IDbCommandFactory>();
            dbCommandFactoryMoq.Setup(factory => factory.Create(It.IsAny<IDbConnection>())).Returns((IDbConnection connection) => connection.CreateCommand());
            dbCommandFactoryMoq.Setup(factory => factory.Create(It.IsAny<IDbConnection>(), It.IsAny<string>())).Returns((IDbConnection connection, string command) => new OdbcCommand(command, (OdbcConnection)connection));

            var dataReaderProviderFactoryMoq = new Mock<IDbBridgeProviderFactory>();
            dataReaderProviderFactoryMoq.Setup(factory => factory.Create()).Returns(new DbBridge(dbConnectionFactoryMoq.Object, dbCommandFactoryMoq.Object));

            dataReaderProviderFactory = dataReaderProviderFactoryMoq.Object;
        }

        [Test]
        public async Task ShouldCancelSession()
        {
            var queryCancellation = new VerticaQueryCancellation(dataReaderProviderFactory);

            var connection = getConnection(connectionString);

            try
            {
                connection.Open();

                Assert.AreEqual(ConnectionState.Open, connection.State);

                var query = await queryCancellation.GetCancellationRequestAsync(connection);
                var query2 = await queryCancellation.GetCancellationRequestAsync(connection);

                Assert.AreEqual(query, query2);

                Assert.IsTrue(!string.IsNullOrEmpty(query));

                await queryCancellation.ExecuteCancellationRequestAsync(query);

                Assert.AreNotEqual(query, queryCancellation.GetCancellationRequestAsync(connection));

                connection.Close();
            }
            catch (System.Exception)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private IDbConnection getConnection(string connStr) => new OdbcConnection(connStr);
    }
}