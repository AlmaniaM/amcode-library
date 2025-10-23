namespace AMCode.Data.Logging
{
    /// <summary>
    /// Provider interface for creating logger instances
    /// </summary>
    public interface ILoggerProvider
    {
        /// <summary>
        /// Creates a logger for the specified category
        /// </summary>
        ILogger CreateLogger(string category);
    }
}
