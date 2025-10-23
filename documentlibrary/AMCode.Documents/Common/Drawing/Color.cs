using System;

namespace AMCode.Documents.Common.Drawing
{
    /// <summary>
    /// Represents a color with ARGB values
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Alpha component (0-255)
        /// </summary>
        public byte A { get; set; } = 255;

        /// <summary>
        /// Red component (0-255)
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Green component (0-255)
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Blue component (0-255)
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Create a color from ARGB values
        /// </summary>
        public static Color FromArgb(int argb)
        {
            return new Color
            {
                A = (byte)((argb >> 24) & 0xFF),
                R = (byte)((argb >> 16) & 0xFF),
                G = (byte)((argb >> 8) & 0xFF),
                B = (byte)(argb & 0xFF)
            };
        }

        /// <summary>
        /// Create a color from ARGB values
        /// </summary>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color { A = a, R = r, G = g, B = b };
        }

        /// <summary>
        /// Create a color from RGB values
        /// </summary>
        public static Color FromArgb(byte r, byte g, byte b)
        {
            return new Color { A = 255, R = r, G = g, B = b };
        }

        /// <summary>
        /// Convert to ARGB integer
        /// </summary>
        public int ToArgb()
        {
            return (A << 24) | (R << 16) | (G << 8) | B;
        }

        /// <summary>
        /// Black color
        /// </summary>
        public static Color Black => new Color { R = 0, G = 0, B = 0 };

        /// <summary>
        /// White color
        /// </summary>
        public static Color White => new Color { R = 255, G = 255, B = 255 };

        /// <summary>
        /// Red color
        /// </summary>
        public static Color Red => new Color { R = 255, G = 0, B = 0 };

        /// <summary>
        /// Green color
        /// </summary>
        public static Color Green => new Color { R = 0, G = 255, B = 0 };

        /// <summary>
        /// Blue color
        /// </summary>
        public static Color Blue => new Color { R = 0, G = 0, B = 255 };

        /// <summary>
        /// Transparent color
        /// </summary>
        public static Color Transparent => new Color { A = 0, R = 0, G = 0, B = 0 };

        /// <summary>
        /// Light gray color
        /// </summary>
        public static Color LightGray => new Color { R = 211, G = 211, B = 211 };

        /// <summary>
        /// Light blue color
        /// </summary>
        public static Color LightBlue => new Color { R = 173, G = 216, B = 230 };

        /// <summary>
        /// Gray color
        /// </summary>
        public static Color Gray => new Color { R = 128, G = 128, B = 128 };

        /// <summary>
        /// Dark gray color
        /// </summary>
        public static Color DarkGray => new Color { R = 64, G = 64, B = 64 };

        /// <summary>
        /// Light green color
        /// </summary>
        public static Color LightGreen => new Color { R = 144, G = 238, B = 144 };

        /// <summary>
        /// Dark blue color
        /// </summary>
        public static Color DarkBlue => new Color { R = 0, G = 0, B = 139 };

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Color other)
            {
                return A == other.A && R == other.R && G == other.G && B == other.B;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for the current object
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(A, R, G, B);
        }

        /// <summary>
        /// Determines whether two Color instances are equal
        /// </summary>
        public static bool operator ==(Color left, Color right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two Color instances are not equal
        /// </summary>
        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }
    }
}
