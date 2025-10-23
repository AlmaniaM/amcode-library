using System.IO;

namespace DemandLink.Storage.AzureBlob
{
    /// <summary>
    /// A class designed to store <see cref="Stream"/>s in Azure Blob storage.
    /// </summary>
    public class AzureBlobStreamDataSource : BaseStreamDataSource, IStreamDataSourceAsync
    {
        /// <summary>
        /// Create an instance of the <see cref="AzureBlobStreamDataSource"/> class.
        /// </summary>
        /// <param name="connectionString">Provide the Azure Storage connections string.</param>
        /// <param name="containerName">Provide the container name where the files will be stored.</param>
        public AzureBlobStreamDataSource(string connectionString, string containerName)
            : base(new SimpleAzureBlobStorage(connectionString, containerName)) { }
    }
}