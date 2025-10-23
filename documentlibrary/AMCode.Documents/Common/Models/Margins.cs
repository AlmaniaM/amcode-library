namespace AMCode.Documents.Common.Models
{
    /// <summary>
    /// Represents page margins
    /// </summary>
    public class Margins
    {
        /// <summary>
        /// Top margin in points
        /// </summary>
        public double Top { get; set; } = 72; // 1 inch

        /// <summary>
        /// Bottom margin in points
        /// </summary>
        public double Bottom { get; set; } = 72; // 1 inch

        /// <summary>
        /// Left margin in points
        /// </summary>
        public double Left { get; set; } = 72; // 1 inch

        /// <summary>
        /// Right margin in points
        /// </summary>
        public double Right { get; set; } = 72; // 1 inch

        /// <summary>
        /// Create default margins (1 inch all around)
        /// </summary>
        public static Margins Default => new Margins();

        /// <summary>
        /// Create normal margins (1 inch all around) - alias for Default
        /// </summary>
        public static Margins Normal => Default;

        /// <summary>
        /// Create narrow margins (0.5 inch all around)
        /// </summary>
        public static Margins Narrow => new Margins { Top = 36, Bottom = 36, Left = 36, Right = 36 };

        /// <summary>
        /// Create wide margins (1.5 inch all around)
        /// </summary>
        public static Margins Wide => new Margins { Top = 108, Bottom = 108, Left = 108, Right = 108 };
    }
}
