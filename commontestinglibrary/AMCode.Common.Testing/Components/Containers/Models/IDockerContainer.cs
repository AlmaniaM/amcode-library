using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace AMCode.Common.Testing.Containers.Models
{
    /// <summary>
    /// An interface representing a Docker container.
    /// </summary>
    public interface IDockerContainer
    {
        /// <summary>
        /// Get a container by its name.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <returns>A <see cref="ContainerListResponse"/> instance.</returns>
        Task<ContainerListResponse> GetContainerAsync(string containerName);

        /// <summary>
        /// Get the <see cref="IContainerState"/> of the named container.
        /// </summary>
        /// <param name="stateParams">A <see cref="IGetContainerStateParams"/> to set whether to search for container
        /// by name or ID.</param>
        /// <returns>An instance of a <see cref="IContainerState"/>.</returns>
        Task<IContainerState> GetContainerStateAsync(IGetContainerStateParams stateParams);

        /// <summary>
        /// Get the container ID based on its name.
        /// </summary>
        /// <param name="containerName">The name of the container you are looking for.</param>
        /// <returns>The <see cref="string"/> ID of the container.</returns>
        Task<string> GetIdAsync(string containerName);

        /// <summary>
        /// Inspect a container.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <returns>An instance of the <see cref="ContainerInspectResponse"/> class.</returns>
        Task<ContainerInspectResponse> InspectContainer(string containerName);

        /// <summary>
        /// Remove a Docker container by its name.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <returns></returns>
        Task RemoveAsync(string containerName);

        /// <summary>
        /// Delete a Docker container.
        /// </summary>
        /// <param name="removeParams">The configured removal parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request if needed.</param>
        /// <returns></returns>
        Task RemoveAsync(ContainerRemoveParams removeParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks to see if a container with the provided name is running.
        /// </summary>
        /// <param name="containerName">The name of the container you want to check for.</param>
        /// <returns>True if the container is running. False if not.</returns>
        Task<bool> IsRunningAsync(string containerName);

        /// <summary>
        /// Startup a Docker container with the provided configurations.
        /// </summary>
        /// <param name="dockerRunParams">An instance of <see cref="IDockerRunParams"/> with all configurations you want
        /// to apply to the container.</param>
        /// <returns>True if the container was stared successfully. False if not.</returns>
        Task<bool> RunAsync(IDockerRunParams dockerRunParams);

        /// <summary>
        /// Stops the given container by name.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="waitForContainerToStop">Provide true if you would like to wait for the container to
        /// completely stop.</param>
        /// <returns></returns>
        Task<bool> StopAsync(string containerName, bool waitForContainerToStop = false);

        /// <summary>
        /// Wait for the docker container to stop running.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="maxWaitTime">The maximum time to wait before failing the call.</param>
        /// <returns>true if the container stopped and false if not.</returns>
        Task<bool> WaitForContainerToStop(string containerName, double maxWaitTime = 10000);
    }

    /// <summary>
    /// A class designed to hold options when removing a container.
    /// </summary>
    public class ContainerRemoveParams
    {
        /// <summary>
        /// Remove the container by its name.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Remove the container by its ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Remove Volumes connected to this container.
        /// </summary>
        public bool? RemoveVolumes { get; set; }

        /// <summary>
        /// Remove any links to this container.
        /// </summary>
        public bool? RemoveLinks { get; set; }

        /// <summary>
        /// Force remove the container.
        /// </summary>
        public bool? Force { get; set; }
    }
}