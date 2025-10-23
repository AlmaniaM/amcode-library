using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableCells
    /// </summary>
    public class OpenXmlTableCells : ITableCells
    {
        private readonly OpenXmlTableRow _row;
        private readonly List<ITableCell> _cells;

        public int Count => _cells.Count;
        public ITableRow Row => _row;

        public ITableCell this[int index] => _cells[index];

        public OpenXmlTableCells(OpenXmlTableRow row)
        {
            _row = row ?? throw new ArgumentNullException(nameof(row));
            _cells = new List<ITableCell>();
            
            // Initialize with existing cells
            InitializeCells();
        }

        private void InitializeCells()
        {
            var openXmlCells = _row.OpenXmlElement.Elements<TableCell>();
            int columnIndex = 0;
            foreach (var openXmlCell in openXmlCells)
            {
                if (_row.Table is OpenXmlTable openXmlTable)
                {
                    _cells.Add(new OpenXmlTableCell(openXmlTable, openXmlCell, _row.Index, columnIndex++));
                }
            }
        }

        public ITableCell Create()
        {
            var tableCell = new TableCell();
            var cellProperties = new TableCellProperties();
            var cellWidth = new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2000" };
            cellProperties.AppendChild(cellWidth);
            tableCell.AppendChild(cellProperties);
            
            // Add empty paragraph to cell
            var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            tableCell.AppendChild(paragraph);
            
            _row.OpenXmlElement.AppendChild(tableCell);
            
            if (_row.Table is OpenXmlTable openXmlTable)
            {
                var cell = new OpenXmlTableCell(openXmlTable, tableCell, _row.Index, _cells.Count);
                _cells.Add(cell);
                return cell;
            }
            return null;
        }

        public ITableCell Create(string content)
        {
            var cell = Create();
            cell.Text = content;
            return cell;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var cell = _cells[index];
            Remove(cell);
        }

        public void Remove(ITableCell cell)
        {
            if (cell is OpenXmlTableCell openXmlCell)
            {
                openXmlCell.OpenXmlElement.Remove();
                _cells.Remove(cell);
            }
        }

        public IEnumerator<ITableCell> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
