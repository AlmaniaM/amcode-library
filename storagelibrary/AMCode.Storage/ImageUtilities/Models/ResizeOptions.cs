namespace AMCode.Storage.ImageUtilities.Models
{
    /// <summary>
    /// Controls how the image is fitted to the target dimensions
    /// </summary>
    public enum ResizeMode
    {
        /// <summary>Resize to fit within the target dimensions, maintaining aspect ratio</summary>
        Max,
        /// <summary>Resize and crop to exactly fill the target dimensions</summary>
        Crop,
        /// <summary>Resize to fit within the target dimensions, padding to fill</summary>
        Pad,
        /// <summary>Stretch/distort image to exactly match target dimensions</summary>
        Stretch
    }

    /// <summary>
    /// Options controlling how an image is resized
    /// </summary>
    public class ResizeOptions
    {
        /// <summary>
        /// Target width in pixels. Use with Height for exact dimensions.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Target height in pixels. Use with Width for exact dimensions.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Constrains the longest side to this pixel count while maintaining aspect ratio.
        /// Takes precedence over Width/Height when set.
        /// </summary>
        public int? MaxDimension { get; set; }

        /// <summary>
        /// Scale factor as a percentage (e.g., 50 = 50%). Takes precedence over all other size options when set.
        /// </summary>
        public float? ScalePercent { get; set; }

        /// <summary>
        /// When true (default), preserves the original aspect ratio.
        /// </summary>
        public bool MaintainAspectRatio { get; set; } = true;

        /// <summary>
        /// When true (default), prevents enlarging images smaller than the target size.
        /// </summary>
        public bool PreventUpscale { get; set; } = true;

        /// <summary>
        /// Resize mode controlling how the image is fitted to the target dimensions.
        /// </summary>
        public ResizeMode Mode { get; set; } = ResizeMode.Max;
    }
}
