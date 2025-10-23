using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers.Models;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// A class that represents a Docker Volume with some simple functions.
    /// </summary>
    public class DockerVolume : IDockerVolume
    {
        private readonly IDockerClient dockerClient;

        /// <summary>
        /// Create an instance of the <see cref="DockerVolume"/> class.
        /// </summary>
        /// <param name="dockerClient">Provide a <see cref="IDockerClient"/> to communicate with.</param>
        public DockerVolume(IDockerClient dockerClient)
        {
            this.dockerClient = dockerClient;
        }

        /// <summary>
        /// Creates a volume.
        /// </summary>
        /// <param name="volumeName">The volume you are looking for.</param>
        /// <returns></returns>
        public async Task<VolumeResponse> CreateAsync(string volumeName)
        {
            return await dockerClient.Volumes.CreateAsync(new VolumesCreateParameters
            {
                Driver = "local",
                Name = volumeName,
            });
        }

        /// <summary>
        /// Forcefully removes a Docker volume by it's name.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public async Task DeleteVolumeAsync(string volumeName) => await DeleteVolumeAsync(volumeName, true);

        /// <summary>
        /// Removes a Docker volume by it's name.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public async Task DeleteVolumeAsync(string volumeName, bool? force = null) => await dockerClient.Volumes.RemoveAsync(volumeName, force);

        /// <summary>
        /// Checks if the volume already exists.
        /// </summary>
        /// <param name="volumeName">The volume you are looking for.</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string volumeName)
        {
            var volumesResponse = await dockerClient.Volumes.ListAsync();

            if (volumesResponse.Volumes.Count > 0)
            {
                var volumeToFind = volumesResponse.Volumes.FirstOrDefault(volume => volume.Name.Equals(volumeName));

                return volumeToFind != null;
            }

            return false;
        }
    }
}