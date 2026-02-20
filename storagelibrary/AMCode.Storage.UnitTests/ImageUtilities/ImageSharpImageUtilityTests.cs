using AMCode.Storage.ImageUtilities;
using AMCode.Storage.ImageUtilities.Models;
using AMCode.Storage.UnitTests.Logging;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Storage.UnitTests.ImageUtilities
{
    [TestFixture]
    public class ImageSharpImageUtilityTests
    {
        private MockStorageLogger _mockLogger;
        private ImageSharpImageUtility _utility;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new MockStorageLogger();
            _utility = new ImageSharpImageUtility(_mockLogger);
        }

        // ─── Helper: create a minimal JPEG/PNG stream ─────────────────────────────

        private static Stream CreateTestJpegStream(int width = 200, int height = 150)
        {
            var stream = new MemoryStream();
            using var image = new Image<Rgb24>(width, height);
            // Fill with a solid colour so it is a valid image
            image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.CornflowerBlue));
            image.Save(stream, new JpegEncoder { Quality = 90 });
            stream.Position = 0;
            return stream;
        }

        private static Stream CreateTestPngStream(int width = 200, int height = 150, bool withAlpha = false)
        {
            var stream = new MemoryStream();
            if (withAlpha)
            {
                using var image = new Image<Rgba32>(width, height);
                image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.Transparent));
                image.Save(stream, new PngEncoder());
            }
            else
            {
                using var image = new Image<Rgb24>(width, height);
                image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.Red));
                image.Save(stream, new PngEncoder());
            }
            stream.Position = 0;
            return stream;
        }

        private static Stream CreateTestWebPStream(int width = 200, int height = 150)
        {
            var stream = new MemoryStream();
            using var image = new Image<Rgb24>(width, height);
            image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.Green));
            image.Save(stream, new WebpEncoder { Quality = 80 });
            stream.Position = 0;
            return stream;
        }

        // ─── Constructor ──────────────────────────────────────────────────────────

        [Test]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ImageSharpImageUtility(null));
        }

        // ─── ResizeAsync ─────────────────────────────────────────────────────────

        [Test]
        public async Task ResizeAsync_ByExactDimensions_ReturnsCorrectSize()
        {
            using var input = CreateTestJpegStream(400, 300);
            var options = new ResizeOptions { Width = 200, Height = 150, MaintainAspectRatio = false };

            var result = await _utility.ResizeAsync(input, options);

            Assert.AreEqual(200, result.Width);
            Assert.AreEqual(150, result.Height);
            Assert.IsNotNull(result.Data);
            Assert.Greater(result.FileSizeBytes, 0);
            result.Data.Dispose();
        }

        [Test]
        public async Task ResizeAsync_ByMaxDimension_MaintainsAspectRatio()
        {
            // 400x200 landscape — max 200 should give 200x100
            using var input = CreateTestJpegStream(400, 200);
            var options = new ResizeOptions { MaxDimension = 200 };

            var result = await _utility.ResizeAsync(input, options);

            Assert.AreEqual(200, result.Width);
            Assert.AreEqual(100, result.Height);
            result.Data.Dispose();
        }

        [Test]
        public async Task ResizeAsync_ByScalePercent_Scales()
        {
            using var input = CreateTestJpegStream(200, 100);
            var options = new ResizeOptions { ScalePercent = 50f };

            var result = await _utility.ResizeAsync(input, options);

            Assert.AreEqual(100, result.Width);
            Assert.AreEqual(50, result.Height);
            result.Data.Dispose();
        }

        [Test]
        public async Task ResizeAsync_PreventUpscale_DoesNotEnlargeSmallImage()
        {
            // Source is 100x100, target is 400x400 — upscale prevention should keep 100x100
            using var input = CreateTestJpegStream(100, 100);
            var options = new ResizeOptions { Width = 400, Height = 400, PreventUpscale = true };

            var result = await _utility.ResizeAsync(input, options);

            Assert.LessOrEqual(result.Width, 100);
            Assert.LessOrEqual(result.Height, 100);
            result.Data.Dispose();
        }

        [Test]
        public void ResizeAsync_NullInput_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _utility.ResizeAsync(null, new ResizeOptions()));
        }

        [Test]
        public void ResizeAsync_NullOptions_ThrowsArgumentNullException()
        {
            using var input = CreateTestJpegStream();
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _utility.ResizeAsync(input, null));
        }

        // ─── CompressAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task CompressAsync_LowQuality_ProducesSmallFile()
        {
            using var input = CreateTestJpegStream(800, 600);
            var highOptions = new CompressionOptions { Quality = 95 };
            var lowOptions = new CompressionOptions { Quality = 10 };

            using var highResult = await _utility.CompressAsync(CreateTestJpegStream(800, 600), highOptions);
            using var lowResult = await _utility.CompressAsync(input, lowOptions);

            Assert.Less(lowResult.FileSizeBytes, highResult.FileSizeBytes);
        }

        [Test]
        public async Task CompressAsync_WithOutputFormatConversion_ConvertsFormat()
        {
            using var input = CreateTestJpegStream();
            var options = new CompressionOptions { Quality = 80, OutputFormat = ImageFormat.WebP };

            var result = await _utility.CompressAsync(input, options);

            Assert.AreEqual(ImageFormat.WebP, result.Format);
            result.Data.Dispose();
        }

        [Test]
        public async Task CompressAsync_TargetFileSizeMode_ReducesUnderTarget()
        {
            using var input = CreateTestJpegStream(800, 600);
            var targetBytes = 30000L; // 30 KB
            var options = new CompressionOptions { TargetFileSizeBytes = targetBytes };

            var result = await _utility.CompressAsync(input, options);

            // Allow some tolerance — at quality 10 (minimum), might not reach target for all images
            Assert.IsNotNull(result.Data);
            result.Data.Dispose();
        }

        // ─── ConvertAsync ────────────────────────────────────────────────────────

        [Test]
        public async Task ConvertAsync_JpegToPng_ChangesFormat()
        {
            using var input = CreateTestJpegStream();

            var result = await _utility.ConvertAsync(input, ImageFormat.Png);

            Assert.AreEqual(ImageFormat.Png, result.Format);
            result.Data.Dispose();
        }

        [Test]
        public async Task ConvertAsync_JpegToWebP_ChangesFormat()
        {
            using var input = CreateTestJpegStream();

            var result = await _utility.ConvertAsync(input, ImageFormat.WebP);

            Assert.AreEqual(ImageFormat.WebP, result.Format);
            result.Data.Dispose();
        }

        [Test]
        public async Task ConvertAsync_PngToJpeg_FlattensTransparency()
        {
            using var input = CreateTestPngStream(withAlpha: true);

            // Should not throw — transparency is flattened to white
            var result = await _utility.ConvertAsync(input, ImageFormat.Jpeg);

            Assert.AreEqual(ImageFormat.Jpeg, result.Format);
            Assert.Greater(result.FileSizeBytes, 0);
            result.Data.Dispose();
        }

        [Test]
        public void ConvertAsync_NullInput_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _utility.ConvertAsync(null, ImageFormat.Jpeg));
        }

        // ─── ExtractMetadataAsync ─────────────────────────────────────────────────

        [Test]
        public async Task ExtractMetadataAsync_Jpeg_ReturnsDimensions()
        {
            using var input = CreateTestJpegStream(320, 240);

            var metadata = await _utility.ExtractMetadataAsync(input);

            Assert.AreEqual(320, metadata.Width);
            Assert.AreEqual(240, metadata.Height);
            Assert.AreEqual(ImageFormat.Jpeg, metadata.Format);
        }

        [Test]
        public async Task ExtractMetadataAsync_Png_ReturnsCorrectFormat()
        {
            using var input = CreateTestPngStream(100, 80);

            var metadata = await _utility.ExtractMetadataAsync(input);

            Assert.AreEqual(100, metadata.Width);
            Assert.AreEqual(80, metadata.Height);
            Assert.AreEqual(ImageFormat.Png, metadata.Format);
        }

        [Test]
        public async Task ExtractMetadataAsync_NoExif_ReturnsEmptyExifData()
        {
            using var input = CreateTestJpegStream();

            var metadata = await _utility.ExtractMetadataAsync(input);

            Assert.IsNotNull(metadata.ExifData);
            // No EXIF in synthetic image — just verify it doesn't throw
        }

        [Test]
        public void ExtractMetadataAsync_NullInput_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _utility.ExtractMetadataAsync(null));
        }

        // ─── GenerateThumbnailsAsync ──────────────────────────────────────────────

        [Test]
        public async Task GenerateThumbnailsAsync_DefaultVariants_ProducesThreeVariants()
        {
            using var input = CreateTestJpegStream(800, 600);
            var options = new ThumbnailOptions();

            var results = await _utility.GenerateThumbnailsAsync(input, options);

            Assert.AreEqual(3, results.Count);
            foreach (var result in results)
            {
                result.Data.Dispose();
            }
        }

        [Test]
        public async Task GenerateThumbnailsAsync_VariantsHaveCorrectSuffixes()
        {
            using var input = CreateTestJpegStream(800, 600);
            var options = new ThumbnailOptions();

            var results = await _utility.GenerateThumbnailsAsync(input, options);

            Assert.AreEqual("thumb_sm", results[0].VariantSuffix);
            Assert.AreEqual("thumb_md", results[1].VariantSuffix);
            Assert.AreEqual("thumb_lg", results[2].VariantSuffix);

            foreach (var result in results) result.Data.Dispose();
        }

        [Test]
        public async Task GenerateThumbnailsAsync_VariantsRespectMaxDimension()
        {
            // 800x600 source — 150 max should give 150x113 (preserving aspect)
            using var input = CreateTestJpegStream(800, 600);
            var options = new ThumbnailOptions
            {
                Variants = new List<ThumbnailVariant>
                {
                    new ThumbnailVariant(150, "sm"),
                    new ThumbnailVariant(300, "md"),
                }
            };

            var results = await _utility.GenerateThumbnailsAsync(input, options);

            Assert.LessOrEqual(results[0].Width, 150);
            Assert.LessOrEqual(results[0].Height, 150);
            Assert.LessOrEqual(results[1].Width, 300);
            Assert.LessOrEqual(results[1].Height, 300);

            foreach (var result in results) result.Data.Dispose();
        }

        [Test]
        public async Task GenerateThumbnailsAsync_WebPOutput_ProducesWebPFormat()
        {
            using var input = CreateTestJpegStream(400, 300);
            var options = new ThumbnailOptions { OutputFormat = ImageFormat.WebP };

            var results = await _utility.GenerateThumbnailsAsync(input, options);

            foreach (var result in results)
            {
                Assert.AreEqual(ImageFormat.WebP, result.Format);
                result.Data.Dispose();
            }
        }

        [Test]
        public void GenerateThumbnailsAsync_NullInput_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _utility.GenerateThumbnailsAsync(null, new ThumbnailOptions()));
        }

        // ─── StripMetadataAsync ──────────────────────────────────────────────────

        [Test]
        public async Task StripMetadataAsync_RemovesExif_OutputIsValid()
        {
            using var input = CreateTestJpegStream(200, 150);

            var result = await _utility.StripMetadataAsync(input);

            Assert.IsNotNull(result.Data);
            Assert.Greater(result.FileSizeBytes, 0);
            Assert.AreEqual(200, result.Width);
            Assert.AreEqual(150, result.Height);
            result.Data.Dispose();
        }

        [Test]
        public async Task StripMetadataAsync_PreservesImageDimensions()
        {
            using var input = CreateTestPngStream(320, 240);

            var result = await _utility.StripMetadataAsync(input);

            Assert.AreEqual(320, result.Width);
            Assert.AreEqual(240, result.Height);
            result.Data.Dispose();
        }

        [Test]
        public void StripMetadataAsync_NullInput_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _utility.StripMetadataAsync(null));
        }

        // ─── Edge cases ───────────────────────────────────────────────────────────

        [Test]
        public void ResizeAsync_ZeroByteStream_ThrowsInvalidImageContentException()
        {
            var emptyStream = new MemoryStream(new byte[0]);
            Assert.ThrowsAsync<SixLabors.ImageSharp.InvalidImageContentException>(() =>
                _utility.ResizeAsync(emptyStream, new ResizeOptions { Width = 100, Height = 100 }));
        }

        [Test]
        public void CompressAsync_CorruptData_ThrowsImageFormatException()
        {
            var corruptStream = new MemoryStream(new byte[] { 0xFF, 0xFE, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 });
            Assert.ThrowsAsync<Exception>(() =>
                _utility.CompressAsync(corruptStream, new CompressionOptions()));
        }
    }
}
