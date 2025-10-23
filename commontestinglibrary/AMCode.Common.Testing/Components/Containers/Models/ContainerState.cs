namespace AMCode.Common.Testing.Containers.Models
{
    /// <summary>
    /// A class designed to represent the various states a container can be in.
    /// </summary>
    public class ContainerState : IContainerState
    {
        /// <summary>
        /// The status of which the container is in. It could be any of the following: created, restarting, running, paused, or exited.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Whether or not the container is running.
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// Whether or not the container is paused.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Whether or not the container is restarting.
        /// </summary>
        public bool Restarting { get; set; }

        /// <summary>
        /// Whether or not the container was killed because of a
        /// Out Of Memory circumstance.
        /// </summary>
        public bool OOMKilled { get; set; }

        /// <summary>
        /// Whether or not the container is dead.
        /// </summary>
        public bool Dead { get; set; }

        /// <summary>
        /// The process ID of the container.
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// An exit code if the container has exited.
        /// </summary>
        public long ExitCode { get; set; }

        /// <summary>
        /// Any errors reported by the container when it exits.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Timestamp of when the container started running.
        /// </summary>
        public string StartedAt { get; set; }

        /// <summary>
        /// Timestamp of when the container finished running.
        /// </summary>
        public string FinishedAt { get; set; }
    }

    /// <summary>
    /// A class designed for holding options on how to retrieve a container state.
    /// </summary>
    public class GetContainerStateParams : IGetContainerStateParams
    {
        /// <summary>
        /// Name of the container.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// ID of the container.
        /// </summary>
        public string Id { get; set; }
    }
}