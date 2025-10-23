using System;
using System.Data;
using System.Data.Odbc;
using AMCode.Data.Odbc;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Odbc.OdbcCommandFactoryTests
{
    [TestFixture]
    public class OdbcCommandFactoryTest
    {
        IDbConnection connection;

        [SetUp]
        public void SetUp() => connection = new OdbcConnection(string.Empty);

        [Test]
        public void ShouldCreateOdbcCommand()
        {
            try
            {
                var factory = new OdbcCommandFactory();
                Assert.AreSame(typeof(OdbcCommand), factory.Create(connection).GetType());
            }
            catch (Exception)
            {
                connection.Close();
            }
        }

        [Test]
        public void ShouldCreateOdbcCommandWithCommandString()
        {
            try
            {
                var factory = new OdbcCommandFactory();
                var command = factory.Create(connection, "test command");
                Assert.AreSame(typeof(OdbcCommand), command.GetType());
                Assert.AreEqual("test command", command.CommandText);
            }
            catch (Exception)
            {
                connection.Close();
            }
        }
    }
}