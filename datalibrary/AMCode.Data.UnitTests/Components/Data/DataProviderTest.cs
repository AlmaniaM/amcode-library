using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.DataTransform;
using AMCode.Data;
using AMCode.Data.UnitTests.Data.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.DataProviderTests
{
    [TestFixture]
    public class DataProviderTest
    {
        IDataProvider dataProvider;
        Mock<IDbBridge> dbBridgeMoq;
        Mock<IDbBridgeProviderFactory> dataReaderProviderFactoryMoq;
        Mock<IExpandoObjectDataProvider> expandoDataProviderMoq;
        Mock<IExpandoObjectDataProviderFactory> expandoObjectDataProviderFactoryMoq;
        Mock<IDbExecute> dbExecuteMoq;
        Mock<IDbExecuteFactory> dbExecuteFactoryMoq;
        Mock<IGenericDataProvider> genericDataProviderMoq;
        Mock<IGenericDataProviderFactory> genericDataProviderFactoryMoq;

        [SetUp]
        public void SetUp()
        {
            // Set up an IDataReaderProviderFactory to return a DbBridge on Create() call
            dbBridgeMoq = new Mock<IDbBridge>();
            dataReaderProviderFactoryMoq = new Mock<IDbBridgeProviderFactory>();
            dataReaderProviderFactoryMoq.Setup(factory => factory.Create()).Returns(dbBridgeMoq.Object);

            // Set up an IDbExecuteFactory
            dbExecuteMoq = new Mock<IDbExecute>();
            dbExecuteFactoryMoq = new Mock<IDbExecuteFactory>();
            dbExecuteFactoryMoq.Setup(factory => factory.Create()).Returns(dbExecuteMoq.Object);

            // Set up an IExpandoObjectDataProviderFactory and an IGenericDataProviderFactory
            expandoDataProviderMoq = new Mock<IExpandoObjectDataProvider>();
            expandoObjectDataProviderFactoryMoq = new Mock<IExpandoObjectDataProviderFactory>();
            expandoObjectDataProviderFactoryMoq.Setup(factory => factory.Create()).Returns(expandoDataProviderMoq.Object);

            genericDataProviderMoq = new Mock<IGenericDataProvider>();
            genericDataProviderFactoryMoq = new Mock<IGenericDataProviderFactory>();
            genericDataProviderFactoryMoq.Setup(factory => factory.Create()).Returns(genericDataProviderMoq.Object);

            // Set up the IDataProvider
            dataProvider = new DataProvider(dbExecuteFactoryMoq.Object, expandoObjectDataProviderFactoryMoq.Object, genericDataProviderFactoryMoq.Object);
        }

        [Test]
        public async Task ShouldPassExecuteQuery()
        {
            await dataProvider.ExecuteAsync(string.Empty);

            dbExecuteFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            dbExecuteMoq.Verify(mock => mock.ExecuteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldPassExecuteQueryCancellationToken()
        {
            var token = new CancellationToken();
            await dataProvider.ExecuteAsync(string.Empty, token);

            dbExecuteFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            dbExecuteMoq.Verify(mock => mock.ExecuteAsync(It.IsAny<string>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetExpandoListQuery()
        {
            await dataProvider.GetExpandoListAsync(string.Empty);

            expandoObjectDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            expandoDataProviderMoq.Verify(mock => mock.GetExpandoListAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetExpandoListQueryCancellationToken()
        {
            var token = new CancellationToken();
            await dataProvider.GetExpandoListAsync(string.Empty, token);

            expandoObjectDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            expandoDataProviderMoq.Verify(mock => mock.GetExpandoListAsync(It.IsAny<string>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetExpandoListQueryColumnsCancellationToken()
        {
            var columnsMoq = new Mock<IList<IDataTransformColumnDefinition>>();
            var token = new CancellationToken();
            await dataProvider.GetExpandoListAsync(string.Empty, columnsMoq.Object, token);

            expandoObjectDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            expandoDataProviderMoq.Verify(mock => mock.GetExpandoListAsync(It.IsAny<string>(), It.IsAny<IList<IDataTransformColumnDefinition>>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetExpandoListQueryConnection()
        {
            var connectionMoq = new Mock<IDbConnection>();
            await dataProvider.GetExpandoListAsync(string.Empty, connectionMoq.Object);

            expandoObjectDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            expandoDataProviderMoq.Verify(mock => mock.GetExpandoListAsync(It.IsAny<string>(), It.IsAny<IDbConnection>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetExpandoListQueryConnectionCancellationToken()
        {
            var connectionMoq = new Mock<IDbConnection>();
            var token = new CancellationToken();
            await dataProvider.GetExpandoListAsync(string.Empty, connectionMoq.Object, token);

            expandoObjectDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            expandoDataProviderMoq.Verify(mock => mock.GetExpandoListAsync(It.IsAny<string>(), It.IsAny<IDbConnection>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetExpandoListQueryColumnsConnectionCancellationToken()
        {
            var columnsMoq = new Mock<IList<IDataTransformColumnDefinition>>();
            var connectionMoq = new Mock<IDbConnection>();
            var token = new CancellationToken();
            await dataProvider.GetExpandoListAsync(string.Empty, columnsMoq.Object, connectionMoq.Object, token);

            expandoObjectDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            expandoDataProviderMoq.Verify(mock => mock.GetExpandoListAsync(
                It.IsAny<string>(),
                It.IsAny<IList<IDataTransformColumnDefinition>>(),
                It.IsAny<IDbConnection>(),
                It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetListOfQuery()
        {
            await dataProvider.GetListOfAsync<DataProviderTestMock>(string.Empty);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetListOfAsync<DataProviderTestMock>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetListOfQueryCancellationToken()
        {
            var token = new CancellationToken();
            await dataProvider.GetListOfAsync<DataProviderTestMock>(string.Empty, token);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetListOfAsync<DataProviderTestMock>(It.IsAny<string>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetListOfQueryColumnsCancellationToken()
        {
            var columnsMoq = new Mock<IList<IDataTransformColumnDefinition>>();
            var token = new CancellationToken();
            await dataProvider.GetListOfAsync<DataProviderTestMock>(string.Empty, columnsMoq.Object, token);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetListOfAsync<DataProviderTestMock>(It.IsAny<string>(), It.IsAny<IList<IDataTransformColumnDefinition>>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetListOfQueryConnection()
        {
            var connectionMoq = new Mock<IDbConnection>();
            await dataProvider.GetListOfAsync<DataProviderTestMock>(string.Empty, connectionMoq.Object);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetListOfAsync<DataProviderTestMock>(It.IsAny<string>(), It.IsAny<IDbConnection>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetListOfQueryConnectionCancellationToken()
        {
            var connectionMoq = new Mock<IDbConnection>();
            var token = new CancellationToken();
            await dataProvider.GetListOfAsync<DataProviderTestMock>(string.Empty, connectionMoq.Object, token);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetListOfAsync<DataProviderTestMock>(It.IsAny<string>(), It.IsAny<IDbConnection>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetListOfQueryColumnsConnectionCancellationToken()
        {
            var columnsMoq = new Mock<IList<IDataTransformColumnDefinition>>();
            var connectionMoq = new Mock<IDbConnection>();
            var token = new CancellationToken();
            await dataProvider.GetListOfAsync<DataProviderTestMock>(string.Empty, columnsMoq.Object, connectionMoq.Object, token);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetListOfAsync<DataProviderTestMock>(
                It.IsAny<string>(),
                It.IsAny<IList<IDataTransformColumnDefinition>>(),
                It.IsAny<IDbConnection>(),
                It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetValueOfQuery()
        {
            await dataProvider.GetValueOfAsync<string>("TestColumn", string.Empty);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetValueOfAsync<string>(It.Is<string>(str => str.Equals("TestColumn")), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldPassGetValueOfQueryCancellationToken()
        {
            var token = new CancellationToken();
            await dataProvider.GetValueOfAsync<string>("TestColumn2", string.Empty, token);

            genericDataProviderFactoryMoq.Verify(mock => mock.Create(), Times.Once());
            genericDataProviderMoq.Verify(mock => mock.GetValueOfAsync<string>(It.Is<string>(str => str.Equals("TestColumn2")), It.IsAny<string>(), It.Is<CancellationToken>(ct => ct == token)), Times.Once());
        }
    }
}