using AMCode.Common.Models;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Storage.Interfaces
{
    /// <summary>
    /// Interface for simple file storage operations
    /// </summary>
    public interface ISimpleFileStorage
    {
        /// <summary>
        /// Stores a file and returns the path where it was stored
        /// </summary>
        /// <param name="fileStream">The file stream to store</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="folderPath">The folder path where to store the file</param>
        /// <returns>Result containing the stored file path</returns>
        Task<Result<string>> StoreFileAsync(Stream fileStream, string fileName, string folderPath = null);
        
        /// <summary>
        /// Retrieves a file stream
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Result containing the file stream</returns>
        Task<Result<Stream>> GetFileAsync(string filePath);
        
        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="filePath">The path to the file to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result> DeleteFileAsync(string filePath);
        
        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>True if the file exists, false otherwise</returns>
        Task<bool> FileExistsAsync(string filePath);
        
        /// <summary>
        /// Lists files in a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Result containing the list of file paths</returns>
        Task<Result<string[]>> ListFilesAsync(string directoryPath);
    }
}
