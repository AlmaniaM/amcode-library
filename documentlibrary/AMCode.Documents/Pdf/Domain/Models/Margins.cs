using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Page margins
    /// </summary>
    public class Margins
    {
        /// <summary>
        /// Top margin in points
        /// </summary>
        public double Top { get; set; }
        
        /// <summary>
        /// Bottom margin in points
        /// </summary>
        public double Bottom { get; set; }
        
        /// <summary>
        /// Left margin in points
        /// </summary>
        public double Left { get; set; }
        
        /// <summary>
        /// Right margin in points
        /// </summary>
        public double Right { get; set; }
        
        /// <summary>
        /// Create margins with all sides equal
        /// </summary>
        public Margins(double all) : this(all, all, all, all)
        {
        }
        
        /// <summary>
        /// Create margins with specific values
        /// </summary>
        public Margins(double top, double bottom, double left, double right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
        
        /// <summary>
        /// Default margins (1 inch = 72 points)
        /// </summary>
        public static Margins Default => new Margins(72);
        
        /// <summary>
        /// No margins
        /// </summary>
        public static Margins None => new Margins(0);
    }
}
