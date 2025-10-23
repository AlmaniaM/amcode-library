using System.Collections.Generic;

namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// An interface designed to hold configuration options for setting up a container.
    /// </summary>
    public interface IContainerSetupParams
    {
        /// <summary>
        /// What to do if a container is already running.
        /// </summary>
        DockerRunAlreadyRunningAction ActionForWhenAlreadyRunning { get; set; }

        /// <summary>
        /// Whether or not to remove the container once it's done running.
        /// </summary>
        bool AutoRemoveContainer { get; set; }

        /// <summary>
        /// The name you want to give to the Docker container.
        /// </summary>
        string ContainerName { get; set; }

        /// <summary>
        /// Provide a <see cref="KeyValuePair{TKey, TValue}"/> with the key being the name of the database name environment variable
        /// and the value being the database name.
        /// </summary>
        KeyValuePair<string, string> DbName { get; set; }

        /// <summary>
        /// Provide a <see cref="KeyValuePair{TKey, TValue}"/> with the key being the name of the password environment variable
        /// and the value being the password.
        /// </summary>
        KeyValuePair<string, string> DbPassword { get; set; }

        /// <summary>
        /// The name of the image to build the container from.
        /// </summary>
        string ImageName { get; set; }

        /// <summary>
        /// The tag version of the image you want to build the container from.
        /// </summary>
        string ImageTag { get; set; }

        /// <summary>
        /// The password of the account to access private Docker images.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> key values. Each key
        /// must be a port of the host machine. Each value is the container port. Each key/value will be mapped
        /// from host machine to the container.
        /// </summary>
        IDictionary<string, string> PortMappings { get; set; }

        /// <summary>
        /// The username to use when accessing private Docker images.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, string> VolumeMaps { get; set; }
    }
}