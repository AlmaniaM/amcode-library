using System.Collections.Generic;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers.Models;
using Docker.DotNet;

namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// A class used to create a Docker container with a database inside of it.
    /// </summary>
    public class DbContainer : IDbContainer
    {
        private readonly IDockerClient dockerClient;

        /// <summary>
        /// Create an instance of the <see cref="DbContainer"/> class.
        /// </summary>
        /// <param name="dockerClient">Provide a <see cref="IDockerClient"/> to communicate with.</param>
        public DbContainer(IDockerClient dockerClient)
        {
            this.dockerClient = dockerClient;
        }

        /// <summary>
        /// Start up a Docker container with a Vertica database running inside of it.
        /// </summary>
        /// <param name="runSetupParams">A <see cref="IContainerSetupParams"/> object with every property filled out.</param>
        /// <returns>The value true if setup was successful. And false if not.</returns>
        public async Task<bool> SetupAsync(IContainerSetupParams runSetupParams)
        {
            if (runSetupParams.VolumeMaps != null)
            {
                foreach (var keyVal in runSetupParams.VolumeMaps)
                {
                    var verticaVolume = new DockerVolume(dockerClient);
                    var volumeName = keyVal.Key;

                    if (!await verticaVolume.ExistsAsync(volumeName))
                    {
                        await verticaVolume.CreateAsync(volumeName);
                    }
                }
            }

            var container = new DockerContainer(dockerClient);

            var runParams = new DockerRunParams
            {
                ActionForWhenAlreadyRunning = runSetupParams.ActionForWhenAlreadyRunning,
                AutoRemoveContainer = runSetupParams.AutoRemoveContainer,
                ContainerName = runSetupParams.ContainerName,
                ImageName = runSetupParams.ImageName,
                ImageTag = runSetupParams.ImageTag,
                Password = runSetupParams.Password,
                Ports = runSetupParams.PortMappings,
                UserName = runSetupParams.UserName,
                Volumes = runSetupParams.VolumeMaps
            };

            if (!runSetupParams.DbName.IsNull() || !runSetupParams.DbPassword.IsNull())
            {
                runParams.EnvironmentVariables = new Dictionary<string, string>();

                if (!runSetupParams.DbName.IsNull())
                {
                    runParams.EnvironmentVariables.Add(runSetupParams.DbName.Key, runSetupParams.DbName.Value);
                }

                if (!runSetupParams.DbPassword.IsNull())
                {
                    runParams.EnvironmentVariables.Add(runSetupParams.DbPassword.Key, runSetupParams.DbPassword.Value);
                }
            }

            var containerStarted = await container.RunAsync(runParams);

            return containerStarted;
        }

        /// <summary>
        /// Shuts down the running container with the given name.
        /// </summary>
        /// <param name="containerName">The name of the container you want to stop.</param>
        /// <param name="waitForContainerToStop">Provide true if you would like to wait for the container to
        /// completely stop.</param>
        /// <returns>True if the container was successfully shut down. False if not.</returns>
        public async Task<bool> TeardownAsync(string containerName, bool waitForContainerToStop = false)
        {
            var container = new DockerContainer(dockerClient);
            var stopped = await container.StopAsync(containerName);

            if (waitForContainerToStop)
            {
                stopped = await container.WaitForContainerToStop(containerName);
            }

            return stopped;
        }

        /// <summary>
        /// Shuts down the running container with the given name.
        /// </summary>
        /// <param name="containerName">The name of the container you want to stop.</param>
        /// <returns>True if the container was successfully shut down. False if not.</returns>
        public async Task<bool> TeardownAndRemoveAsync(string containerName)
        {
            var container = new DockerContainer(dockerClient);
            var stopped = await container.StopAsync(containerName);

            await container.StopAsync(containerName, true);
            await container.RemoveAsync(containerName);

            return stopped;
        }
    }

    /// <summary>
    /// A configuration class for starting up a DB Docker container.
    /// </summary>
    public class ContainerSetupParams : IContainerSetupParams
    {
        /// <summary>
        /// What to do if a container is already running.
        /// </summary>
        public DockerRunAlreadyRunningAction ActionForWhenAlreadyRunning { get; set; }

        /// <summary>
        /// Whether or not to remove the container once it's done running.
        /// </summary>
        public bool AutoRemoveContainer { get; set; }

        /// <summary>
        /// The name you want to give to the Docker container.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Provide a <see cref="KeyValuePair{TKey, TValue}"/> with the key being the name of the database name environment variable
        /// and the value being the database name.
        /// </summary>
        public KeyValuePair<string, string> DbName { get; set; }

        /// <summary>
        /// Provide a <see cref="KeyValuePair{TKey, TValue}"/> with the key being the name of the password environment variable
        /// and the value being the password.
        /// </summary>
        public KeyValuePair<string, string> DbPassword { get; set; }

        /// <summary>
        /// The name of the image to build the container from.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// The tag version of the image you want to build the container from.
        /// </summary>
        public string ImageTag { get; set; }

        /// <summary>
        /// The password of the account to access private Docker images.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> key values. Each key
        /// must be a port of the host machine. Each value is the container port. Each key/value will be mapped
        /// from host machine to the container.
        /// </summary>
        public IDictionary<string, string> PortMappings { get; set; }

        /// <summary>
        /// The username to use when accessing private Docker images.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> VolumeMaps { get; set; }
    }

    /// <summary>
    /// Docker run actions to take when in certain states.
    /// </summary>
    public enum DockerRunAlreadyRunningAction
    {
        /// <summary>
        /// Don't run the container if it's already running.
        /// </summary>
        DontRunNewInstance,
        /// <summary>
        /// Run if the container exists but isn't running.
        /// </summary>
        RunIfExists,
        /// <summary>
        /// Stop the container if it's already running and re-run.
        /// </summary>
        StopExistingAndRunNew
    }

    internal static class Extensions
    {
        public static bool IsNull<K, V>(this KeyValuePair<K, V> kvp) => kvp.Key == null && kvp.Value == null;
    }
}