using System;
using System.Data;
using AMCode.Data.Vertica;
using NUnit.Framework;
using Vertica.Data.VerticaClient;

namespace AMCode.Data.UnitTests.Vertica.VerticaCommandFactoryTests
{
    [TestFixture]
    public class VerticaCommandFactoryTest
    {
        IDbConnection connection;

        [SetUp]
        public void SetUp() => connection = new VerticaConnection(string.Empty);

        [Test]
        public void ShouldCreateOdbcCommand()
        {
            try
            {
                var factory = new VerticaCommandFactory();
                Assert.AreSame(typeof(VerticaCommand), factory.Create(connection).GetType());
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
                var factory = new VerticaCommandFactory();
                var command = factory.Create(connection, "test command");
                Assert.AreSame(typeof(VerticaCommand), command.GetType());
                Assert.AreEqual("test command", command.CommandText);
            }
            catch (Exception)
            {
                connection.Close();
            }
        }
    }
}