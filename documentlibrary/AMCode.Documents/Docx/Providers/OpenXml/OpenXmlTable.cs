using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITable
    /// </summary>
    public class OpenXmlTable : ITable
    {
        private readonly OpenXmlTables _tables;
        private readonly DocumentFormat.OpenXml.Wordprocessing.Table _openXmlTable;

        public ITableRows Rows { get; }
        public ITableColumns Columns { get; }
        public ITableStyle Style { get; set; }
        public ITables Tables => _tables;

        public int RowCount => _openXmlTable.Elements<TableRow>().Count();
        public int ColumnCount => _openXmlTable.Elements<TableRow>().FirstOrDefault()?.Elements<TableCell>().Count() ?? 0;

        public ITableCell this[int row, int column] => GetCell(row, column);

        internal DocumentFormat.OpenXml.Wordprocessing.Table OpenXmlElement => _openXmlTable;

        public OpenXmlTable(OpenXmlTables tables, DocumentFormat.OpenXml.Wordprocessing.Table openXmlTable)
        {
            _tables = tables ?? throw new ArgumentNullException(nameof(tables));
            _openXmlTable = openXmlTable ?? throw new ArgumentNullException(nameof(openXmlTable));
            
            Rows = new OpenXmlTableRows(this);
            Columns = new OpenXmlTableColumns(this);
        }

        public ITableRow AddRow()
        {
            var tableRow = new TableRow();
            
            // Add cells to match column count
            for (int col = 0; col < ColumnCount; col++)
            {
                var tableCell = new TableCell();
                var cellProperties = new TableCellProperties();
                var cellWidth = new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" };
                cellProperties.AppendChild(cellWidth);
                tableCell.AppendChild(cellProperties);
                
                // Add empty paragraph to cell
                var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                tableCell.AppendChild(paragraph);
                
                tableRow.AppendChild(tableCell);
            }
            
            _openXmlTable.AppendChild(tableRow);
            var rowIndex = _openXmlTable.Elements<TableRow>().Count() - 1;
            return new OpenXmlTableRow(this, tableRow, rowIndex);
        }

        public ITableColumn AddColumn()
        {
            // Add a cell to each existing row
            var rows = _openXmlTable.Elements<TableRow>().ToList();
            foreach (var row in rows)
            {
                var tableCell = new TableCell();
                var cellProperties = new TableCellProperties();
                var cellWidth = new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" };
                cellProperties.AppendChild(cellWidth);
                tableCell.AppendChild(cellProperties);
                
                // Add empty paragraph to cell
                var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                tableCell.AppendChild(paragraph);
                
                row.AppendChild(tableCell);
            }
            
            return new OpenXmlTableColumn(this, ColumnCount - 1);
        }

        public void RemoveRow(int index)
        {
            var rows = _openXmlTable.Elements<TableRow>().ToList();
            if (index >= 0 && index < rows.Count)
            {
                rows[index].Remove();
            }
        }

        public void RemoveColumn(int index)
        {
            var rows = _openXmlTable.Elements<TableRow>().ToList();
            foreach (var row in rows)
            {
                var cells = row.Elements<TableCell>().ToList();
                if (index >= 0 && index < cells.Count)
                {
                    cells[index].Remove();
                }
            }
        }

        public void ApplyStyle(ITableStyle style)
        {
            Style = style;
            
            // Apply table style properties
            var tableProperties = _openXmlTable.GetFirstChild<TableProperties>();
            if (tableProperties == null)
            {
                tableProperties = new TableProperties();
                _openXmlTable.PrependChild(tableProperties);
            }
            
            // Set table style
            if (!string.IsNullOrEmpty(style.Name))
            {
                tableProperties.TableStyle = new TableStyle() { Val = style.Name };
            }
            
            // Apply styling to rows and cells
            var rows = _openXmlTable.Elements<TableRow>().ToList();
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = rows[rowIndex];
                var cells = row.Elements<TableCell>().ToList();
                
                for (int colIndex = 0; colIndex < cells.Count; colIndex++)
                {
                    var cell = cells[colIndex];
                    var cellProperties = cell.TableCellProperties ?? new TableCellProperties();
                    
                    // Apply background color
                    if (rowIndex == 0 && style.HeaderBackgroundColor != null)
                    {
                        var shading = new Shading()
                        {
                            Val = ShadingPatternValues.Clear,
                            Color = style.HeaderBackgroundColor.ToArgb().ToString("X8")[2..]
                        };
                        cellProperties.Shading = shading;
                    }
                    else if (rowIndex % 2 == 1 && style.UseAlternatingRows && style.AlternatingRowColor != null)
                    {
                        var shading = new Shading()
                        {
                            Val = ShadingPatternValues.Clear,
                            Color = style.AlternatingRowColor.ToArgb().ToString("X8")[2..]
                        };
                        cellProperties.Shading = shading;
                    }
                    
                    cell.TableCellProperties = cellProperties;
                }
            }
        }

        public void SetCellValue(int row, int column, string value)
        {
            var cell = GetCell(row, column);
            if (cell != null)
            {
                cell.Text = value;
            }
        }

        public string GetCellValue(int row, int column)
        {
            var cell = GetCell(row, column);
            return cell?.Text ?? string.Empty;
        }

        private ITableCell GetCell(int row, int column)
        {
            var rows = _openXmlTable.Elements<TableRow>().ToList();
            if (row >= 0 && row < rows.Count)
            {
                var cells = rows[row].Elements<TableCell>().ToList();
                if (column >= 0 && column < cells.Count)
                {
                    return new OpenXmlTableCell(this, cells[column], row, column);
                }
            }
            return null;
        }
    }
}
