using System;
using System.Configuration;
using AMCode.Data.Vertica;
using NUnit.Framework;
using Vertica.Data.VerticaClient;

namespace AMCode.Data.SQLTests.Vertica.VerticaDataReaderProviderFactoryTests
{
    [TestFixture]
    public class VerticaDataReaderProviderFactoryTest
    {
        [Test]
        public void ShouldCreateIDbBridgeWithAccess()
        {
            var factory = new VerticaDataReaderProviderFactory(ConfigurationManager.AppSettings["VerticaConnectionString"]);
            var db = factory.Create();

            try
            {
                Assert.AreSame(typeof(VerticaConnection), db.Connect(false).GetType());
                db.Disconnect();
            }
            catch (Exception)
            {
                db.Disconnect();
            }
        }
    }
}