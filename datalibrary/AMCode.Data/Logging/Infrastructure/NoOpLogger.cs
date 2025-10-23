using System;
using System.Collections.Generic;

namespace AMCode.Data.Logging.Infrastructure
{
    /// <summary>
    /// No-operation logger implementation for backward compatibility
    /// </summary>
    public class NoOpLogger : ILogger
    {
        public void Log(LogLevel level, string message, Exception exception = null, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public void Log(LogLevel level, string message, IDictionary<string, object> properties, Exception exception = null)
        {
            // No operation - maintains backward compatibility
        }

        public void LogInformation(string message, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public void LogWarning(string message, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public void LogError(string message, Exception exception = null, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public void LogDebug(string message, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public void LogTrace(string message, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public void LogPerformance(string operation, TimeSpan duration, object context = null)
        {
            // No operation - maintains backward compatibility
        }

        public ILogger WithContext(string key, object value)
        {
            return this; // Return self for chaining
        }

        public ILogger WithContext(IDictionary<string, object> context)
        {
            return this; // Return self for chaining
        }
    }
}
