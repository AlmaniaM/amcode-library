using AMCode.Common.Util;
using AMCode.SyncfusionIo.Xlsx.Licensing;
using NUnit.Framework;

namespace DlXlsxLibrary.UnitTests
{
    [SetUpFixture]
    public class TestingEnvironmentInit
    {
        [OneTimeSetUp]
        public void InitializeEnvironment() => LicenseManager.RegisterLicense(EnvironmentUtil.GetEnvironmentVariable("DL_XLSX_LIBRARY_LICENSE"));
    }
}