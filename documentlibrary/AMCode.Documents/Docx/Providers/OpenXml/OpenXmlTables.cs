using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITables
    /// </summary>
    public class OpenXmlTables : ITables
    {
        private readonly OpenXmlDocument _document;
        private readonly List<ITable> _tables;

        public int Count => _tables.Count;
        public IDocument Document => _document;

        public ITable this[int index] => _tables[index];

        public OpenXmlTables(OpenXmlDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            _tables = new List<ITable>();
            
            // Initialize with existing tables
            InitializeTables();
        }

        private void InitializeTables()
        {
            var openXmlTables = _document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>();
            foreach (var openXmlTable in openXmlTables)
            {
                _tables.Add(new OpenXmlTable(this, openXmlTable));
            }
        }

        public ITable Create(int rows, int columns)
        {
            var openXmlTable = new DocumentFormat.OpenXml.Wordprocessing.Table();
            
            // Create table properties
            var tableProperties = new TableProperties();
            var tableStyle = new TableStyle() { Val = "TableGrid" };
            tableProperties.AppendChild(tableStyle);
            openXmlTable.AppendChild(tableProperties);
            
            // Create rows and cells
            for (int row = 0; row < rows; row++)
            {
                var tableRow = new TableRow();
                
                for (int col = 0; col < columns; col++)
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
                
                openXmlTable.AppendChild(tableRow);
            }
            
            _document.Body.AppendChild(openXmlTable);
            
            var table = new OpenXmlTable(this, openXmlTable);
            _tables.Add(table);
            return table;
        }

        public ITable Create(int rows, int columns, ITableStyle style)
        {
            var table = Create(rows, columns);
            table.ApplyStyle(style);
            return table;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _tables.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var table = _tables[index];
            Remove(table);
        }

        public void Remove(ITable table)
        {
            if (table is OpenXmlTable openXmlTable)
            {
                openXmlTable.OpenXmlElement.Remove();
                _tables.Remove(table);
            }
        }

        public IEnumerator<ITable> GetEnumerator()
        {
            return _tables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
