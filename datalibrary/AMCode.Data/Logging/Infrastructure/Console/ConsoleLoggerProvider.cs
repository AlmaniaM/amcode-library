using AMCode.Data.Logging.Configuration;

namespace AMCode.Data.Logging.Infrastructure.Console
{
    /// <summary>
    /// Console logger provider
    /// </summary>
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly LoggingConfiguration _configuration;
        private readonly ConsoleLoggerConfiguration _consoleConfig;

        public ConsoleLoggerProvider(LoggingConfiguration configuration = null, ConsoleLoggerConfiguration consoleConfig = null)
        {
            _configuration = configuration ?? new LoggingConfiguration();
            _consoleConfig = consoleConfig ?? new ConsoleLoggerConfiguration();
        }

        public ILogger CreateLogger(string category)
        {
            return new ConsoleLogger(category, this, _configuration, _consoleConfig);
        }
    }
}
