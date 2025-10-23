using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents a block of text with its position and properties
/// </summary>
public class TextBlock
{
    /// <summary>
    /// The text content of this block
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Confidence score for this text block (0.0 to 1.0)
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Bounding box coordinates for this text block
    /// </summary>
    public BoundingBox BoundingBox { get; set; } = new();

    /// <summary>
    /// Whether this text block is handwritten
    /// </summary>
    public bool IsHandwritten { get; set; }

    /// <summary>
    /// Whether this text block is printed
    /// </summary>
    public bool IsPrinted { get; set; }

    /// <summary>
    /// The language of this text block
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Additional properties specific to this text block
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// The font size of this text block (if available)
    /// </summary>
    public double? FontSize { get; set; }

    /// <summary>
    /// The font family of this text block (if available)
    /// </summary>
    public string? FontFamily { get; set; }

    /// <summary>
    /// The reading order of this text block
    /// </summary>
    public int ReadingOrder { get; set; }

    /// <summary>
    /// Whether this text block is part of a table
    /// </summary>
    public bool IsTableContent { get; set; }

    /// <summary>
    /// The table cell information if this block is part of a table
    /// </summary>
    public TableCellInfo? TableCell { get; set; }
}

/// <summary>
/// Represents table cell information for text blocks that are part of tables
/// </summary>
public class TableCellInfo
{
    /// <summary>
    /// The row index of the table cell
    /// </summary>
    public int RowIndex { get; set; }

    /// <summary>
    /// The column index of the table cell
    /// </summary>
    public int ColumnIndex { get; set; }

    /// <summary>
    /// The number of rows this cell spans
    /// </summary>
    public int RowSpan { get; set; } = 1;

    /// <summary>
    /// The number of columns this cell spans
    /// </summary>
    public int ColumnSpan { get; set; } = 1;

    /// <summary>
    /// Whether this cell is a header cell
    /// </summary>
    public bool IsHeader { get; set; }
}