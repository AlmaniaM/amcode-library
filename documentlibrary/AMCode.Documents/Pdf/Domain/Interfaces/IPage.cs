using System;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Represents a PDF page
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// Page number (1-based)
        /// </summary>
        int PageNumber { get; }
        
        /// <summary>
        /// Page size
        /// </summary>
        PageSize Size { get; set; }
        
        /// <summary>
        /// Page margins
        /// </summary>
        Margins Margins { get; set; }
        
        /// <summary>
        /// Page orientation
        /// </summary>
        PageOrientation Orientation { get; set; }
        
        /// <summary>
        /// Parent document
        /// </summary>
        IPdfDocument Document { get; }
        
        /// <summary>
        /// Add text to the page
        /// </summary>
        void AddText(string text, double x, double y, FontStyle fontStyle = null);
        
        /// <summary>
        /// Add image to the page
        /// </summary>
        void AddImage(byte[] imageData, double x, double y, double width, double height);
        
        /// <summary>
        /// Add rectangle to the page
        /// </summary>
        void AddRectangle(double x, double y, double width, double height, Color? fillColor = null, Color? strokeColor = null);
        
        /// <summary>
        /// Add line to the page
        /// </summary>
        void AddLine(double x1, double y1, double x2, double y2, Color color, double thickness = 1.0);
        
        /// <summary>
        /// Add table to the page
        /// </summary>
        ITable AddTable(double x, double y, int rows, int columns);
    }
}
