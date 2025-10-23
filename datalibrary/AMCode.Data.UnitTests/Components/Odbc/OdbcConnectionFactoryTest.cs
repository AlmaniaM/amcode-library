using System.Data.Odbc;
using AMCode.Data.Odbc;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Odbc.OdbcConnectionFactoryTests
{
    [TestFixture]
    public class OdbcConnectionFactoryTest
    {
        [Test]
        public void ShouldCreateOdbcConnection()
        {
            var factory = new OdbcConnectionFactory(string.Empty);
            Assert.AreSame(typeof(OdbcConnection), factory.Create().GetType());
        }

        [Test]
        public void ShouldCreateOdbcConnectionWithCustomConnectionString()
        {
            var factory = new OdbcConnectionFactory(string.Empty);
            Assert.AreSame(typeof(OdbcConnection), factory.Create("Driver=TestDriver").GetType());
        }
    }
}