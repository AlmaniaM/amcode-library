using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using ContainerState = AMCode.Common.Testing.Containers.Models.ContainerState;

namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// A class that represents a Docker container with some simple functions.
    /// </summary>
    public class DockerContainer : IDockerContainer
    {
        private readonly IDockerClient dockerClient;

        /// <summary>
        /// Create an instance of the <see cref="DockerContainer"/> class.
        /// </summary>
        /// <param name="dockerClient">Provide a <see cref="IDockerClient"/> to communicate with.</param>
        public DockerContainer(IDockerClient dockerClient)
        {
            this.dockerClient = dockerClient;
        }

        /// <inheritdoc/>
        public async Task<ContainerListResponse> GetContainerAsync(string containerName)
        {
            var containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["name"] = new Dictionary<string, bool>
                    {
                        [containerName] = true
                    }
                },
                Limit = 1
            });

            return containers.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IContainerState> GetContainerStateAsync(IGetContainerStateParams stateParams)
        {
            if (string.IsNullOrEmpty(stateParams.ContainerName) && string.IsNullOrEmpty(stateParams.Id))
            {
                throw new Exception($"[{nameof(DockerContainer)}][{nameof(GetContainerStateAsync)}]({nameof(stateParams)}) Both ContainerName and Id properties of {nameof(IGetContainerStateParams)} cannot be null or empty.");
            }

            var containerId = stateParams.Id ?? (await GetContainerAsync(stateParams.ContainerName))?.ID;

            if (string.IsNullOrEmpty(containerId))
            {
                return new ContainerState { Status = "none" };
            }

            var containerInspectResponse = await dockerClient.Containers.InspectContainerAsync(containerId);
            var state = containerInspectResponse.State;

            return new ContainerState
            {
                Dead = state.Dead,
                Error = state.Error,
                ExitCode = state.ExitCode,
                FinishedAt = state.FinishedAt,
                OOMKilled = state.OOMKilled,
                Paused = state.Paused,
                Pid = state.Pid,
                Restarting = state.Restarting,
                Running = state.Running,
                StartedAt = state.StartedAt,
                Status = state.Status
            };
        }

        /// <inheritdoc/>
        public async Task<string> GetIdAsync(string containerName)
        {
            var container = await GetContainerAsync(containerName);

            if (container == null || container == default(ContainerListResponse))
            {
                return string.Empty;
            }

            return container.ID;
        }

        /// <inheritdoc/>
        public async Task<ContainerInspectResponse> InspectContainer(string containerName)
        {
            var containerId = await GetIdAsync(containerName);
            if (containerId.Equals(string.Empty))
            {
                return default;
            }

            var inspectResponse = await dockerClient.Containers.InspectContainerAsync(containerId);

            return inspectResponse;
        }

        /// <inheritdoc/>
        public async Task<bool> IsRunningAsync(string containerName)
        {
            var runningContainer = await GetContainerAsync(containerName);

            if (runningContainer == null)
            {
                return false;
            }

            var containerState = runningContainer.State;

            return containerState.Equals("running") || containerState.Equals("paused");
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string containerName)
        {
            await RemoveAsync(new ContainerRemoveParams
            {
                ContainerName = containerName,
                Force = true,
                RemoveLinks = false,
                RemoveVolumes = false
            });
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(ContainerRemoveParams removeParams, CancellationToken cancellationToken = default)
        {
            var containerId = string.Empty;

            if (string.IsNullOrEmpty(removeParams.ID) && !string.IsNullOrEmpty(removeParams.ContainerName))
            {
                containerId = (await GetContainerAsync(removeParams.ContainerName))?.ID;
            }
            else if (!string.IsNullOrEmpty(removeParams.ID))
            {
                containerId = removeParams.ID;
            }

            if (string.IsNullOrEmpty(containerId))
            {
                return;
            }

            await dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters
            {
                Force = removeParams.Force,
                RemoveLinks = removeParams.RemoveLinks,
                RemoveVolumes = removeParams.RemoveVolumes
            }, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> RunAsync(IDockerRunParams dockerRunParams)
        {
            var dockerImage = new DockerImage(dockerClient);
            var imageName = dockerRunParams.ImageTag != null ? $"{dockerRunParams.ImageName}:{dockerRunParams.ImageTag}" : dockerRunParams.ImageName;
            var images = await dockerImage.ListImagesAsync(new List<string> { imageName });

            var authConfig = dockerRunParams.UserName == null ? null : new AuthConfig
            {
                Username = dockerRunParams.UserName,
                Password = dockerRunParams.Password
            };

            if (images.Count == 0)
            {
                await new DockerImage(dockerClient).CreateAsync(imageName, authConfig);
            }

            var startParams = new CreateContainerParameters
            {
                AttachStderr = true,
                AttachStdin = true,
                AttachStdout = true,
                Env = dockerRunParams.EnvironmentVariables?.Select(keyVal => $"{keyVal.Key}={keyVal.Value}").ToList(),
                ExposedPorts = dockerRunParams.Ports?.ToDictionary(keyVal => keyVal.Value, keyVal => default(EmptyStruct)),
                HostConfig = new HostConfig
                {
                    AutoRemove = dockerRunParams.AutoRemoveContainer,
                    Binds = dockerRunParams.Volumes?.Select(keyValue =>
                    {
                        var key = string.IsNullOrEmpty(keyValue.Key) ? "" : $"{keyValue.Key}:";
                        return $"{key}{keyValue.Value}";
                    }).ToList(),
                    PortBindings = dockerRunParams.Ports?.ToDictionary(
                        keyVal => keyVal.Value,
                        keyVal => (IList<PortBinding>)new List<PortBinding> {
                        new PortBinding { HostPort = keyVal.Key,  }
                        }),
                    PublishAllPorts = dockerRunParams.PublishAllPorts
                },
                Image = dockerRunParams.ImageTag != null ? $"{dockerRunParams.ImageName}:{dockerRunParams.ImageTag}" : dockerRunParams.ImageName,
                Name = dockerRunParams.ContainerName.ToString(),
                Volumes = dockerRunParams.Volumes?.ToDictionary(keyVal => keyVal.Value, keyVal => default(EmptyStruct)),
                Cmd = dockerRunParams.Cmd,
                Entrypoint = dockerRunParams.Entrypoint,
                Shell = dockerRunParams.Shell
            };

            var state = await getContainerState(dockerRunParams.ContainerName);

            if (dockerRunParams.ActionForWhenAlreadyRunning == DockerRunAlreadyRunningAction.StopExistingAndRunNew)
            {
                if (state.Running)
                {
                    await StopAsync(dockerRunParams.ContainerName, true);
                }

                if (!state.Status.Equals("none"))
                {
                    await RemoveAsync(dockerRunParams.ContainerName);
                }
            }

            var containerExists = !state.Status.Equals("none") && !state.Status.ToLowerInvariant().Equals("dead");
            if (containerExists && dockerRunParams.ActionForWhenAlreadyRunning == DockerRunAlreadyRunningAction.RunIfExists)
            {
                var container = await GetContainerAsync(dockerRunParams.ContainerName);
                return await dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
            }

            if (state.Status.Equals("dead"))
            {
                await RemoveAsync(dockerRunParams.ContainerName);
            }

            if (state.Running && dockerRunParams.ActionForWhenAlreadyRunning == DockerRunAlreadyRunningAction.DontRunNewInstance)
            {
                return true;
            }

            var containerResponse = await dockerClient.Containers.CreateContainerAsync(startParams);
            var started = await dockerClient.Containers.StartContainerAsync(containerResponse.ID, new ContainerStartParameters());
            return started;
        }

        /// <inheritdoc/>
        public async Task<bool> StopAsync(string containerName, bool waitForContainerToStop = false)
        {
            var runningContainer = await GetContainerAsync(containerName);

            if (runningContainer != null || runningContainer != default(ContainerListResponse))
            {
                var stopped = await dockerClient.Containers.StopContainerAsync(runningContainer.ID, new ContainerStopParameters());

                if (waitForContainerToStop)
                {
                    stopped = await WaitForContainerToStop(containerName);
                }

                return stopped;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> WaitForContainerToStop(string containerName, double maxWaitTime = 10000)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.ElapsedMilliseconds < maxWaitTime)
            {
                var isStillRunning = await IsRunningAsync(containerName);

                if (isStillRunning)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    stopWatch.Stop();
                    return isStillRunning == false;
                }
            }

            stopWatch.Stop();
            return (await IsRunningAsync(containerName)) == false;
        }

        /// <inheritdoc/>
        private async Task<IContainerState> getContainerState(string containerName)
            => await GetContainerStateAsync(new GetContainerStateParams { ContainerName = containerName }) ?? new ContainerState { Status = string.Empty };
    }

    /// <summary>
    /// A class designed for holding Docker run configurations.
    /// </summary>
    public class DockerRunParams : IDockerRunParams
    {
        /// <inheritdoc/>
        public DockerRunAlreadyRunningAction ActionForWhenAlreadyRunning { get; set; }

        /// <inheritdoc/>
        public bool AutoRemoveContainer { get; set; }

        /// <inheritdoc/>
        public IList<string> Cmd { get; set; }

        /// <inheritdoc/>
        public string ContainerName { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> EnvironmentVariables { get; set; }

        /// <inheritdoc/>
        public IList<string> Entrypoint { get; set; }

        /// <inheritdoc/>
        public string ImageId { get; set; }

        /// <inheritdoc/>
        public string ImageName { get; set; }

        /// <inheritdoc/>
        public string ImageTag { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Ports { get; set; }

        /// <inheritdoc/>
        public bool PublishAllPorts { get; set; }

        /// <inheritdoc/>
        public IList<string> Shell { get; set; }

        /// <inheritdoc/>
        public string UserName { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Volumes { get; set; }
    }
}