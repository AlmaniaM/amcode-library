namespace AMCode.Documents.Common.Models
{
    /// <summary>
    /// Represents page size information
    /// </summary>
    public class PageSize
    {
        /// <summary>
        /// Page width in points (1 point = 1/72 inch)
        /// </summary>
        public double Width { get; set; } = 595.3; // A4 width

        /// <summary>
        /// Page height in points (1 point = 1/72 inch)
        /// </summary>
        public double Height { get; set; } = 841.9; // A4 height

        /// <summary>
        /// Page width in points - convenience property for OpenXml conversions
        /// </summary>
        public double WidthInPoints => Width;

        /// <summary>
        /// Page height in points - convenience property for OpenXml conversions
        /// </summary>
        public double HeightInPoints => Height;

        /// <summary>
        /// Page size name
        /// </summary>
        public string Name { get; set; } = "A4";

        /// <summary>
        /// Create A4 page size
        /// </summary>
        public static PageSize A4 => new PageSize { Width = 595.3, Height = 841.9, Name = "A4" };

        /// <summary>
        /// Create Letter page size
        /// </summary>
        public static PageSize Letter => new PageSize { Width = 612, Height = 792, Name = "Letter" };

        /// <summary>
        /// Create Legal page size
        /// </summary>
        public static PageSize Legal => new PageSize { Width = 612, Height = 1008, Name = "Legal" };
    }
}
