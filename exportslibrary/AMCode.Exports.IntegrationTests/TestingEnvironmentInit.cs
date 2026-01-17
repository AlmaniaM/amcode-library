using AMCode.Common.Util;
using AMCode.Documents.Xlsx.Licensing;
using NUnit.Framework;

namespace AMCode.Exports.IntegrationTests
{
    [SetUpFixture]
    public class TestingEnvironmentInit
    {
        [OneTimeSetUp]
        public void InitializeEnvironment() => LicenseManager.RegisterLicense(EnvironmentUtil.GetEnvironmentVariable("DL_XLSX_LIBRARY_LICENSE"));
    }
}