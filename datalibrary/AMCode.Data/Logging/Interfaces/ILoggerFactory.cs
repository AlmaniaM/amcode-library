namespace AMCode.Data.Logging
{
    /// <summary>
    /// Factory interface for creating logger instances
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a logger for the specified category
        /// </summary>
        ILogger CreateLogger(string category);
        
        /// <summary>
        /// Creates a typed logger for the specified type
        /// </summary>
        ILogger<T> CreateLogger<T>();
    }
}
