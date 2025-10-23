using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;

namespace DemandLink.Storage.AzureBlob
{
    /// <summary>
    /// A class designed to manage blobs in a single container on a single
    /// Azure Blob storage account.
    /// </summary>
    public class SimpleAzureBlobStorage : ISimpleFileStorage
    {
        private readonly string connectionString;
        private readonly string containerName;

        /// <summary>
        /// Create an instance of the <see cref="SimpleAzureBlobStorage"/> class.
        /// </summary>
        /// <param name="connectionString">Provide the Azure Storage connections string.</param>
        /// <param name="containerName">Provide the container name where the files will be stored.</param>
        public SimpleAzureBlobStorage(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        /// <summary>
        /// Create a file blob in Azure Blob storage.
        /// </summary>
        /// <inheritdoc/>
        public async Task<string> CreateFileAsync(string fileName, Stream stream, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = createBlobContainerClient();

            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(stream, overwrite, cancellationToken);

            return fileName;
        }

        /// <summary>
        /// Delete a file from Azure Blob storage.
        /// </summary>
        /// <inheritdoc/>
        public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = createBlobContainerClient();

            if ((await blobContainerClient.ExistsAsync()) == false)
            {
                return false;
            }

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Download a file <see cref="Stream"/> from Azure Blob storage.
        /// </summary>
        /// <inheritdoc/>
        public async Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = createBlobContainerClient();

            if ((await blobContainerClient.ExistsAsync()) == false)
            {
                return default;
            }

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            try
            {
                var blobDownloadResultResponse = await blobClient.DownloadContentAsync(cancellationToken);
                return blobDownloadResultResponse?.Value?.Content?.ToStream();
            }
            catch (RequestFailedException)
            {
                throw new FileDoesNotExistException(
                    $"[{nameof(SimpleAzureBlobStorage)}][{nameof(DownloadFileAsync)}]({nameof(fileName)}, {nameof(cancellationToken)})",
                    fileName
                );
            }
        }

        /// <summary>
        /// Check if a file name exists in Azure Blob storage.
        /// </summary>
        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = createBlobContainerClient();

            if ((await blobContainerClient.ExistsAsync()) == false)
            {
                return false;
            }

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            return blobClient.Exists(cancellationToken);
        }

        /// <summary>
        /// Create an instance of the <see cref="BlobContainerClient"/> class with the current connection string
        /// and container name.
        /// </summary>
        /// <returns>An instance of the <see cref="BlobContainerClient"/> class.</returns>
        private BlobContainerClient createBlobContainerClient()
        {
            try
            {
                return new BlobContainerClient(connectionString, containerName);
            }
            catch (Exception)
            {
                throw new CannotAccessStorageException(
                    $"[{nameof(SimpleAzureBlobStorage)}][{nameof(createBlobContainerClient)}]()",
                    "Possibly incorrect connection string"
                );
            }
        }
    }
}