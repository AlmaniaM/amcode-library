using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableColumns
    /// </summary>
    public class OpenXmlTableColumns : ITableColumns
    {
        private readonly OpenXmlTable _table;
        private readonly List<ITableColumn> _columns;

        public int Count => _columns.Count;
        public ITable Table => _table;

        public ITableColumn this[int index] => _columns[index];

        public OpenXmlTableColumns(OpenXmlTable table)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _columns = new List<ITableColumn>();
            
            // Initialize with existing columns
            InitializeColumns();
        }

        private void InitializeColumns()
        {
            var firstRow = _table.OpenXmlElement.Elements<TableRow>().FirstOrDefault();
            if (firstRow != null)
            {
                var cells = firstRow.Elements<TableCell>();
                int index = 0;
                foreach (var cell in cells)
                {
                    _columns.Add(new OpenXmlTableColumn(_table, index++));
                }
            }
        }

        public ITableColumn Create()
        {
            // Add a cell to each existing row
            var rows = _table.OpenXmlElement.Elements<TableRow>().ToList();
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
            
            var column = new OpenXmlTableColumn(_table, _columns.Count);
            _columns.Add(column);
            return column;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var column = _columns[index];
            Remove(column);
        }

        public void Remove(ITableColumn column)
        {
            if (column is OpenXmlTableColumn openXmlColumn)
            {
                var rows = _table.OpenXmlElement.Elements<TableRow>().ToList();
                foreach (var row in rows)
                {
                    var cells = row.Elements<TableCell>().ToList();
                    if (openXmlColumn.Index < cells.Count)
                    {
                        cells[openXmlColumn.Index].Remove();
                    }
                }
                _columns.Remove(column);
            }
        }

        public IEnumerator<ITableColumn> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
