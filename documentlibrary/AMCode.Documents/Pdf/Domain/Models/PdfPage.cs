using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF page domain model
    /// </summary>
    public class PdfPage : IPage
    {
        private readonly List<IPageElement> _elements = new List<IPageElement>();
        private int _pageNumber;

        /// <summary>
        /// Page number (1-based)
        /// </summary>
        public int PageNumber => _pageNumber;

        /// <summary>
        /// Page size
        /// </summary>
        public PageSize Size { get; set; } = PageSize.A4;

        /// <summary>
        /// Page margins
        /// </summary>
        public Margins Margins { get; set; } = Margins.Default;

        /// <summary>
        /// Page orientation
        /// </summary>
        public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;

        /// <summary>
        /// Parent document
        /// </summary>
        public IPdfDocument Document { get; set; }

        /// <summary>
        /// Page elements
        /// </summary>
        public IReadOnlyList<IPageElement> Elements => _elements.AsReadOnly();

        /// <summary>
        /// Create PDF page
        /// </summary>
        public PdfPage(int pageNumber)
        {
            _pageNumber = pageNumber;
        }

        /// <summary>
        /// Add text to the page
        /// </summary>
        public void AddText(string text, double x, double y, FontStyle fontStyle = null)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var element = new TextElement(text, x, y, fontStyle ?? new FontStyle());
            _elements.Add(element);
        }

        /// <summary>
        /// Add image to the page
        /// </summary>
        public void AddImage(byte[] imageData, double x, double y, double width, double height)
        {
            if (imageData == null || imageData.Length == 0)
                return;

            var element = new ImageElement(imageData, x, y, width, height);
            _elements.Add(element);
        }

        /// <summary>
        /// Add rectangle to the page
        /// </summary>
        public void AddRectangle(double x, double y, double width, double height, Color? fillColor = null, Color? strokeColor = null)
        {
            var element = new RectangleElement(x, y, width, height, fillColor ?? Color.Transparent, strokeColor ?? Color.Black);
            _elements.Add(element);
        }

        /// <summary>
        /// Add line to the page
        /// </summary>
        public void AddLine(double x1, double y1, double x2, double y2, Color color, double thickness = 1.0)
        {
            var element = new LineElement(x1, y1, x2, y2, color, thickness);
            _elements.Add(element);
        }

        /// <summary>
        /// Add table to the page
        /// </summary>
        public ITable AddTable(double x, double y, int rows, int columns)
        {
            var table = new PdfTable(x, y, rows, columns);
            _elements.Add(table);
            return table;
        }
    }

    /// <summary>
    /// Base interface for page elements
    /// </summary>
    public interface IPageElement
    {
        /// <summary>
        /// Element type
        /// </summary>
        string ElementType { get; }
    }

    /// <summary>
    /// Text element
    /// </summary>
    public class TextElement : IPageElement
    {
        public string ElementType => "Text";
        public string Text { get; }
        public double X { get; }
        public double Y { get; }
        public FontStyle FontStyle { get; }

        public TextElement(string text, double x, double y, FontStyle fontStyle)
        {
            Text = text;
            X = x;
            Y = y;
            FontStyle = fontStyle;
        }
    }

    /// <summary>
    /// Image element
    /// </summary>
    public class ImageElement : IPageElement
    {
        public string ElementType => "Image";
        public byte[] ImageData { get; }
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public ImageElement(byte[] imageData, double x, double y, double width, double height)
        {
            ImageData = imageData;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Rectangle element
    /// </summary>
    public class RectangleElement : IPageElement
    {
        public string ElementType => "Rectangle";
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }
        public Color FillColor { get; }
        public Color StrokeColor { get; }

        public RectangleElement(double x, double y, double width, double height, Color fillColor, Color strokeColor)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            FillColor = fillColor;
            StrokeColor = strokeColor;
        }
    }

    /// <summary>
    /// Line element
    /// </summary>
    public class LineElement : IPageElement
    {
        public string ElementType => "Line";
        public double X1 { get; }
        public double Y1 { get; }
        public double X2 { get; }
        public double Y2 { get; }
        public Color Color { get; }
        public double Thickness { get; }

        public LineElement(double x1, double y1, double x2, double y2, Color color, double thickness)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Color = color;
            Thickness = thickness;
        }
    }
}
