using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF page size enumeration
    /// </summary>
    public enum PageSize
    {
        /// <summary>
        /// A4 size (210 x 297 mm)
        /// </summary>
        A4,
        
        /// <summary>
        /// A3 size (297 x 420 mm)
        /// </summary>
        A3,
        
        /// <summary>
        /// A5 size (148 x 210 mm)
        /// </summary>
        A5,
        
        /// <summary>
        /// Letter size (8.5 x 11 inches)
        /// </summary>
        Letter,
        
        /// <summary>
        /// Legal size (8.5 x 14 inches)
        /// </summary>
        Legal,
        
        /// <summary>
        /// Tabloid size (11 x 17 inches)
        /// </summary>
        Tabloid,
        
        /// <summary>
        /// Custom size
        /// </summary>
        Custom
    }
    
    /// <summary>
    /// Page size dimensions
    /// </summary>
    public class PageDimensions
    {
        /// <summary>
        /// Width in points
        /// </summary>
        public double Width { get; set; }
        
        /// <summary>
        /// Height in points
        /// </summary>
        public double Height { get; set; }
        
        /// <summary>
        /// Create page dimensions
        /// </summary>
        public PageDimensions(double width, double height)
        {
            Width = width;
            Height = height;
        }
        
        /// <summary>
        /// Get dimensions for a page size
        /// </summary>
        public static PageDimensions GetDimensions(PageSize pageSize)
        {
            return pageSize switch
            {
                PageSize.A4 => new PageDimensions(595.28, 841.89), // 210 x 297 mm in points
                PageSize.A3 => new PageDimensions(841.89, 1190.55), // 297 x 420 mm in points
                PageSize.A5 => new PageDimensions(419.53, 595.28), // 148 x 210 mm in points
                PageSize.Letter => new PageDimensions(612, 792), // 8.5 x 11 inches in points
                PageSize.Legal => new PageDimensions(612, 1008), // 8.5 x 14 inches in points
                PageSize.Tabloid => new PageDimensions(792, 1224), // 11 x 17 inches in points
                _ => new PageDimensions(595.28, 841.89) // Default to A4
            };
        }
    }
}
