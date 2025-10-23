using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents a bounding box with coordinates and dimensions
/// </summary>
public class BoundingBox
{
    /// <summary>
    /// The X coordinate of the top-left corner
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// The Y coordinate of the top-left corner
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// The width of the bounding box
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// The height of the bounding box
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// The X coordinate of the bottom-right corner
    /// </summary>
    [JsonIgnore]
    public double Right => X + Width;

    /// <summary>
    /// The Y coordinate of the bottom-right corner
    /// </summary>
    [JsonIgnore]
    public double Bottom => Y + Height;

    /// <summary>
    /// The center X coordinate
    /// </summary>
    [JsonIgnore]
    public double CenterX => X + (Width / 2);

    /// <summary>
    /// The center Y coordinate
    /// </summary>
    [JsonIgnore]
    public double CenterY => Y + (Height / 2);

    /// <summary>
    /// The area of the bounding box
    /// </summary>
    [JsonIgnore]
    public double Area => Width * Height;

    /// <summary>
    /// Whether this bounding box is valid (has positive dimensions)
    /// </summary>
    [JsonIgnore]
    public bool IsValid => Width > 0 && Height > 0;

    /// <summary>
    /// Creates a bounding box from coordinates
    /// </summary>
    public static BoundingBox FromCoordinates(double x, double y, double width, double height)
    {
        return new BoundingBox
        {
            X = x,
            Y = y,
            Width = width,
            Height = height
        };
    }

    /// <summary>
    /// Creates a bounding box from two points
    /// </summary>
    public static BoundingBox FromPoints(double x1, double y1, double x2, double y2)
    {
        var minX = Math.Min(x1, x2);
        var minY = Math.Min(y1, y2);
        var maxX = Math.Max(x1, x2);
        var maxY = Math.Max(y1, y2);

        return new BoundingBox
        {
            X = minX,
            Y = minY,
            Width = maxX - minX,
            Height = maxY - minY
        };
    }

    /// <summary>
    /// Checks if this bounding box contains a point
    /// </summary>
    public bool Contains(double x, double y)
    {
        return x >= X && x <= Right && y >= Y && y <= Bottom;
    }

    /// <summary>
    /// Checks if this bounding box intersects with another bounding box
    /// </summary>
    public bool Intersects(BoundingBox other)
    {
        return !(Right < other.X || X > other.Right || Bottom < other.Y || Y > other.Bottom);
    }

    /// <summary>
    /// Returns the intersection of this bounding box with another
    /// </summary>
    public BoundingBox? Intersection(BoundingBox other)
    {
        if (!Intersects(other))
            return null;

        var left = Math.Max(X, other.X);
        var top = Math.Max(Y, other.Y);
        var right = Math.Min(Right, other.Right);
        var bottom = Math.Min(Bottom, other.Bottom);

        return FromCoordinates(left, top, right - left, bottom - top);
    }

    /// <summary>
    /// Returns the union of this bounding box with another
    /// </summary>
    public BoundingBox Union(BoundingBox other)
    {
        var left = Math.Min(X, other.X);
        var top = Math.Min(Y, other.Y);
        var right = Math.Max(Right, other.Right);
        var bottom = Math.Max(Bottom, other.Bottom);

        return FromCoordinates(left, top, right - left, bottom - top);
    }

    /// <summary>
    /// Returns a string representation of the bounding box
    /// </summary>
    public override string ToString()
    {
        return $"BoundingBox(X={X:F2}, Y={Y:F2}, Width={Width:F2}, Height={Height:F2})";
    }
}