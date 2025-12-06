using AMCode.Common.Models;
using AMCode.Storage.Interfaces;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AMCode.Storage.Components.Storage
{
    /// <summary>
    /// Azure Blob Storage implementation of ISimpleFileStorage
    /// </summary>
    public class AzureBlobStorage : ISimpleFileStorage
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly IStorageLogger _logger;

        /// <summary>
        /// Initializes a new instance of the AzureBlobStorage class
        /// </summary>
        /// <param name="connectionString">Azure Storage connection string</param>
        /// <param name="containerName">Container name for blob storage</param>
        /// <param name="logger">Logger for storage operations</param>
        public AzureBlobStorage(string connectionString, string containerName, IStorageLogger logger)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _containerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Stores a file in Azure Blob Storage
        /// </summary>
        /// <param name="fileStream">The file stream to store</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="folderPath">The folder path where to store the file</param>
        /// <returns>Result containing the stored file path</returns>
        public async Task<Result<string>> StoreFileAsync(Stream fileStream, string fileName, string folderPath = null)
        {
            try
            {
                _logger.LogInformation("Storing file: {FileName} in folder: {FolderPath}", fileName, folderPath);

                var blobPath = CombinePath(folderPath, fileName);
                var blobContainerClient = CreateBlobContainerClient();

                // Create container if it doesn't exist
                await blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = blobContainerClient.GetBlobClient(blobPath);

                // Reset stream position if needed
                if (fileStream.CanSeek && fileStream.Position > 0)
                {
                    fileStream.Position = 0;
                }

                await blobClient.UploadAsync(fileStream, overwrite: true);

                _logger.LogInformation("File stored successfully: {BlobPath}", blobPath);
                return Result<string>.Success(blobPath);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Failed to store file in Azure Blob Storage: {FileName}", fileName);
                return Result<string>.Failure($"Failed to store file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error storing file: {FileName}", fileName);
                return Result<string>.Failure($"Failed to store file: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a file from Azure Blob Storage
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Result containing the file stream</returns>
        public async Task<Result<Stream>> GetFileAsync(string filePath)
        {
            try
            {
                _logger.LogInformation("Retrieving file: {FilePath}", filePath);

                var blobContainerClient = CreateBlobContainerClient();
                var blobClient = blobContainerClient.GetBlobClient(filePath);

                if (!await blobClient.ExistsAsync())
                {
                    _logger.LogWarning("File not found: {FilePath}", filePath);
                    return Result<Stream>.Failure("File not found");
                }

                var downloadResult = await blobClient.DownloadContentAsync();
                var stream = new MemoryStream(downloadResult.Value.Content.ToArray());

                _logger.LogInformation("File retrieved successfully: {FilePath}", filePath);
                return Result<Stream>.Success(stream);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                _logger.LogWarning("File not found: {FilePath}", filePath);
                return Result<Stream>.Failure("File not found");
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Failed to retrieve file from Azure Blob Storage: {FilePath}", filePath);
                return Result<Stream>.Failure($"Failed to retrieve file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving file: {FilePath}", filePath);
                return Result<Stream>.Failure($"Failed to retrieve file: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a file from Azure Blob Storage
        /// </summary>
        /// <param name="filePath">The path to the file to delete</param>
        /// <returns>Result indicating success or failure</returns>
        public async Task<Result> DeleteFileAsync(string filePath)
        {
            try
            {
                _logger.LogInformation("Deleting file: {FilePath}", filePath);

                var blobContainerClient = CreateBlobContainerClient();
                var blobClient = blobContainerClient.GetBlobClient(filePath);

                var deleted = await blobClient.DeleteIfExistsAsync();

                if (!deleted)
                {
                    _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                    return Result.Failure("File not found");
                }

                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                return Result.Success();
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Failed to delete file from Azure Blob Storage: {FilePath}", filePath);
                return Result.Failure($"Failed to delete file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting file: {FilePath}", filePath);
                return Result.Failure($"Failed to delete file: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a file exists in Azure Blob Storage
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>True if the file exists, false otherwise</returns>
        public async Task<bool> FileExistsAsync(string filePath)
        {
            try
            {
                var blobContainerClient = CreateBlobContainerClient();
                var blobClient = blobContainerClient.GetBlobClient(filePath);
                return await blobClient.ExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if file exists: {FilePath}", filePath);
                return false;
            }
        }

        /// <summary>
        /// Lists files in a directory in Azure Blob Storage
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Result containing the list of file paths</returns>
        public async Task<Result<string[]>> ListFilesAsync(string directoryPath)
        {
            try
            {
                _logger.LogInformation("Listing files in directory: {DirectoryPath}", directoryPath);

                var blobContainerClient = CreateBlobContainerClient();

                // Ensure container exists
                if (!await blobContainerClient.ExistsAsync())
                {
                    _logger.LogWarning("Container does not exist: {ContainerName}", _containerName);
                    return Result<string[]>.Success(Array.Empty<string>());
                }

                var prefix = string.IsNullOrEmpty(directoryPath) ? null : NormalizePath(directoryPath) + "/";
                var filePaths = new List<string>();

                await foreach (var blobItem in blobContainerClient.GetBlobsAsync(prefix: prefix))
                {
                    // Only include blobs (not directories)
                    if (blobItem.Properties.BlobType == BlobType.Block)
                    {
                        filePaths.Add(blobItem.Name);
                    }
                }

                _logger.LogInformation("Found {FileCount} files in directory: {DirectoryPath}", filePaths.Count, directoryPath);
                return Result<string[]>.Success(filePaths.ToArray());
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Failed to list files in Azure Blob Storage: {DirectoryPath}", directoryPath);
                return Result<string[]>.Failure($"Failed to list files: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error listing files: {DirectoryPath}", directoryPath);
                return Result<string[]>.Failure($"Failed to list files: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a blob URL for the given file path
        /// </summary>
        /// <param name="filePath">The file path in blob storage</param>
        /// <returns>The full blob URL</returns>
        public string GenerateBlobUrl(string filePath)
        {
            try
            {
                var accountName = ExtractAccountName(_connectionString);
                if (string.IsNullOrEmpty(accountName))
                {
                    _logger.LogWarning("Could not extract account name from connection string");
                    return string.Empty;
                }

                var normalizedPath = NormalizePath(filePath);
                var url = $"https://{accountName}.blob.core.windows.net/{_containerName}/{normalizedPath}";
                
                _logger.LogDebug("Generated blob URL: {Url}", url);
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate blob URL for: {FilePath}", filePath);
                return string.Empty;
            }
        }

        /// <summary>
        /// Creates a BlobContainerClient instance
        /// </summary>
        private BlobContainerClient CreateBlobContainerClient()
        {
            try
            {
                return new BlobContainerClient(_connectionString, _containerName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create BlobContainerClient");
                throw new InvalidOperationException("Failed to create Azure Blob Storage client. Check connection string.", ex);
            }
        }

        /// <summary>
        /// Combines folder path and file name into a blob path
        /// </summary>
        private string CombinePath(string folderPath, string fileName)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return NormalizePath(fileName);
            }

            var combined = $"{NormalizePath(folderPath)}/{NormalizePath(fileName)}";
            return combined;
        }

        /// <summary>
        /// Normalizes a path by removing leading/trailing slashes and normalizing separators
        /// </summary>
        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            // Replace backslashes with forward slashes
            path = path.Replace('\\', '/');

            // Remove leading and trailing slashes
            path = path.Trim('/');

            return path;
        }

        /// <summary>
        /// Extracts the account name from an Azure Storage connection string
        /// </summary>
        private string ExtractAccountName(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            // Pattern to match AccountName=value
            var match = Regex.Match(connectionString, @"AccountName=([^;]+)", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value.Trim();
            }

            return null;
        }
    }
}

