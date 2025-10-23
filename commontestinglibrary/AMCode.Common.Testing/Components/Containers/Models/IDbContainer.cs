using System.Threading.Tasks;

namespace AMCode.Common.Testing.Containers.Models
{
    /// <summary>
    /// An interface representing a database container.
    /// </summary>
    public interface IDbContainer
    {
        /// <summary>
        /// Start up a Docker container with a Vertica database running inside of it.
        /// </summary>
        /// <param name="runSetupParams">An instance of <see cref="IContainerSetupParams"/> with every property filled out.</param>
        /// <returns></returns>
        Task<bool> SetupAsync(IContainerSetupParams runSetupParams);

        /// <summary>
        /// Shuts down the running container with the given name.
        /// </summary>
        /// <param name="containerName">The name of the container you want to stop.</param>
        /// <param name="waitForContainerToStop">Provide true if you would like to wait for the container to
        /// completely stop.</param>
        /// <returns>True if the container was successfully shut down. False if not.</returns>
        Task<bool> TeardownAsync(string containerName, bool waitForContainerToStop = false);

        /// <summary>
        /// Shuts down the running container with the given name.
        /// </summary>
        /// <param name="containerName">The name of the container you want to stop.</param>
        /// <returns>True if the container was successfully shut down. False if not.</returns>
        Task<bool> TeardownAndRemoveAsync(string containerName);
    }
}