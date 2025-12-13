using AMCode.Common.Models;
using AMCode.Storage.Interfaces;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMCode.Storage.Components.Storage
{
    /// <summary>
    /// AWS S3 Storage implementation of ISimpleFileStorage
    /// </summary>
    public class S3Storage : ISimpleFileStorage
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _region;
        private readonly string _bucketName;
        private readonly string? _serviceUrl;
        private readonly IStorageLogger _logger;
        private readonly IAmazonS3 _s3Client;

        /// <summary>
        /// Initializes a new instance of the S3Storage class
        /// </summary>
        /// <param name="accessKey">AWS Access Key ID</param>
        /// <param name="secretKey">AWS Secret Access Key</param>
        /// <param name="region">AWS Region (e.g., "us-east-1")</param>
        /// <param name="bucketName">S3 bucket name</param>
        /// <param name="logger">Logger for storage operations</param>
        /// <param name="serviceUrl">Optional custom service URL for LocalStack or other S3-compatible services</param>
        public S3Storage(string accessKey, string secretKey, string region, string bucketName, IStorageLogger logger, string? serviceUrl = null)
        {
            _accessKey = accessKey ?? throw new ArgumentNullException(nameof(accessKey));
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _region = region ?? throw new ArgumentNullException(nameof(region));
            _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceUrl = serviceUrl;

            // Create S3 client
            var regionEndpoint = RegionEndpoint.GetBySystemName(_region);

            if (!string.IsNullOrEmpty(_serviceUrl))
            {
                // Use custom service URL (e.g., LocalStack)
                var config = new AmazonS3Config
                {
                    ServiceURL = _serviceUrl,
                    ForcePathStyle = true, // Required for LocalStack
                    RegionEndpoint = regionEndpoint
                };
                _s3Client = new AmazonS3Client(_accessKey, _secretKey, config);
                _logger.LogInformation("S3Storage initialized with custom service URL: {ServiceUrl}", _serviceUrl);
            }
            else
            {
                // Use standard AWS S3 endpoints
                _s3Client = new AmazonS3Client(_accessKey, _secretKey, regionEndpoint);
            }
        }

        /// <summary>
        /// Stores a file in AWS S3
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

                var s3Key = CombinePath(folderPath, fileName);

                // Reset stream position if needed
                if (fileStream.CanSeek && fileStream.Position > 0)
                {
                    fileStream.Position = 0;
                }

                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = s3Key,
                    InputStream = fileStream,
                    ContentType = GetContentType(fileName)
                };

                await _s3Client.PutObjectAsync(request);

                _logger.LogInformation("File stored successfully: {S3Key}", s3Key);
                return Result<string>.Success(s3Key);
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "Failed to store file in S3: {FileName}", fileName);
                return Result<string>.Failure($"Failed to store file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error storing file: {FileName}", fileName);
                return Result<string>.Failure($"Failed to store file: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a file from AWS S3
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Result containing the file stream</returns>
        public async Task<Result<Stream>> GetFileAsync(string filePath)
        {
            try
            {
                _logger.LogInformation("Retrieving file: {FilePath}", filePath);

                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = NormalizePath(filePath)
                };

                var response = await _s3Client.GetObjectAsync(request);

                if (response == null)
                {
                    _logger.LogWarning("File not found: {FilePath}", filePath);
                    return Result<Stream>.Failure("File not found");
                }

                var stream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(stream);
                stream.Position = 0;

                _logger.LogInformation("File retrieved successfully: {FilePath}", filePath);
                return Result<Stream>.Success(stream);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("File not found: {FilePath}", filePath);
                return Result<Stream>.Failure("File not found");
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve file from S3: {FilePath}", filePath);
                return Result<Stream>.Failure($"Failed to retrieve file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving file: {FilePath}", filePath);
                return Result<Stream>.Failure($"Failed to retrieve file: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a file from AWS S3
        /// </summary>
        /// <param name="filePath">The path to the file to delete</param>
        /// <returns>Result indicating success or failure</returns>
        public async Task<Result> DeleteFileAsync(string filePath)
        {
            try
            {
                _logger.LogInformation("Deleting file: {FilePath}", filePath);

                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = NormalizePath(filePath)
                };

                await _s3Client.DeleteObjectAsync(request);

                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                return Result.Success();
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file from S3: {FilePath}", filePath);
                return Result.Failure($"Failed to delete file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting file: {FilePath}", filePath);
                return Result.Failure($"Failed to delete file: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a file exists in AWS S3
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>True if the file exists, false otherwise</returns>
        public async Task<bool> FileExistsAsync(string filePath)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = NormalizePath(filePath)
                };

                await _s3Client.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if file exists: {FilePath}", filePath);
                return false;
            }
        }

        /// <summary>
        /// Lists files in a directory in AWS S3
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Result containing the list of file paths</returns>
        public async Task<Result<string[]>> ListFilesAsync(string directoryPath)
        {
            try
            {
                _logger.LogInformation("Listing files in directory: {DirectoryPath}", directoryPath);

                var prefix = string.IsNullOrEmpty(directoryPath) ? null : NormalizePath(directoryPath) + "/";
                var filePaths = new List<string>();

                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = prefix
                };

                ListObjectsV2Response response;
                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);

                    foreach (var s3Object in response.S3Objects)
                    {
                        // Only include files (not directories)
                        if (!s3Object.Key.EndsWith("/"))
                        {
                            filePaths.Add(s3Object.Key);
                        }
                    }

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                _logger.LogInformation("Found {FileCount} files in directory: {DirectoryPath}", filePaths.Count, directoryPath);
                return Result<string[]>.Success(filePaths.ToArray());
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "Failed to list files in S3: {DirectoryPath}", directoryPath);
                return Result<string[]>.Failure($"Failed to list files: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error listing files: {DirectoryPath}", directoryPath);
                return Result<string[]>.Failure($"Failed to list files: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates an S3 URL for the given file path
        /// </summary>
        /// <param name="filePath">The file path in S3</param>
        /// <returns>The full S3 URL</returns>
        public string GenerateS3Url(string filePath)
        {
            try
            {
                var normalizedPath = NormalizePath(filePath);

                // If using custom service URL (e.g., LocalStack), generate URL accordingly
                if (!string.IsNullOrEmpty(_serviceUrl))
                {
                    // For LocalStack, use path-style URLs
                    var baseUrl = _serviceUrl.TrimEnd('/');
                    var url = $"{baseUrl}/{_bucketName}/{normalizedPath}";
                    _logger.LogDebug("Generated S3 URL (custom service): {Url}", url);
                    return url;
                }

                // Standard AWS S3 URL
                var standardUrl = $"https://{_bucketName}.s3.{_region}.amazonaws.com/{normalizedPath}";
                _logger.LogDebug("Generated S3 URL: {Url}", standardUrl);
                return standardUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate S3 URL for: {FilePath}", filePath);
                return string.Empty;
            }
        }

        /// <summary>
        /// Generates a presigned URL for secure, time-limited access to an S3 object
        /// </summary>
        /// <param name="filePath">The file path in S3</param>
        /// <param name="expiryMinutes">Expiration time in minutes (default: 15)</param>
        /// <returns>Presigned URL, or empty string on failure</returns>
        public string GeneratePresignedUrl(string filePath, int expiryMinutes = 15)
        {
            try
            {
                var normalizedPath = NormalizePath(filePath);

                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = normalizedPath,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddMinutes(expiryMinutes)
                };

                // Generate presigned URL using access key credentials
                var presignedUrl = _s3Client.GetPreSignedURL(request);
                
                _logger.LogDebug("Generated presigned S3 URL: {Url} (expires in {Minutes} minutes)", presignedUrl, expiryMinutes);
                return presignedUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate presigned S3 URL for: {FilePath}", filePath);
                return string.Empty;
            }
        }

        /// <summary>
        /// Combines folder path and file name into an S3 key
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
        /// Normalizes a path by removing leading/trailing slashes
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
        /// Gets the content type based on file extension
        /// </summary>
        private string GetContentType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "application/octet-stream";
            }

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".svg" => "image/svg+xml",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".json" => "application/json",
                ".xml" => "application/xml",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }

        /// <summary>
        /// Disposes the S3 client
        /// </summary>
        public void Dispose()
        {
            _s3Client?.Dispose();
        }
    }
}
