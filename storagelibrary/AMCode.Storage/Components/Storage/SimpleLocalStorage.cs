using AMCode.Common.Models;
using AMCode.Storage.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMCode.Storage.Components.Storage
{
    /// <summary>
    /// Simple local file storage implementation
    /// </summary>
    public class SimpleLocalStorage : ISimpleFileStorage
    {
        private readonly IStorageLogger _logger;
        private readonly string _basePath;

        public SimpleLocalStorage(IStorageLogger logger, string basePath = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _basePath = basePath ?? Path.Combine(Directory.GetCurrentDirectory(), "storage");
            
            // Ensure base directory exists
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<Result<string>> StoreFileAsync(Stream fileStream, string fileName, string folderPath = null)
        {
            try
            {
                _logger.LogInformation("Storing file: {FileName} in folder: {FolderPath}", fileName, folderPath);
                
                var fullPath = Path.Combine(_basePath, folderPath ?? "", fileName);
                var directory = Path.GetDirectoryName(fullPath);
                
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                using (var fileStreamOut = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await fileStream.CopyToAsync(fileStreamOut);
                }
                
                _logger.LogInformation("File stored successfully: {FilePath}", fullPath);
                return Result<string>.Success(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store file: {FileName}", fileName);
                return Result<string>.Failure($"Failed to store file: {ex.Message}");
            }
        }

        public async Task<Result<Stream>> GetFileAsync(string filePath)
        {
            try
            {
                _logger.LogInformation("Retrieving file: {FilePath}", filePath);
                
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("File not found: {FilePath}", filePath);
                    return Result<Stream>.Failure("File not found");
                }
                
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                _logger.LogInformation("File retrieved successfully: {FilePath}", filePath);
                return Result<Stream>.Success(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve file: {FilePath}", filePath);
                return Result<Stream>.Failure($"Failed to retrieve file: {ex.Message}");
            }
        }

        public async Task<Result> DeleteFileAsync(string filePath)
        {
            try
            {
                _logger.LogInformation("Deleting file: {FilePath}", filePath);
                
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                    return Result.Failure("File not found");
                }
                
                File.Delete(filePath);
                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file: {FilePath}", filePath);
                return Result.Failure($"Failed to delete file: {ex.Message}");
            }
        }

        public async Task<bool> FileExistsAsync(string filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if file exists: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<Result<string[]>> ListFilesAsync(string directoryPath)
        {
            try
            {
                _logger.LogInformation("Listing files in directory: {DirectoryPath}", directoryPath);
                
                var fullPath = Path.Combine(_basePath, directoryPath);
                
                if (!Directory.Exists(fullPath))
                {
                    _logger.LogWarning("Directory not found: {DirectoryPath}", fullPath);
                    return Result<string[]>.Success(new string[0]);
                }
                
                var files = Directory.GetFiles(fullPath).Select(f => Path.GetFileName(f)).ToArray();
                _logger.LogInformation("Found {FileCount} files in directory: {DirectoryPath}", files.Length, fullPath);
                return Result<string[]>.Success(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list files in directory: {DirectoryPath}", directoryPath);
                return Result<string[]>.Failure($"Failed to list files: {ex.Message}");
            }
        }
    }
}
