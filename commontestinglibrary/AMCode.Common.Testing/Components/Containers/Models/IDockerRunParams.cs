using System.Collections.Generic;

namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// An interface designed for holding Docker run configurations.
    /// </summary>
    public interface IDockerRunParams
    {
        /// <summary>
        /// What action to take when an instance of the container is already running.
        /// </summary>
        DockerRunAlreadyRunningAction ActionForWhenAlreadyRunning { get; set; }

        /// <summary>
        /// If true, then the container will be automatically removed when the container is shut down.
        /// </summary>
        bool AutoRemoveContainer { get; set; }

        /// <summary>
        /// A list of commands to run that will override the Dockerfile CMD.
        /// </summary>
        IList<string> Cmd { get; set; }

        /// <summary>
        /// The name you want to give to the container.
        /// </summary>
        string ContainerName { get; set; }

        /// <summary>
        /// Provide a dictionary of environment variables to override in the docker container. The key should be the name
        /// of the environment variable and the value is the value of the environment variable.
        /// </summary>
        IDictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// A list of string commands to run that will override the Dockerfile ENTRYPOINT command.
        /// </summary>
        IList<string> Entrypoint { get; set; }

        /// <summary>
        /// The ID of the image you wish to build the container from.
        /// </summary>
        string ImageId { get; set; }

        /// <summary>
        /// The name of the image you wish to build the container from.
        /// </summary>
        string ImageName { get; set; }

        /// <summary>
        /// The tag version of th image you wish to build the container from.
        /// </summary>
        string ImageTag { get; set; }

        /// <summary>
        /// The password of the account to access private Docker images.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Provide a dictionary of port mappings. The key represents the host machine port (A.K.A. Your computer localhost). The
        /// value represents the port in the container.
        /// </summary>
        IDictionary<string, string> Ports { get; set; }

        /// <summary>
        /// Whether or not to publish all ports
        /// </summary>
        bool PublishAllPorts { get; set; }

        /// <summary>
        /// A list of shell commands to run.
        /// </summary>
        IList<string> Shell { get; set; }

        /// <summary>
        /// The username to use when accessing private Docker images.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Provide a dictionary of volume mappings or just volumes. If the key is not an empty string then only the value
        /// will be used. If you provide a key then that key must be a valid existing volume.
        /// </summary>
        IDictionary<string, string> Volumes { get; set; }
    }
}