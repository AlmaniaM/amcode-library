using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableRows
    /// </summary>
    public class OpenXmlTableRows : ITableRows
    {
        private readonly OpenXmlTable _table;
        private readonly List<ITableRow> _rows;

        public int Count => _rows.Count;
        public ITable Table => _table;

        public ITableRow this[int index] => _rows[index];

        public OpenXmlTableRows(OpenXmlTable table)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _rows = new List<ITableRow>();
            
            // Initialize with existing rows
            InitializeRows();
        }

        private void InitializeRows()
        {
            var openXmlRows = _table.OpenXmlElement.Elements<TableRow>();
            int index = 0;
            foreach (var openXmlRow in openXmlRows)
            {
                _rows.Add(new OpenXmlTableRow(_table, openXmlRow, index++));
            }
        }

        public ITableRow Create()
        {
            var tableRow = new TableRow();
            
            // Add cells to match column count
            for (int col = 0; col < _table.ColumnCount; col++)
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
            
            _table.OpenXmlElement.AppendChild(tableRow);
            
            var row = new OpenXmlTableRow(_table, tableRow, _rows.Count);
            _rows.Add(row);
            return row;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _rows.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var row = _rows[index];
            Remove(row);
        }

        public void Remove(ITableRow row)
        {
            if (row is OpenXmlTableRow openXmlRow)
            {
                openXmlRow.OpenXmlElement.Remove();
                _rows.Remove(row);
            }
        }

        public IEnumerator<ITableRow> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
