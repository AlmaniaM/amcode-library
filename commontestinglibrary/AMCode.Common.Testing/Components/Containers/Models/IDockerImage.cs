using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace AMCode.Common.Testing.Containers.Models
{
    /// <summary>
    /// An interface representing a Docker image.
    /// </summary>
    public interface IDockerImage
    {
        /// <summary>
        /// Currently, this pulls down an image from our private Docker account. A more general
        /// Create function was not needed at the moment.
        /// </summary>
        /// <param name="imageNameWithTag"></param>
        /// <param name="authConfig"></param>
        /// <returns></returns>
        Task CreateAsync(string imageNameWithTag, AuthConfig authConfig);

        /// <summary>
        /// Delete an existing image.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        Task<IList<IDictionary<string, string>>> DeleteImageAsync(string imageName);

        /// <summary>
        /// Get a <see cref="IList{T}"/> of <see cref="ImagesListResponse"/> based on the <see cref="IList{T}"/>
        /// of <see cref="string"/> image names.
        /// </summary>
        /// <param name="imageNames"></param>
        /// <returns></returns>
        Task<IList<ImagesListResponse>> ListImagesAsync(IList<string> imageNames);

        /// <summary>
        /// Lists images based on the provided parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<ImagesListResponse>> ListImagesAsync(ImagesListParameters parameters, CancellationToken cancellationToken = default);
    }
}