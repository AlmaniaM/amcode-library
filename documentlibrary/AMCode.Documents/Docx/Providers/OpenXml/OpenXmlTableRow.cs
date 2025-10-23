using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableRow
    /// </summary>
    public class OpenXmlTableRow : ITableRow
    {
        private readonly OpenXmlTable _table;
        private readonly TableRow _openXmlRow;
        private readonly int _index;

        public ITableCells Cells { get; }
        public int Index => _index;
        public ITable Table => _table;

        public ITableCell this[int columnIndex] => GetCell(columnIndex);

        internal TableRow OpenXmlElement => _openXmlRow;

        public OpenXmlTableRow(OpenXmlTable table, TableRow openXmlRow, int index)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _openXmlRow = openXmlRow ?? throw new ArgumentNullException(nameof(openXmlRow));
            _index = index;
            Cells = new OpenXmlTableCells(this);
        }

        public ITableCell AddCell()
        {
            var tableCell = new TableCell();
            var cellProperties = new TableCellProperties();
            var cellWidth = new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" };
            cellProperties.AppendChild(cellWidth);
            tableCell.AppendChild(cellProperties);
            
            // Add empty paragraph to cell
            var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            tableCell.AppendChild(paragraph);
            
            _openXmlRow.AppendChild(tableCell);
            
            return new OpenXmlTableCell(_table, tableCell, _index, _openXmlRow.Elements<TableCell>().Count() - 1);
        }

        public ITableCell AddCell(string content)
        {
            var cell = AddCell();
            cell.Text = content;
            return cell;
        }

        public void RemoveCell(int columnIndex)
        {
            var cells = _openXmlRow.Elements<TableCell>().ToList();
            if (columnIndex >= 0 && columnIndex < cells.Count)
            {
                cells[columnIndex].Remove();
            }
        }

        private ITableCell GetCell(int columnIndex)
        {
            var cells = _openXmlRow.Elements<TableCell>().ToList();
            if (columnIndex >= 0 && columnIndex < cells.Count)
            {
                return new OpenXmlTableCell(_table, cells[columnIndex], _index, columnIndex);
            }
            return null;
        }
    }
}
