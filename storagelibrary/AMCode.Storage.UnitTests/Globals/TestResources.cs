namespace AMCode.Storage.UnitTests.Globals
{
    /// <summary>
    /// A class designed to hold global test resources.
    /// </summary>
    public class TestResources
    {
        /// <summary>
        /// The Azure Storage account connection string.
        /// </summary>
        /// <remarks>
        /// The default connection string is for the <see href="https://github.com/Azure/Azurite">Azurite</see> Docker container.
        /// You can find the connection string information on the Azurite <see href="https://github.com/Azure/Azurite#default-storage-account">GitHub README</see> page.
        /// </remarks>
        public static readonly string StorageConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";
    }
}