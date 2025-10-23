using System;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace DlCommonTestingLibrary.UnitTests.Containers.DbContainerTests.Models
{
    public class DbContainerHelpers
    {
        /// <summary>
        /// Waits for the provided delegate to succeed or for the timeout to expire.
        /// </summary>
        /// <param name="dockerClient">The <see cref="IDockerClient"/> of which to read logs from.</param>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="success">A <see cref="Func{T, TResult}"/> that reads the log output and returns a boolean
        /// of whether it's done waiting or not. Return <see cref="true"/> if the process can continue and
        /// <see cref="false"/> if not.</param>
        /// <returns>Returns <see cref="true"/> if the process can continue and
        /// <see cref="false"/> if not.</returns>
        public static async Task<bool> WaitFor(IDockerClient dockerClient, string containerName, Func<string, bool> success)
        {
            var containerId = await new DockerContainer(dockerClient).GetIdAsync(containerName);
            var MAX_WAIT_TIME = 60000;
            var timePassed = 0;

            while (timePassed < MAX_WAIT_TIME)
            {
                var logs = await dockerClient.Containers.GetContainerLogsAsync(containerId, false, new ContainerLogsParameters
                {
                    ShowStdout = true
                });

                var (stdout, stderr) = await logs.ReadOutputToEndAsync(new CancellationToken());

                if (success(stdout))
                {
                    return true;
                }

                await Task.Run(() => Thread.Sleep(1000));
                timePassed += 1000;
            }

            return false;
        }
    }
}