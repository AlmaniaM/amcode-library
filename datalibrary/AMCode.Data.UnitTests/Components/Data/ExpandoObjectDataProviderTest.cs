using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using AMCode.Data;
using AMCode.Data.Exceptions;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.ExpandoObjectDataProviderTests
{
    [TestFixture]
    public class ExpandoObjectDataProviderTest
    {
        [Test]
        public void ShouldThrowDbConnectionBrokenStateException()
        {
            var dataProvider = new ExpandoObjectDataProvider(
                new Mock<IDbBridgeProviderFactory>().Object,
                new Mock<IQueryCancellation>().Object
            );

            var connectionMoq = new Mock<IDbConnection>();
            connectionMoq.SetupGet(moq => moq.State).Returns(ConnectionState.Broken);

            Assert.ThrowsAsync<DbConnectionBrokenStateException>(() => dataProvider.GetExpandoListAsync(string.Empty, connectionMoq.Object, reader => new List<ExpandoObject>()));
        }
    }
}