using System.Collections.Generic;
using System.Data;
using AMCode.Data;
using AMCode.Data.Exceptions;
using AMCode.Data.UnitTests.Data.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.GenericDataProviderTests
{
    [TestFixture]
    public class GenericDataProviderTest
    {
        [Test]
        public void ShouldThrowDbConnectionBrokenStateException()
        {
            var dataProvider = new GenericDataProvider(
                new Mock<IDbBridgeProviderFactory>().Object,
                new Mock<IExpandoObjectDataProviderFactory>().Object,
                new Mock<IQueryCancellation>().Object
            );

            var connectionMoq = new Mock<IDbConnection>();
            connectionMoq.SetupGet(moq => moq.State).Returns(ConnectionState.Broken);

            Assert.ThrowsAsync<DbConnectionBrokenStateException>(() => dataProvider.GetListOfAsync(string.Empty, connectionMoq.Object, reader => new List<DataProviderTestMock>()));
        }
    }
}