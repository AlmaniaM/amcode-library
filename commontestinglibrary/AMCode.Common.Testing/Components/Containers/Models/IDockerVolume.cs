using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace AMCode.Common.Testing.Containers.Models
{
    /// <summary>
    /// An interface representing a Docker volume.
    /// </summary>
    public interface IDockerVolume
    {
        /// <summary>
        /// Creates a volume.
        /// </summary>
        /// <param name="volumeName">The volume you are looking for.</param>
        /// <returns></returns>
        Task<VolumeResponse> CreateAsync(string volumeName);

        /// <summary>
        /// Forcefully removes a Docker volume by it's name.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        Task DeleteVolumeAsync(string volumeName);

        /// <summary>
        /// Removes a Docker volume by it's name.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        Task DeleteVolumeAsync(string volumeName, bool? force = null);

        /// <summary>
        /// Checks if the volume already exists.
        /// </summary>
        /// <param name="volumeName">The volume you are looking for.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string volumeName);
    }
}