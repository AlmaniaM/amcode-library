using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Enums;

// Type aliases to disambiguate between Common and OpenXml types
using CommonColor = AMCode.Documents.Common.Drawing.Color;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableCell
    /// </summary>
    public class OpenXmlTableCell : ITableCell
    {
        private readonly OpenXmlTable _table;
        private readonly TableCell _openXmlCell;
        private readonly int _rowIndex;
        private readonly int _columnIndex;

        public string Text
        {
            get => _openXmlCell.InnerText;
            set
            {
                // Clear existing content
                _openXmlCell.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
                
                if (!string.IsNullOrEmpty(value))
                {
                    var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                    var run = new Run(new Text(value));
                    paragraph.AppendChild(run);
                    _openXmlCell.AppendChild(paragraph);
                }
            }
        }

        public IParagraphs Paragraphs { get; }
        public int RowIndex => _rowIndex;
        public int ColumnIndex => _columnIndex;
        public ITableRow Row => _table.Rows[_rowIndex];

        public AMCode.Documents.Common.Drawing.Color BackgroundColor
        {
            get
            {
                var shading = _openXmlCell.TableCellProperties?.Shading;
                if (shading?.Color != null)
                {
                    return CommonColor.FromArgb(Convert.ToInt32(shading.Color, 16));
                }
                return CommonColor.White;
            }
            set
            {
                SetBackgroundColor(value);
            }
        }

        public BorderStyle BorderStyle
        {
            get
            {
                var borders = _openXmlCell.TableCellProperties?.GetFirstChild<TableCellBorders>();
                if (borders != null)
                {
                    return new BorderStyle
                    {
                        Color = borders.TopBorder?.Color != null ? CommonColor.FromArgb(Convert.ToInt32(borders.TopBorder.Color, 16)) : CommonColor.Black,
                        LineStyle = BorderLineStyle.Thin,
                        Sides = BorderSides.All
                    };
                }
                return new BorderStyle();
            }
            set
            {
                SetBorder(value);
            }
        }

        internal TableCell OpenXmlElement => _openXmlCell;

        public OpenXmlTableCell(OpenXmlTable table, TableCell openXmlCell, int rowIndex, int columnIndex)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _openXmlCell = openXmlCell ?? throw new ArgumentNullException(nameof(openXmlCell));
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            var document = table.Tables.Document as OpenXmlDocument;
            Paragraphs = new OpenXmlParagraphs(document);
        }

        public void AddText(string text)
        {
            var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            var run = new Run(new Text(text));
            paragraph.AppendChild(run);
            _openXmlCell.AppendChild(paragraph);
        }

        public void AddText(string text, FontStyle fontStyle)
        {
            var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            var run = new Run();
            
            // Apply font formatting
            var runProperties = new RunProperties();
            
            if (fontStyle.Bold)
                runProperties.AppendChild(new Bold());
            
            if (fontStyle.Italic)
                runProperties.AppendChild(new Italic());
            
            if (fontStyle.Underline)
                runProperties.AppendChild(new Underline());
            
            if (!string.IsNullOrEmpty(fontStyle.FontName))
                runProperties.AppendChild(new RunFonts() { Ascii = fontStyle.FontName });
            
            if (fontStyle.FontSize > 0)
                runProperties.AppendChild(new FontSize() { Val = (fontStyle.FontSize * 2).ToString() });
            
            if (fontStyle.Color != null)
                runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = fontStyle.Color.ToArgb().ToString("X8")[2..] });
            
            run.AppendChild(runProperties);
            run.AppendChild(new Text(text));
            paragraph.AppendChild(run);
            _openXmlCell.AppendChild(paragraph);
        }

        public IParagraph AddParagraph()
        {
            var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            _openXmlCell.AppendChild(paragraph);
            return new OpenXmlParagraph(Paragraphs as OpenXmlParagraphs, paragraph);
        }

        public IParagraph AddParagraph(string text)
        {
            var paragraph = AddParagraph();
            paragraph.Text = text;
            return paragraph;
        }

        public void SetBackgroundColor(AMCode.Documents.Common.Drawing.Color color)
        {
            var cellProperties = _openXmlCell.TableCellProperties ?? new TableCellProperties();
            cellProperties.Shading = new Shading()
            {
                Val = ShadingPatternValues.Clear,
                Color = color.ToArgb().ToString("X8")[2..]
            };
            _openXmlCell.TableCellProperties = cellProperties;
        }

        public void SetBorder(BorderStyle borderStyle)
        {
            var cellProperties = _openXmlCell.TableCellProperties ?? new TableCellProperties();
            
            var borders = new TableBorders();
            
            if (borderStyle.Sides.HasFlag(BorderSides.Top))
            {
                borders.TopBorder = new TopBorder()
                {
                    Val = GetBorderLineStyle(borderStyle.LineStyle),
                    Color = borderStyle.Color.ToArgb().ToString("X8")[2..]
                };
            }
            
            if (borderStyle.Sides.HasFlag(BorderSides.Bottom))
            {
                borders.BottomBorder = new BottomBorder()
                {
                    Val = GetBorderLineStyle(borderStyle.LineStyle),
                    Color = borderStyle.Color.ToArgb().ToString("X8")[2..]
                };
            }
            
            if (borderStyle.Sides.HasFlag(BorderSides.Left))
            {
                borders.LeftBorder = new LeftBorder()
                {
                    Val = GetBorderLineStyle(borderStyle.LineStyle),
                    Color = borderStyle.Color.ToArgb().ToString("X8")[2..]
                };
            }
            
            if (borderStyle.Sides.HasFlag(BorderSides.Right))
            {
                borders.RightBorder = new RightBorder()
                {
                    Val = GetBorderLineStyle(borderStyle.LineStyle),
                    Color = borderStyle.Color.ToArgb().ToString("X8")[2..]
                };
            }
            
            cellProperties.AppendChild(borders);
            _openXmlCell.TableCellProperties = cellProperties;
        }

        private static BorderValues GetBorderLineStyle(BorderLineStyle lineStyle)
        {
            return lineStyle switch
            {
                BorderLineStyle.None => BorderValues.None,
                BorderLineStyle.Thin => BorderValues.Single,
                BorderLineStyle.Medium => BorderValues.Single,
                BorderLineStyle.Thick => BorderValues.Single,
                BorderLineStyle.Double => BorderValues.Double,
                BorderLineStyle.Dashed => BorderValues.Dashed,
                BorderLineStyle.Dotted => BorderValues.Dotted,
                _ => BorderValues.Single
            };
        }
    }
}
