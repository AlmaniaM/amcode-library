using System;

namespace AMCode.Common.Util
{
    /// <summary>
    /// A class designed to provide helper methods for the <see cref="Environment"/> class.
    /// </summary>
    public static class EnvironmentUtil
    {
        /// <summary>
        /// Get the value of an environment variable. This will check for the variable value
        /// in the process, user, and machine. All in that order.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to retrieve.</param>
        /// <returns>A string value if found and default(string) if not.</returns>
        public static string GetEnvironmentVariable(string variableName)
        {
            var processTargets = new EnvironmentVariableTarget[]
            {
                EnvironmentVariableTarget.Process,
                EnvironmentVariableTarget.User,
                EnvironmentVariableTarget.Machine
            };

            foreach (var target in processTargets)
            {
                var connectionString = Environment.GetEnvironmentVariable(variableName, target);
                if (connectionString != null)
                {
                    return connectionString;
                }
            }

            return default;
        }
    }
}