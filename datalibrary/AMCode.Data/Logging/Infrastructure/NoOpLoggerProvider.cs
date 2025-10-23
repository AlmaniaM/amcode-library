namespace AMCode.Data.Logging.Infrastructure
{
    /// <summary>
    /// No-operation logger provider for backward compatibility
    /// </summary>
    public class NoOpLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string category)
        {
            return new NoOpLogger();
        }
    }
}
