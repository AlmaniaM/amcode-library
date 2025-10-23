using System;
using System.Collections.Generic;
using AMCode.Data.Logging.Configuration;

namespace AMCode.Data.Logging.Infrastructure
{
    /// <summary>
    /// Logger factory implementation
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        private readonly LoggingConfiguration _configuration;
        private readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();
        private readonly ILoggerProvider _provider;

        public LoggerFactory(ILoggerProvider provider = null, LoggingConfiguration configuration = null)
        {
            _provider = provider ?? new NoOpLoggerProvider();
            _configuration = configuration ?? new LoggingConfiguration();
        }

        public ILogger CreateLogger(string category)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentException("Category cannot be null or empty", nameof(category));

            if (!_loggers.TryGetValue(category, out var logger))
            {
                logger = _provider.CreateLogger(category);
                _loggers[category] = logger;
            }

            return logger;
        }

        public ILogger<T> CreateLogger<T>()
        {
            var category = typeof(T).FullName;
            return new Logger<T>(CreateLogger(category));
        }
    }
}
