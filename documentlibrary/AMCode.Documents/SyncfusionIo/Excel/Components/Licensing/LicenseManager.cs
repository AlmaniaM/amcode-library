using Syncfusion.Licensing;

namespace AMCode.SyncfusionIo.Xlsx.Licensing
{
    /// <summary>
    /// A class designed for managing license keys.
    /// </summary>
    public class LicenseManager
    {
        /// <summary>
        /// Set the underlying exports library license.
        /// </summary>
        /// <param name="licenseKey">The license <see cref="string"/>.</param>
        public static void RegisterLicense(string licenseKey)
            => SyncfusionLicenseProvider.RegisterLicense(licenseKey);
    }
}