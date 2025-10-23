namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// An interface representing the various container states.
    /// </summary>
    public interface IContainerState
    {
        /// <summary>
        /// The status of which the container is in. It could be any of the following: created, restarting, running, paused, or exited.
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// Whether or not the container is running.
        /// </summary>
        bool Running { get; set; }

        /// <summary>
        /// Whether or not the container is paused.
        /// </summary>
        bool Paused { get; set; }

        /// <summary>
        /// Whether or not the container is restarting.
        /// </summary>
        bool Restarting { get; set; }

        /// <summary>
        /// Whether or not the container was killed because of a
        /// Out Of Memory circumstance.
        /// </summary>
        bool OOMKilled { get; set; }

        /// <summary>
        /// Whether or not the container is dead.
        /// </summary>
        bool Dead { get; set; }

        /// <summary>
        /// The process ID of the container.
        /// </summary>
        long Pid { get; set; }

        /// <summary>
        /// An exit code if the container has exited.
        /// </summary>
        long ExitCode { get; set; }

        /// <summary>
        /// Any errors reported by the container when it exits.
        /// </summary>
        string Error { get; set; }

        /// <summary>
        /// Timestamp of when the container started running.
        /// </summary>
        string StartedAt { get; set; }

        /// <summary>
        /// Timestamp of when the container finished running.
        /// </summary>
        string FinishedAt { get; set; }
    }
}