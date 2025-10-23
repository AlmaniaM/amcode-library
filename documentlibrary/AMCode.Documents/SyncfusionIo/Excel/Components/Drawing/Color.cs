using LibDraw = Syncfusion.Drawing;
using AMCode.SyncfusionIo.Xlsx.Common;
namespace AMCode.SyncfusionIo.Xlsx.Drawing
{
    /// <summary>
    /// A color class for creating and manipulating colors.
    /// </summary>
    public class Color
    {
        /// <summary>
        /// No color;
        /// </summary>
        public static readonly Color Empty;

        /// <summary>
        /// The default cell border color.
        /// </summary>
        public static Color DefaultCellBorder { get; } = Color.FromArgb(20, 208, 206, 206);

        /// <summary>
        /// MediumSeaGreen color.
        /// </summary>
        public static Color MediumSeaGreen { get; } = new Color(LibDraw.Color.MediumSeaGreen);

        /// <summary>
        /// MediumSlateBlue color.
        /// </summary>
        public static Color MediumSlateBlue { get; } = new Color(LibDraw.Color.MediumSlateBlue);

        /// <summary>
        /// MediumSpringGreen color.
        /// </summary>
        public static Color MediumSpringGreen { get; } = new Color(LibDraw.Color.MediumSpringGreen);

        /// <summary>
        /// MediumTurquoise color.
        /// </summary>
        public static Color MediumTurquoise { get; } = new Color(LibDraw.Color.MediumTurquoise);

        /// <summary>
        /// MediumVioletRed color.
        /// </summary>
        public static Color MediumVioletRed { get; } = new Color(LibDraw.Color.MediumVioletRed);

        /// <summary>
        /// MidnightBlue color.
        /// </summary>
        public static Color MidnightBlue { get; } = new Color(LibDraw.Color.MidnightBlue);

        /// <summary>
        /// MintCream color.
        /// </summary>
        public static Color MintCream { get; } = new Color(LibDraw.Color.MintCream);

        /// <summary>
        /// MistyRose color.
        /// </summary>
        public static Color MistyRose { get; } = new Color(LibDraw.Color.MistyRose);

        /// <summary>
        /// Moccasin color.
        /// </summary>
        public static Color Moccasin { get; } = new Color(LibDraw.Color.Moccasin);

        /// <summary>
        /// NavajoWhite color.
        /// </summary>
        public static Color NavajoWhite { get; } = new Color(LibDraw.Color.NavajoWhite);

        /// <summary>
        /// Navy color.
        /// </summary>
        public static Color Navy { get; } = new Color(LibDraw.Color.Navy);

        /// <summary>
        /// OldLace color.
        /// </summary>
        public static Color OldLace { get; } = new Color(LibDraw.Color.OldLace);

        /// <summary>
        /// Olive color.
        /// </summary>
        public static Color Olive { get; } = new Color(LibDraw.Color.Olive);

        /// <summary>
        /// OliveDrab color.
        /// </summary>
        public static Color OliveDrab { get; } = new Color(LibDraw.Color.OliveDrab);

        /// <summary>
        /// Orange color.
        /// </summary>
        public static Color Orange { get; } = new Color(LibDraw.Color.Orange);

        /// <summary>
        /// MediumPurple color.
        /// </summary>
        public static Color MediumPurple { get; } = new Color(LibDraw.Color.MediumPurple);

        /// <summary>
        /// MediumOrchid color.
        /// </summary>
        public static Color MediumOrchid { get; } = new Color(LibDraw.Color.MediumOrchid);

        /// <summary>
        /// MediumBlue color.
        /// </summary>
        public static Color MediumBlue { get; } = new Color(LibDraw.Color.MediumBlue);

        /// <summary>
        /// MediumAquamarine color.
        /// </summary>
        public static Color MediumAquamarine { get; } = new Color(LibDraw.Color.MediumAquamarine);

        /// <summary>
        /// LightCoral color.
        /// </summary>
        public static Color LightCoral { get; } = new Color(LibDraw.Color.LightCoral);

        /// <summary>
        /// LightGoldenrodYellow color.
        /// </summary>
        public static Color LightGoldenrodYellow { get; } = new Color(LibDraw.Color.LightGoldenrodYellow);

        /// <summary>
        /// LightGreen color.
        /// </summary>
        public static Color LightGreen { get; } = new Color(LibDraw.Color.LightGreen);

        /// <summary>
        /// LightGray color.
        /// </summary>
        public static Color LightGray { get; } = new Color(LibDraw.Color.LightGray);

        /// <summary>
        /// LightPink color.
        /// </summary>
        public static Color LightPink { get; } = new Color(LibDraw.Color.LightPink);

        /// <summary>
        /// LightSalmon color.
        /// </summary>
        public static Color LightSalmon { get; } = new Color(LibDraw.Color.LightSalmon);

        /// <summary>
        /// LightSeaGreen color.
        /// </summary>
        public static Color LightSeaGreen { get; } = new Color(LibDraw.Color.LightSeaGreen);

        /// <summary>
        /// OrangeRed color.
        /// </summary>
        public static Color OrangeRed { get; } = new Color(LibDraw.Color.OrangeRed);

        /// <summary>
        /// LightSkyBlue color.
        /// </summary>
        public static Color LightSkyBlue { get; } = new Color(LibDraw.Color.LightSkyBlue);

        /// <summary>
        /// LightSteelBlue color.
        /// </summary>
        public static Color LightSteelBlue { get; } = new Color(LibDraw.Color.LightSteelBlue);

        /// <summary>
        /// LightYellow color.
        /// </summary>
        public static Color LightYellow { get; } = new Color(LibDraw.Color.LightYellow);

        /// <summary>
        /// Lime color.
        /// </summary>
        public static Color Lime { get; } = new Color(LibDraw.Color.Lime);

        /// <summary>
        /// LimeGreen color.
        /// </summary>
        public static Color LimeGreen { get; } = new Color(LibDraw.Color.LimeGreen);

        /// <summary>
        /// Linen color.
        /// </summary>
        public static Color Linen { get; } = new Color(LibDraw.Color.Linen);

        /// <summary>
        /// Magenta color.
        /// </summary>
        public static Color Magenta { get; } = new Color(LibDraw.Color.Magenta);

        /// <summary>
        /// Maroon color.
        /// </summary>
        public static Color Maroon { get; } = new Color(LibDraw.Color.Maroon);

        /// <summary>
        /// LightSlateGray color.
        /// </summary>
        public static Color LightSlateGray { get; } = new Color(LibDraw.Color.LightSlateGray);

        /// <summary>
        /// Orchid color.
        /// </summary>
        public static Color Orchid { get; } = new Color(LibDraw.Color.Orchid);

        /// <summary>
        /// PaleGoldenrod color.
        /// </summary>
        public static Color PaleGoldenrod { get; } = new Color(LibDraw.Color.PaleGoldenrod);

        /// <summary>
        /// PaleGreen color.
        /// </summary>
        public static Color PaleGreen { get; } = new Color(LibDraw.Color.PaleGreen);

        /// <summary>
        /// SlateBlue color.
        /// </summary>
        public static Color SlateBlue { get; } = new Color(LibDraw.Color.SlateBlue);

        /// <summary>
        /// SlateGray color.
        /// </summary>
        public static Color SlateGray { get; } = new Color(LibDraw.Color.SlateGray);

        /// <summary>
        /// Snow color.
        /// </summary>
        public static Color Snow { get; } = new Color(LibDraw.Color.Snow);

        /// <summary>
        /// SpringGreen color.
        /// </summary>
        public static Color SpringGreen { get; } = new Color(LibDraw.Color.SpringGreen);

        /// <summary>
        /// SteelBlue color.
        /// </summary>
        public static Color SteelBlue { get; } = new Color(LibDraw.Color.SteelBlue);

        /// <summary>
        /// Tan color.
        /// </summary>
        public static Color Tan { get; } = new Color(LibDraw.Color.Tan);

        /// <summary>
        /// Teal color.
        /// </summary>
        public static Color Teal { get; } = new Color(LibDraw.Color.Teal);

        /// <summary>
        /// SkyBlue color.
        /// </summary>
        public static Color SkyBlue { get; } = new Color(LibDraw.Color.SkyBlue);

        /// <summary>
        /// Thistle color.
        /// </summary>
        public static Color Thistle { get; } = new Color(LibDraw.Color.Thistle);

        /// <summary>
        /// Turquoise color.
        /// </summary>
        public static Color Turquoise { get; } = new Color(LibDraw.Color.Turquoise);

        /// <summary>
        /// Violet color.
        /// </summary>
        public static Color Violet { get; } = new Color(LibDraw.Color.Violet);

        /// <summary>
        /// Wheat color.
        /// </summary>
        public static Color Wheat { get; } = new Color(LibDraw.Color.Wheat);

        /// <summary>
        /// White color.
        /// </summary>
        public static Color White { get; } = new Color(LibDraw.Color.White);

        /// <summary>
        /// WhiteSmoke color.
        /// </summary>
        public static Color WhiteSmoke { get; } = new Color(LibDraw.Color.WhiteSmoke);

        /// <summary>
        /// Yellow color.
        /// </summary>
        public static Color Yellow { get; } = new Color(LibDraw.Color.Yellow);

        /// <summary>
        /// YellowGreen color.
        /// </summary>
        public static Color YellowGreen { get; } = new Color(LibDraw.Color.YellowGreen);

        /// <summary>
        /// Tomato color.
        /// </summary>
        public static Color Tomato { get; } = new Color(LibDraw.Color.Tomato);

        /// <summary>
        /// LightBlue color.
        /// </summary>
        public static Color LightBlue { get; } = new Color(LibDraw.Color.LightBlue);

        /// <summary>
        /// Silver color.
        /// </summary>
        public static Color Silver { get; } = new Color(LibDraw.Color.Silver);

        /// <summary>
        /// SeaShell color.
        /// </summary>
        public static Color SeaShell { get; } = new Color(LibDraw.Color.SeaShell);

        /// <summary>
        /// PaleTurquoise color.
        /// </summary>
        public static Color PaleTurquoise { get; } = new Color(LibDraw.Color.PaleTurquoise);

        /// <summary>
        /// PaleVioletRed color.
        /// </summary>
        public static Color PaleVioletRed { get; } = new Color(LibDraw.Color.PaleVioletRed);

        /// <summary>
        /// PapayaWhip color.
        /// </summary>
        public static Color PapayaWhip { get; } = new Color(LibDraw.Color.PapayaWhip);

        /// <summary>
        /// PeachPuff color.
        /// </summary>
        public static Color PeachPuff { get; } = new Color(LibDraw.Color.PeachPuff);

        /// <summary>
        /// Peru color.
        /// </summary>
        public static Color Peru { get; } = new Color(LibDraw.Color.Peru);

        /// <summary>
        /// Pink color.
        /// </summary>
        public static Color Pink { get; } = new Color(LibDraw.Color.Pink);

        /// <summary>
        /// Plum color.
        /// </summary>
        public static Color Plum { get; } = new Color(LibDraw.Color.Plum);

        /// <summary>
        /// Sienna color.
        /// </summary>
        public static Color Sienna { get; } = new Color(LibDraw.Color.Sienna);

        /// <summary>
        /// PowderBlue color.
        /// </summary>
        public static Color PowderBlue { get; } = new Color(LibDraw.Color.PowderBlue);

        /// <summary>
        /// Red color.
        /// </summary>
        public static Color Red { get; } = new Color(LibDraw.Color.Red);

        /// <summary>
        /// RosyBrown color.
        /// </summary>
        public static Color RosyBrown { get; } = new Color(LibDraw.Color.RosyBrown);

        /// <summary>
        /// RoyalBlue color.
        /// </summary>
        public static Color RoyalBlue { get; } = new Color(LibDraw.Color.RoyalBlue);

        /// <summary>
        /// SaddleBrown color.
        /// </summary>
        public static Color SaddleBrown { get; } = new Color(LibDraw.Color.SaddleBrown);

        /// <summary>
        /// Salmon color.
        /// </summary>
        public static Color Salmon { get; } = new Color(LibDraw.Color.Salmon);

        /// <summary>
        /// SandyBrown color.
        /// </summary>
        public static Color SandyBrown { get; } = new Color(LibDraw.Color.SandyBrown);

        /// <summary>
        /// SeaGreen color.
        /// </summary>
        public static Color SeaGreen { get; } = new Color(LibDraw.Color.SeaGreen);

        /// <summary>
        /// Purple color.
        /// </summary>
        public static Color Purple { get; } = new Color(LibDraw.Color.Purple);

        /// <summary>
        /// LemonChiffon color.
        /// </summary>
        public static Color LemonChiffon { get; } = new Color(LibDraw.Color.LemonChiffon);

        /// <summary>
        /// LightCyan color.
        /// </summary>
        public static Color LightCyan { get; } = new Color(LibDraw.Color.LightCyan);

        /// <summary>
        /// LavenderBlush color.
        /// </summary>
        public static Color LavenderBlush { get; } = new Color(LibDraw.Color.LavenderBlush);

        /// <summary>
        /// DarkMagenta color.
        /// </summary>
        public static Color DarkMagenta { get; } = new Color(LibDraw.Color.DarkMagenta);

        /// <summary>
        /// DarkKhaki color.
        /// </summary>
        public static Color DarkKhaki { get; } = new Color(LibDraw.Color.DarkKhaki);

        /// <summary>
        /// DarkGreen color.
        /// </summary>
        public static Color DarkGreen { get; } = new Color(LibDraw.Color.DarkGreen);

        /// <summary>
        /// DarkGray color.
        /// </summary>
        public static Color DarkGray { get; } = new Color(LibDraw.Color.DarkGray);

        /// <summary>
        /// DarkGoldenrod color.
        /// </summary>
        public static Color DarkGoldenrod { get; } = new Color(LibDraw.Color.DarkGoldenrod);

        /// <summary>
        /// DarkCyan color.
        /// </summary>
        public static Color DarkCyan { get; } = new Color(LibDraw.Color.DarkCyan);

        /// <summary>
        /// DarkBlue color.
        /// </summary>
        public static Color DarkBlue { get; } = new Color(LibDraw.Color.DarkBlue);

        /// <summary>
        /// Cyan color.
        /// </summary>
        public static Color Cyan { get; } = new Color(LibDraw.Color.Cyan);

        /// <summary>
        /// Crimson color.
        /// </summary>
        public static Color Crimson { get; } = new Color(LibDraw.Color.Crimson);

        /// <summary>
        /// LawnGreen color.
        /// </summary>
        public static Color LawnGreen { get; } = new Color(LibDraw.Color.LawnGreen);

        /// <summary>
        /// CornflowerBlue color.
        /// </summary>
        public static Color CornflowerBlue { get; } = new Color(LibDraw.Color.CornflowerBlue);

        /// <summary>
        /// Coral color.
        /// </summary>
        public static Color Coral { get; } = new Color(LibDraw.Color.Coral);

        /// <summary>
        /// Chocolate color.
        /// </summary>
        public static Color Chocolate { get; } = new Color(LibDraw.Color.Chocolate);

        /// <summary>
        /// Chartreuse color.
        /// </summary>
        public static Color Chartreuse { get; } = new Color(LibDraw.Color.Chartreuse);

        /// <summary>
        /// CadetBlue color.
        /// </summary>
        public static Color CadetBlue { get; } = new Color(LibDraw.Color.CadetBlue);

        /// <summary>
        /// BurlyWood color.
        /// </summary>
        public static Color BurlyWood { get; } = new Color(LibDraw.Color.BurlyWood);

        /// <summary>
        /// Brown color.
        /// </summary>
        public static Color Brown { get; } = new Color(LibDraw.Color.Brown);

        /// <summary>
        /// BlueViolet color.
        /// </summary>
        public static Color BlueViolet { get; } = new Color(LibDraw.Color.BlueViolet);

        /// <summary>
        /// Blue color.
        /// </summary>
        public static Color Blue { get; } = new Color(LibDraw.Color.Blue);

        /// <summary>
        /// BlanchedAlmond color.
        /// </summary>
        public static Color BlanchedAlmond { get; } = new Color(LibDraw.Color.BlanchedAlmond);

        /// <summary>
        /// Black color.
        /// </summary>
        public static Color Black { get; } = new Color(LibDraw.Color.Black);

        /// <summary>
        /// Bisque color.
        /// </summary>
        public static Color Bisque { get; } = new Color(LibDraw.Color.Bisque);

        /// <summary>
        /// Beige color.
        /// </summary>
        public static Color Beige { get; } = new Color(LibDraw.Color.Beige);

        /// <summary>
        /// Azure color.
        /// </summary>
        public static Color Azure { get; } = new Color(LibDraw.Color.Azure);

        /// <summary>
        /// Aquamarine color.
        /// </summary>
        public static Color Aquamarine { get; } = new Color(LibDraw.Color.Aquamarine);

        /// <summary>
        /// Aqua color.
        /// </summary>
        public static Color Aqua { get; } = new Color(LibDraw.Color.Aqua);

        /// <summary>
        /// AntiqueWhite color.
        /// </summary>
        public static Color AntiqueWhite { get; } = new Color(LibDraw.Color.AntiqueWhite);

        /// <summary>
        /// AliceBlue color.
        /// </summary>
        public static Color AliceBlue { get; } = new Color(LibDraw.Color.AliceBlue);

        /// <summary>
        /// Transparent color.
        /// </summary>
        public static Color Transparent { get; } = new Color(LibDraw.Color.Transparent);

        /// <summary>
        /// DarkOliveGreen color.
        /// </summary>
        public static Color DarkOliveGreen { get; } = new Color(LibDraw.Color.DarkOliveGreen);

        /// <summary>
        /// DarkOrange color.
        /// </summary>
        public static Color DarkOrange { get; } = new Color(LibDraw.Color.DarkOrange);

        /// <summary>
        /// Cornsilk color.
        /// </summary>
        public static Color Cornsilk { get; } = new Color(LibDraw.Color.Cornsilk);

        /// <summary>
        /// DarkRed color.
        /// </summary>
        public static Color DarkRed { get; } = new Color(LibDraw.Color.DarkRed);

        /// <summary>
        /// Lavender color.
        /// </summary>
        public static Color Lavender { get; } = new Color(LibDraw.Color.Lavender);

        /// <summary>
        /// Khaki color.
        /// </summary>
        public static Color Khaki { get; } = new Color(LibDraw.Color.Khaki);

        /// <summary>
        /// DarkOrchid color.
        /// </summary>
        public static Color DarkOrchid { get; } = new Color(LibDraw.Color.DarkOrchid);

        /// <summary>
        /// Indigo color.
        /// </summary>
        public static Color Indigo { get; } = new Color(LibDraw.Color.Indigo);

        /// <summary>
        /// IndianRed color.
        /// </summary>
        public static Color IndianRed { get; } = new Color(LibDraw.Color.IndianRed);

        /// <summary>
        /// HotPink color.
        /// </summary>
        public static Color HotPink { get; } = new Color(LibDraw.Color.HotPink);

        /// <summary>
        /// Honeydew color.
        /// </summary>
        public static Color Honeydew { get; } = new Color(LibDraw.Color.Honeydew);

        /// <summary>
        /// GreenYellow color.
        /// </summary>
        public static Color GreenYellow { get; } = new Color(LibDraw.Color.GreenYellow);

        /// <summary>
        /// Green color.
        /// </summary>
        public static Color Green { get; } = new Color(LibDraw.Color.Green);

        /// <summary>
        /// Gray color.
        /// </summary>
        public static Color Gray { get; } = new Color(LibDraw.Color.Gray);

        /// <summary>
        /// Goldenrod color.
        /// </summary>
        public static Color Goldenrod { get; } = new Color(LibDraw.Color.Goldenrod);

        /// <summary>
        /// Gold color.
        /// </summary>
        public static Color Gold { get; } = new Color(LibDraw.Color.Gold);

        /// <summary>
        /// GhostWhite color.
        /// </summary>
        public static Color GhostWhite { get; } = new Color(LibDraw.Color.GhostWhite);

        /// <summary>
        /// Gainsboro color.
        /// </summary>
        public static Color Gainsboro { get; } = new Color(LibDraw.Color.Gainsboro);

        /// <summary>
        /// Ivory color.
        /// </summary>
        public static Color Ivory { get; } = new Color(LibDraw.Color.Ivory);

        /// <summary>
        /// ForestGreen color.
        /// </summary>
        public static Color ForestGreen { get; } = new Color(LibDraw.Color.ForestGreen);

        /// <summary>
        /// Fuchsia color.
        /// </summary>
        public static Color Fuchsia { get; } = new Color(LibDraw.Color.Fuchsia);

        /// <summary>
        /// DarkSalmon color.
        /// </summary>
        public static Color DarkSalmon { get; } = new Color(LibDraw.Color.DarkSalmon);

        /// <summary>
        /// DarkSeaGreen color.
        /// </summary>
        public static Color DarkSeaGreen { get; } = new Color(LibDraw.Color.DarkSeaGreen);

        /// <summary>
        /// DarkSlateGray color.
        /// </summary>
        public static Color DarkSlateGray { get; } = new Color(LibDraw.Color.DarkSlateGray);

        /// <summary>
        /// DarkTurquoise color.
        /// </summary>
        public static Color DarkTurquoise { get; } = new Color(LibDraw.Color.DarkTurquoise);

        /// <summary>
        /// DarkViolet color.
        /// </summary>
        public static Color DarkViolet { get; } = new Color(LibDraw.Color.DarkViolet);

        /// <summary>
        /// DarkSlateBlue color.
        /// </summary>
        public static Color DarkSlateBlue { get; } = new Color(LibDraw.Color.DarkSlateBlue);

        /// <summary>
        /// DeepSkyBlue color.
        /// </summary>
        public static Color DeepSkyBlue { get; } = new Color(LibDraw.Color.DeepSkyBlue);

        /// <summary>
        /// DimGray color.
        /// </summary>
        public static Color DimGray { get; } = new Color(LibDraw.Color.DimGray);

        /// <summary>
        /// DodgerBlue color.
        /// </summary>
        public static Color DodgerBlue { get; } = new Color(LibDraw.Color.DodgerBlue);

        /// <summary>
        /// Firebrick color.
        /// </summary>
        public static Color Firebrick { get; } = new Color(LibDraw.Color.Firebrick);

        /// <summary>
        /// FloralWhite color.
        /// </summary>
        public static Color FloralWhite { get; } = new Color(LibDraw.Color.FloralWhite);

        /// <summary>
        /// DeepPink color.
        /// </summary>
        public static Color DeepPink { get; } = new Color(LibDraw.Color.DeepPink);

        static Color()
        {
            Empty = new Color(LibDraw.Color.Empty);
        }

        /// <summary>
        /// Create an instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="libColor">Provide a <see cref="LibDraw.Color"/> instance.</param>
        public Color(LibDraw.Color libColor)
        {
            InnerLibColor = libColor;
        }

        /// <summary>
        /// Green color channel.
        /// </summary>
        public byte G => InnerLibColor.G;

        /// <summary>
        /// Get the inner color object.
        /// </summary>
        internal LibDraw.Color InnerLibColor { get; }

        /// <summary>
        /// Whether or not the color has a name.
        /// </summary>
        public bool IsNamedColor => InnerLibColor.IsNamedColor;

        /// <summary>
        /// Whether or not the color is empty.
        /// </summary>
        public bool IsEmpty => InnerLibColor.IsEmpty;

        /// <summary>
        /// Gets a value indicating whether the color object is a predefined color. The known colors are defined by the <see cref="ExcelKnownColors"/> enum.
        /// </summary>
        public bool IsKnownColor => InnerLibColor.IsKnownColor;

        /// <summary>
        /// Alpha channel.
        /// </summary>
        public byte A => InnerLibColor.A;

        /// <summary>
        /// Blue color channel.
        /// </summary>
        public byte B => InnerLibColor.B;

        /// <summary>
        /// Red color channel.
        /// </summary>
        public byte R => InnerLibColor.R;

        /// <summary>
        /// Name of the color;
        /// </summary>
        public string Name => InnerLibColor.Name;

        /// <summary>
        /// Check if this <see cref="Color"/> equals another <see cref="Color"/>.
        /// </summary>
        /// <param name="obj">A <see cref="Color"/> object to compare to.</param>
        /// <returns>True if the object equals this color object.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Color color)
            {
                return color == this;
            }

            return false;
        }

        /// <summary>
        /// Create a color from a red, green, blue value.
        /// </summary>
        /// <param name="red">The value to use for red.</param>
        /// <param name="green">The value to use for green.</param>
        /// <param name="blue">The value to use for blue.</param>
        /// <returns>An instance of the <see cref="Color"/> class.</returns>
        public static Color FromArgb(int red, int green, int blue) => new Color(LibDraw.Color.FromArgb(red, green, blue));

        /// <summary>
        /// Create a color from an existing <see cref="Color"/> and an alpha.
        /// </summary>
        /// <param name="alpha">The value to use for the alpha channel.</param>
        /// <param name="baseColor">The existing <see cref="Color"/> to use.</param>
        /// <returns>An instance of the <see cref="Color"/> class.</returns>
        public static Color FromArgb(int alpha, Color baseColor) => new Color(LibDraw.Color.FromArgb(alpha, baseColor.InnerLibColor));

        /// <summary>
        /// Create a color from an alpha, red, green, blue value.
        /// </summary>
        /// <param name="alpha">The value to use for the alpha channel.</param>
        /// <param name="red">The value to use for red.</param>
        /// <param name="green">The value to use for green.</param>
        /// <param name="blue">The value to use for blue.</param>
        /// <returns>An instance of the <see cref="Color"/> class.</returns>
        public static Color FromArgb(int alpha, int red, int green, int blue) => new Color(LibDraw.Color.FromArgb(alpha, red, green, blue));

        /// <summary>
        /// Creates a <see cref="Color"/> object from a 32-bit ARGB value.
        /// </summary>
        /// <param name="argb">a 32-bit ARGB value.</param>
        /// <returns>An instance of the <see cref="Color"/> class.</returns>
        public static Color FromArgb(int argb) => new Color(LibDraw.Color.FromArgb(argb));

        /// <summary>
        /// Create a named <see cref="Color"/> object.
        /// </summary>
        /// <param name="name">The name to assign.</param>
        /// <returns>An instance of the <see cref="Color"/> class.</returns>
        public static Color FromName(string name) => new Color(LibDraw.Color.FromName(name));

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => InnerLibColor.GetHashCode();

        /// <summary>
        /// Convert this <see cref="Color"/> to a 32-bit ARGB value.
        /// </summary>
        /// <returns></returns>
        public int ToArgb() => InnerLibColor.ToArgb();

        /// <summary>
        /// To string the <see cref="Color"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> value.</returns>
        public override string ToString() => $"Color [A={A}, R={R}, G={G}, B={B}]";

        /// <summary>
        /// Check to see if two <see cref="Color"/> objects are equal.
        /// </summary>
        /// <param name="left">Left <see cref="Color"/> to compare.</param>
        /// <param name="right">Right <see cref="Color"/> to compare.</param>
        /// <returns>True if the colors are equal and false if not.</returns>
        public static bool operator ==(Color left, Color right)
        {
            return left.A == right.A
                && left.R == right.R
                && left.G == right.G
                && left.B == right.B;
        }

        /// <summary>
        /// Check to see if two <see cref="Color"/> objects are not equal.
        /// </summary>
        /// <param name="left">Left <see cref="Color"/> to compare.</param>
        /// <param name="right">Right <see cref="Color"/> to compare.</param>
        /// <returns>True if the colors are not equal and false if they are.</returns>
        public static bool operator !=(Color left, Color right)
        {
            return left.A != right.A
                || left.R != right.R
                || left.G != right.G
                || left.B != right.B;
        }
    }
}