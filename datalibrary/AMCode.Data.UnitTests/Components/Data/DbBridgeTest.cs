using System.Data;
using AMCode.Data;
using AMCode.Data.Exceptions;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.DbBridgeTests
{
    [TestFixture]
    public class DbBridgeTest
    {
        [Test]
        public void ShouldThrowDbConnectionBrokenStateExceptionInExecuteAsync()
        {
            var dbBridge = new DbBridge(
                new Mock<IDbConnectionFactory>().Object,
                new Mock<IDbCommandFactory>().Object
            );

            var connectionMoq = new Mock<IDbConnection>();
            connectionMoq.SetupGet(moq => moq.State).Returns(ConnectionState.Broken);

            Assert.ThrowsAsync<DbConnectionBrokenStateException>(() => dbBridge.ExecuteAsync(string.Empty, connectionMoq.Object));
        }

        [Test]
        public void ShouldThrowDbConnectionBrokenStateExceptionInQueryAsync()
        {
            var dbBridge = new DbBridge(
                new Mock<IDbConnectionFactory>().Object,
                new Mock<IDbCommandFactory>().Object
            );

            var connectionMoq = new Mock<IDbConnection>();
            connectionMoq.SetupGet(moq => moq.State).Returns(ConnectionState.Broken);

            Assert.ThrowsAsync<DbConnectionBrokenStateException>(() => dbBridge.QueryAsync(string.Empty, connectionMoq.Object));
        }
    }
}