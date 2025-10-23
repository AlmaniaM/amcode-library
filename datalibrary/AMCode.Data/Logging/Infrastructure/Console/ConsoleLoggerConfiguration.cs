namespace AMCode.Data.Logging.Infrastructure.Console
{
    /// <summary>
    /// Configuration for console logger
    /// </summary>
    public class ConsoleLoggerConfiguration
    {
        public bool EnableColor { get; set; } = true;
        public bool EnableTimestamp { get; set; } = true;
    }
}
