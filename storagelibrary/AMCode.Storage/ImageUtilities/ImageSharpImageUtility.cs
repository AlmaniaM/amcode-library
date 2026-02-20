using AMCode.Storage.ImageUtilities.Models;
using AMCode.Storage.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageFormatEnum = AMCode.Storage.ImageUtilities.Models.ImageFormat;
using SharpResizeMode = SixLabors.ImageSharp.Processing.ResizeMode;

namespace AMCode.Storage.ImageUtilities
{
    /// <summary>
    /// Implementation of IImageUtility using SixLabors.ImageSharp for all image processing operations.
    /// </summary>
    public class ImageSharpImageUtility : IImageUtility
    {
        private readonly IStorageLogger _logger;

        /// <summary>
        /// Initializes a new instance of ImageSharpImageUtility.
        /// </summary>
        /// <param name="logger">Storage logger for operation tracking</param>
        public ImageSharpImageUtility(IStorageLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<ImageResult> ResizeAsync(Stream input, Models.ResizeOptions options, CancellationToken ct = default)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (options == null) throw new ArgumentNullException(nameof(options));

            _logger.LogInformation("Resizing image");

            using var image = await Image.LoadAsync(input, ct);
            var originalWidth = image.Width;
            var originalHeight = image.Height;

            int targetWidth;
            int targetHeight;

            if (options.ScalePercent.HasValue)
            {
                targetWidth = (int)(originalWidth * options.ScalePercent.Value / 100f);
                targetHeight = (int)(originalHeight * options.ScalePercent.Value / 100f);
            }
            else if (options.MaxDimension.HasValue)
            {
                var maxDim = options.MaxDimension.Value;
                if (originalWidth >= originalHeight)
                {
                    targetWidth = maxDim;
                    targetHeight = (int)((float)originalHeight / originalWidth * maxDim);
                }
                else
                {
                    targetHeight = maxDim;
                    targetWidth = (int)((float)originalWidth / originalHeight * maxDim);
                }
            }
            else
            {
                targetWidth = options.Width ?? originalWidth;
                targetHeight = options.Height ?? originalHeight;
            }

            if (options.PreventUpscale)
            {
                targetWidth = Math.Min(targetWidth, originalWidth);
                targetHeight = Math.Min(targetHeight, originalHeight);
            }

            var resizeMode = MapResizeMode(options.Mode);
            var resizeOptions = new SixLabors.ImageSharp.Processing.ResizeOptions
            {
                Size = new Size(targetWidth, targetHeight),
                Mode = resizeMode
            };

            image.Mutate(x => x.Resize(resizeOptions));

            var outputStream = new MemoryStream();
            var encoder = GetEncoderFromFormat(DetectFormat(image));
            await image.SaveAsync(outputStream, encoder, ct);
            outputStream.Position = 0;

            _logger.LogInformation("Image resized from {0}x{1} to {2}x{3}",
                originalWidth, originalHeight, image.Width, image.Height);

            return new ImageResult
            {
                Data = outputStream,
                Format = DetectFormat(image),
                Width = image.Width,
                Height = image.Height,
                FileSizeBytes = outputStream.Length
            };
        }

        /// <inheritdoc/>
        public async Task<ImageResult> CompressAsync(Stream input, CompressionOptions options, CancellationToken ct = default)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (options == null) throw new ArgumentNullException(nameof(options));

            _logger.LogInformation("Compressing image with quality {0}", options.Quality);

            using var image = await Image.LoadAsync(input, ct);
            var outputFormat = options.OutputFormat ?? DetectFormat(image);

            if (options.TargetFileSizeBytes.HasValue)
            {
                return await CompressToTargetSizeAsync(image, outputFormat, options.TargetFileSizeBytes.Value, ct);
            }

            var quality = Math.Clamp(options.Quality, 1, 100);
            var encoder = GetEncoderWithQuality(outputFormat, quality);

            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder, ct);
            outputStream.Position = 0;

            _logger.LogInformation("Image compressed: {0} bytes", outputStream.Length);

            return new ImageResult
            {
                Data = outputStream,
                Format = outputFormat,
                Width = image.Width,
                Height = image.Height,
                FileSizeBytes = outputStream.Length
            };
        }

        /// <inheritdoc/>
        public async Task<ImageResult> ConvertAsync(Stream input, ImageFormatEnum targetFormat, CancellationToken ct = default)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            _logger.LogInformation("Converting image to {0}", targetFormat);

            using var image = await Image.LoadAsync(input, ct);

            if (targetFormat == ImageFormatEnum.Jpeg)
            {
                // Flatten transparency to white background for JPEG
                var formatName = image.Metadata.DecodedImageFormat?.Name?.ToUpperInvariant();
                if (formatName == "PNG" || formatName == "WEBP")
                {
                    image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White));
                }
            }

            var encoder = GetEncoderWithQuality(targetFormat, 85);
            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder, ct);
            outputStream.Position = 0;

            _logger.LogInformation("Image converted to {0}: {1} bytes", targetFormat, outputStream.Length);

            return new ImageResult
            {
                Data = outputStream,
                Format = targetFormat,
                Width = image.Width,
                Height = image.Height,
                FileSizeBytes = outputStream.Length
            };
        }

        /// <inheritdoc/>
        public async Task<ImageMetadata> ExtractMetadataAsync(Stream input, CancellationToken ct = default)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            _logger.LogInformation("Extracting image metadata");

            var fileSizeBytes = input.CanSeek ? input.Length : 0L;

            using var image = await Image.LoadAsync(input, ct);

            var metadata = new ImageMetadata
            {
                Width = image.Width,
                Height = image.Height,
                Format = DetectFormat(image),
                FileSizeBytes = fileSizeBytes,
                ExifData = new Dictionary<string, string>()
            };

            var exifProfile = image.Metadata.ExifProfile;
            if (exifProfile != null)
            {
                foreach (var exifValue in exifProfile.Values)
                {
                    var key = exifValue.Tag.ToString();
                    var value = exifValue.GetValue()?.ToString() ?? string.Empty;
                    metadata.ExifData[key] = value;

                    if (exifValue.Tag == ExifTag.Orientation && exifValue.GetValue() is ushort orientation)
                    {
                        metadata.Orientation = (int)orientation;
                    }

                    if (exifValue.Tag == ExifTag.DateTimeOriginal && exifValue.GetValue() is string dateStr)
                    {
                        if (DateTime.TryParseExact(dateStr, "yyyy:MM:dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out var dateTaken))
                        {
                            metadata.DateTaken = dateTaken;
                        }
                    }
                }
            }

            _logger.LogInformation("Metadata extracted: {0}x{1}, {2}, {3} EXIF entries",
                metadata.Width, metadata.Height, metadata.Format, metadata.ExifData.Count);

            return metadata;
        }

        /// <inheritdoc/>
        public async Task<List<ImageResult>> GenerateThumbnailsAsync(Stream input, ThumbnailOptions options, CancellationToken ct = default)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (options == null) throw new ArgumentNullException(nameof(options));

            _logger.LogInformation("Generating {0} thumbnail variants", options.Variants.Count);

            using var sourceImage = await Image.LoadAsync(input, ct);

            if (options.StripMetadata)
            {
                sourceImage.Metadata.ExifProfile = null;
                sourceImage.Metadata.IccProfile = null;
                sourceImage.Metadata.IptcProfile = null;
            }

            var results = new List<ImageResult>();
            var quality = Math.Clamp(options.Quality, 1, 100);
            var encoder = GetEncoderWithQuality(options.OutputFormat, quality);

            foreach (var variant in options.Variants)
            {
                ct.ThrowIfCancellationRequested();

                using var thumbnailImage = sourceImage.Clone(x =>
                {
                    x.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions
                    {
                        Size = new Size(variant.MaxDimension, variant.MaxDimension),
                        Mode = SharpResizeMode.Max
                    });
                });

                var outputStream = new MemoryStream();
                await thumbnailImage.SaveAsync(outputStream, encoder, ct);
                outputStream.Position = 0;

                results.Add(new ImageResult
                {
                    Data = outputStream,
                    Format = options.OutputFormat,
                    Width = thumbnailImage.Width,
                    Height = thumbnailImage.Height,
                    FileSizeBytes = outputStream.Length,
                    VariantSuffix = variant.Suffix
                });

                _logger.LogInformation("Generated thumbnail variant {0}: {1}x{2}, {3} bytes",
                    variant.Suffix, thumbnailImage.Width, thumbnailImage.Height, outputStream.Length);
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<ImageResult> StripMetadataAsync(Stream input, CancellationToken ct = default)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            _logger.LogInformation("Stripping EXIF metadata (preserving orientation)");

            using var image = await Image.LoadAsync(input, ct);

            // Preserve orientation before stripping
            ushort? orientation = null;
            var exifProfile = image.Metadata.ExifProfile;
            if (exifProfile != null)
            {
                if (exifProfile.TryGetValue(ExifTag.Orientation, out var orientationExif))
                {
                    orientation = orientationExif.Value;
                }
            }

            // Strip all metadata
            image.Metadata.ExifProfile = null;
            image.Metadata.IccProfile = null;
            image.Metadata.IptcProfile = null;

            // Restore orientation only
            if (orientation.HasValue)
            {
                var newProfile = new ExifProfile();
                newProfile.SetValue(ExifTag.Orientation, orientation.Value);
                image.Metadata.ExifProfile = newProfile;
            }

            var outputFormat = DetectFormat(image);
            var encoder = GetEncoderFromFormat(outputFormat);
            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder, ct);
            outputStream.Position = 0;

            _logger.LogInformation("EXIF stripped, orientation {0} preserved, output: {1} bytes",
                orientation?.ToString() ?? "none", outputStream.Length);

            return new ImageResult
            {
                Data = outputStream,
                Format = outputFormat,
                Width = image.Width,
                Height = image.Height,
                FileSizeBytes = outputStream.Length
            };
        }

        // ─── Private helpers ─────────────────────────────────────────────────────

        private ImageFormatEnum DetectFormat(Image image)
        {
            var formatName = image.Metadata.DecodedImageFormat?.Name?.ToUpperInvariant();
            return formatName switch
            {
                "PNG" => ImageFormatEnum.Png,
                "WEBP" => ImageFormatEnum.WebP,
                _ => ImageFormatEnum.Jpeg
            };
        }

        private IImageEncoder GetEncoderFromFormat(ImageFormatEnum format)
        {
            return format switch
            {
                ImageFormatEnum.Png => new PngEncoder(),
                ImageFormatEnum.WebP => new WebpEncoder(),
                _ => new JpegEncoder()
            };
        }

        private IImageEncoder GetEncoderWithQuality(ImageFormatEnum format, int quality)
        {
            return format switch
            {
                ImageFormatEnum.Png => new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression },
                ImageFormatEnum.WebP => new WebpEncoder { Quality = quality },
                _ => new JpegEncoder { Quality = quality }
            };
        }

        private SharpResizeMode MapResizeMode(Models.ResizeMode mode)
        {
            return mode switch
            {
                Models.ResizeMode.Crop => SharpResizeMode.Crop,
                Models.ResizeMode.Pad => SharpResizeMode.Pad,
                Models.ResizeMode.Stretch => SharpResizeMode.Stretch,
                _ => SharpResizeMode.Max
            };
        }

        private async Task<ImageResult> CompressToTargetSizeAsync(
            Image image,
            ImageFormatEnum format,
            long targetSizeBytes,
            CancellationToken ct)
        {
            var quality = 90;
            MemoryStream outputStream = new MemoryStream();

            do
            {
                outputStream.Dispose();
                outputStream = new MemoryStream();
                var encoder = GetEncoderWithQuality(format, quality);
                await image.SaveAsync(outputStream, encoder, ct);

                if (outputStream.Length <= targetSizeBytes || quality <= 10)
                    break;

                quality -= 10;
            }
            while (true);

            outputStream.Position = 0;
            return new ImageResult
            {
                Data = outputStream,
                Format = format,
                Width = image.Width,
                Height = image.Height,
                FileSizeBytes = outputStream.Length
            };
        }
    }
}
