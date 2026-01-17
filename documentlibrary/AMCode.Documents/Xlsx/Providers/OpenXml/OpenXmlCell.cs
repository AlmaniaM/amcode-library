using System;
using DocumentFormat.OpenXml.Spreadsheet;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of the cell interface
    /// Represents a cell in an Excel worksheet
    /// </summary>
    public class OpenXmlCell : ICell
    {
        private readonly Cell _cell;
        private readonly OpenXmlWorksheet _worksheet;

        /// <summary>
        /// Initializes a new instance of the OpenXmlCell class
        /// </summary>
        /// <param name="cell">The underlying OpenXml cell</param>
        /// <param name="worksheet">The worksheet containing this cell</param>
        /// <exception cref="ArgumentNullException">Thrown when cell or worksheet is null</exception>
        public OpenXmlCell(Cell cell, OpenXmlWorksheet worksheet)
        {
            _cell = cell ?? throw new ArgumentNullException(nameof(cell));
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
        }

        /// <summary>
        /// Gets the cell reference (e.g., "A1")
        /// </summary>
        public string Reference => _cell.CellReference?.Value ?? "";

        /// <summary>
        /// Gets or sets the cell value
        /// </summary>
        public object Value
        {
            get
            {
                if (_cell.CellValue == null)
                    return null;

                var value = _cell.CellValue.Text;
                if (string.IsNullOrEmpty(value))
                    return null;

                if (_cell.DataType?.Value != null)
                {
                    if (_cell.DataType.Value == CellValues.Boolean)
                    {
                        return value == "1" || value.ToLower() == "true";
                    }
                    else if (_cell.DataType.Value == CellValues.Number)
                    {
                        if (double.TryParse(value, out var number))
                            return number;
                        return value;
                    }
                    else if (_cell.DataType.Value == CellValues.Date)
                    {
                        if (double.TryParse(value, out var oaDate))
                            return DateTime.FromOADate(oaDate);
                        return value;
                    }
                }
                return value;
            }
            set
            {
                if (value == null)
                {
                    _cell.CellValue = null;
                    _cell.DataType = null;
                    return;
                }

                if (value is string stringValue)
                {
                    _cell.CellValue = new CellValue(stringValue);
                    _cell.DataType = CellValues.String;
                }
                else if (value is int || value is long || value is short || value is byte)
                {
                    _cell.CellValue = new CellValue(value.ToString());
                    _cell.DataType = CellValues.Number;
                }
                else if (value is double || value is float || value is decimal)
                {
                    _cell.CellValue = new CellValue(value.ToString());
                    _cell.DataType = CellValues.Number;
                }
                else if (value is bool boolValue)
                {
                    _cell.CellValue = new CellValue(boolValue ? "1" : "0");
                    _cell.DataType = CellValues.Boolean;
                }
                else if (value is DateTime dateTime)
                {
                    _cell.CellValue = new CellValue(dateTime.ToOADate().ToString());
                    _cell.DataType = CellValues.Number;
                }
                else
                {
                    _cell.CellValue = new CellValue(value.ToString());
                    _cell.DataType = CellValues.String;
                }
            }
        }

        /// <summary>
        /// Gets the row index (1-based)
        /// </summary>
        public int Row
        {
            get
            {
                var rowPart = "";
                foreach (char c in Reference)
                {
                    if (char.IsDigit(c))
                        rowPart += c;
                }
                return int.Parse(rowPart);
            }
        }

        /// <summary>
        /// Gets the column index (1-based)
        /// </summary>
        public int Column
        {
            get
            {
                var columnPart = "";
                foreach (char c in Reference)
                {
                    if (char.IsLetter(c))
                        columnPart += c;
                }

                int result = 0;
                foreach (char c in columnPart)
                {
                    result = result * 26 + (c - 'A' + 1);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the column letter (e.g., "A", "B", "AA")
        /// </summary>
        public string ColumnLetter
        {
            get
            {
                var columnPart = "";
                foreach (char c in Reference)
                {
                    if (char.IsLetter(c))
                        columnPart += c;
                }
                return columnPart;
            }
        }

        /// <summary>
        /// Gets or sets the cell formula
        /// </summary>
        public string Formula
        {
            get
            {
                if (_cell.CellFormula != null)
                    return _cell.CellFormula.Text;
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _cell.CellFormula = null;
                }
                else
                {
                    _cell.CellFormula = new CellFormula(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the cell comment
        /// </summary>
        public string Comment
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the comment from the worksheet's comments part
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the comment in the worksheet's comments part
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is locked
        /// </summary>
        public bool IsLocked
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would check the cell's protection properties
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the cell's protection properties
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is hidden
        /// </summary>
        public bool IsHidden
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would check the cell's visibility properties
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the cell's visibility properties
            }
        }

        /// <summary>
        /// Gets or sets the cell number format
        /// </summary>
        public string NumberFormat
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the number format from the cell's style
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the number format in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell font name
        /// </summary>
        public string FontName
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the font name from the cell's style
                return "Calibri";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the font name in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell font size
        /// </summary>
        public double FontSize
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the font size from the cell's style
                return 11.0;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the font size in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell font is bold
        /// </summary>
        public bool IsBold
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the bold property from the cell's style
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the bold property in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell font is italic
        /// </summary>
        public bool IsItalic
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the italic property from the cell's style
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the italic property in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell font is underlined
        /// </summary>
        public bool IsUnderlined
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the underline property from the cell's style
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the underline property in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell font color
        /// </summary>
        public string FontColor
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the font color from the cell's style
                return "000000";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the font color in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell background color
        /// </summary>
        public string BackgroundColor
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the background color from the cell's style
                return "FFFFFF";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the background color in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell horizontal alignment
        /// </summary>
        public string HorizontalAlignment
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the horizontal alignment from the cell's style
                return "General";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the horizontal alignment in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell vertical alignment
        /// </summary>
        public string VerticalAlignment
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the vertical alignment from the cell's style
                return "Bottom";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the vertical alignment in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell border style
        /// </summary>
        public string BorderStyle
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the border style from the cell's style
                return "None";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the border style in the cell's style
            }
        }

        /// <summary>
        /// Gets or sets the cell border color
        /// </summary>
        public string BorderColor
        {
            get
            {
                // This is a simplified implementation
                // In a real implementation, you would get the border color from the cell's style
                return "000000";
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the border color in the cell's style
            }
        }

        /// <summary>
        /// Clears the cell content
        /// </summary>
        public void Clear()
        {
            _cell.CellValue = null;
            _cell.DataType = null;
            _cell.CellFormula = null;
        }

        /// <summary>
        /// Clears the cell formatting
        /// </summary>
        public void ClearFormatting()
        {
            // This is a simplified implementation
            // In a real implementation, you would clear all formatting properties
        }

        /// <summary>
        /// Clears the cell comment
        /// </summary>
        public void ClearComment()
        {
            // This is a simplified implementation
            // In a real implementation, you would clear the comment from the worksheet's comments part
        }

        /// <summary>
        /// Copies the cell to another cell
        /// </summary>
        /// <param name="targetCell">The target cell reference</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(string targetCell)
        {
            return CopyTo(targetCell, true);
        }

        /// <summary>
        /// Copies the cell to another cell with formatting
        /// </summary>
        /// <param name="targetCell">The target cell reference</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(string targetCell, bool includeFormatting)
        {
            try
            {
                var targetRawCell = _worksheet.GetCell(targetCell);
                if (targetRawCell == null)
                    return false;

                var target = new OpenXmlCell(targetRawCell, _worksheet);

                target.Value = Value;
                target.Formula = Formula;

                if (includeFormatting)
                {
                    target.FontName = FontName;
                    target.FontSize = FontSize;
                    target.IsBold = IsBold;
                    target.IsItalic = IsItalic;
                    target.IsUnderlined = IsUnderlined;
                    target.FontColor = FontColor;
                    target.BackgroundColor = BackgroundColor;
                    target.HorizontalAlignment = HorizontalAlignment;
                    target.VerticalAlignment = VerticalAlignment;
                    target.BorderStyle = BorderStyle;
                    target.BorderColor = BorderColor;
                    target.NumberFormat = NumberFormat;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
