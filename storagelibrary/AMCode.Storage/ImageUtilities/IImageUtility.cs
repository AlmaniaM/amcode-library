using AMCode.Storage.ImageUtilities.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Storage.ImageUtilities
{
    /// <summary>
    /// Provides composable image processing utilities for resizing, compression,
    /// format conversion, metadata extraction, thumbnail generation, and EXIF stripping.
    /// </summary>
    public interface IImageUtility
    {
        /// <summary>
        /// Resizes an image according to the specified options.
        /// </summary>
        /// <param name="input">Source image stream</param>
        /// <param name="options">Resize configuration</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Processed image result</returns>
        Task<ImageResult> ResizeAsync(Stream input, ResizeOptions options, CancellationToken ct = default);

        /// <summary>
        /// Compresses an image using the specified compression options.
        /// </summary>
        /// <param name="input">Source image stream</param>
        /// <param name="options">Compression configuration</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Compressed image result</returns>
        Task<ImageResult> CompressAsync(Stream input, CompressionOptions options, CancellationToken ct = default);

        /// <summary>
        /// Converts an image to the specified format.
        /// </summary>
        /// <param name="input">Source image stream</param>
        /// <param name="targetFormat">The target output format</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Converted image result</returns>
        Task<ImageResult> ConvertAsync(Stream input, ImageFormat targetFormat, CancellationToken ct = default);

        /// <summary>
        /// Extracts metadata from an image without loading the full image into memory.
        /// </summary>
        /// <param name="input">Source image stream</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Image metadata including dimensions, format, and EXIF data</returns>
        Task<ImageMetadata> ExtractMetadataAsync(Stream input, CancellationToken ct = default);

        /// <summary>
        /// Generates multiple thumbnail variants from a single source image in one pass.
        /// </summary>
        /// <param name="input">Source image stream</param>
        /// <param name="options">Thumbnail generation configuration</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of generated thumbnail variants</returns>
        Task<List<ImageResult>> GenerateThumbnailsAsync(Stream input, ThumbnailOptions options, CancellationToken ct = default);

        /// <summary>
        /// Strips EXIF metadata from an image while preserving orientation.
        /// </summary>
        /// <param name="input">Source image stream</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Image with EXIF data removed (orientation preserved)</returns>
        Task<ImageResult> StripMetadataAsync(Stream input, CancellationToken ct = default);
    }
}
