using System;
using System.Configuration;
using System.Data.Odbc;
using AMCode.Data.Odbc;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.Odbc.OdbcDataReaderProviderFactoryTests
{
    [TestFixture]
    public class OdbcDataReaderProviderFactoryTest
    {
        [Test]
        public void ShouldCreateIDbBridgeWithAccess()
        {
            var factory = new OdbcDataReaderProviderFactory(ConfigurationManager.AppSettings["OdbcConnectionString"]);
            var db = factory.Create();

            try
            {
                Assert.AreSame(typeof(OdbcConnection), db.Connect(false).GetType());
                db.Disconnect();
            }
            catch (Exception)
            {
                db.Disconnect();
            }
        }
    }
}