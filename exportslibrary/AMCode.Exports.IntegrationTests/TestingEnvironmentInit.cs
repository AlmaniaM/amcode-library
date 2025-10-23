using AMCode.Common.Util;
using AMCode.Xlsx.Licensing;
using NUnit.Framework;

namespace DlExportsLibrary.IntegrationTests
{
    [SetUpFixture]
    public class TestingEnvironmentInit
    {
        [OneTimeSetUp]
        public void InitializeEnvironment() => LicenseManager.RegisterLicense(EnvironmentUtil.GetEnvironmentVariable("DL_XLSX_LIBRARY_LICENSE"));
    }
}