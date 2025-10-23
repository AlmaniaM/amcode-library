namespace AMCode.Common.Testing.Containers.Models
{
    /// <summary>
    /// An interface representing options to find a container.
    /// </summary>
    public interface IGetContainerStateParams
    {
        /// <summary>
        /// Name of the container.
        /// </summary>
        string ContainerName { get; set; }

        /// <summary>
        /// ID of the container.
        /// </summary>
        string Id { get; set; }
    }
}