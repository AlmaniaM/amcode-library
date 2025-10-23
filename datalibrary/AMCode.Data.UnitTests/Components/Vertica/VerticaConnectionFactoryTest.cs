using AMCode.Data.Vertica;
using NUnit.Framework;
using Vertica.Data.VerticaClient;

namespace AMCode.Data.UnitTests.Vertica.OdbcConnectionFactoryTests
{
    [TestFixture]
    public class VerticaConnectionFactoryTest
    {
        [Test]
        public void ShouldCreateVerticaConnection()
        {
            var factory = new VerticaConnectionFactory(string.Empty);
            Assert.AreSame(typeof(VerticaConnection), factory.Create().GetType());
        }

        [Test]
        public void ShouldCreateVerticaConnectionWithCustomConnectionString()
        {
            var factory = new VerticaConnectionFactory(string.Empty);
            Assert.AreSame(typeof(VerticaConnection), factory.Create("Driver=TestDriver").GetType());
        }
    }
}