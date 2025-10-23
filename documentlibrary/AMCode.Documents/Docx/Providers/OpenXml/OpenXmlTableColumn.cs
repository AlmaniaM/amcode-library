using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableColumn
    /// </summary>
    public class OpenXmlTableColumn : ITableColumn
    {
        private readonly OpenXmlTable _table;
        private readonly int _index;

        public int Index => _index;
        public ITable Table => _table;

        public double Width
        {
            get
            {
                var firstRow = _table.OpenXmlElement.Elements<TableRow>().FirstOrDefault();
                if (firstRow != null)
                {
                    var cells = firstRow.Elements<TableCell>().ToList();
                    if (_index < cells.Count)
                    {
                        var cellWidth = cells[_index].TableCellProperties?.TableCellWidth;
                        if (cellWidth != null && double.TryParse(cellWidth.Width, out double width))
                        {
                            return width / 20; // Convert from DXA to points
                        }
                    }
                }
                return 2000 / 20; // Default width
            }
            set
            {
                SetWidth(value);
            }
        }

        public ITableCell this[int rowIndex] => GetCell(rowIndex);

        internal int ColumnIndex => _index;

        public OpenXmlTableColumn(OpenXmlTable table, int index)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _index = index;
        }

        public void SetWidth(double width)
        {
            var rows = _table.OpenXmlElement.Elements<TableRow>().ToList();
            foreach (var row in rows)
            {
                var cells = row.Elements<TableCell>().ToList();
                if (_index < cells.Count)
                {
                    var cellProperties = cells[_index].TableCellProperties ?? new TableCellProperties();
                    cellProperties.TableCellWidth = new TableCellWidth() 
                    { 
                        Type = TableWidthUnitValues.Dxa, 
                        Width = (width * 20).ToString() 
                    };
                    cells[_index].TableCellProperties = cellProperties;
                }
            }
        }

        private ITableCell GetCell(int rowIndex)
        {
            var rows = _table.OpenXmlElement.Elements<TableRow>().ToList();
            if (rowIndex >= 0 && rowIndex < rows.Count)
            {
                var cells = rows[rowIndex].Elements<TableCell>().ToList();
                if (_index < cells.Count)
                {
                    return new OpenXmlTableCell(_table, cells[_index], rowIndex, _index);
                }
            }
            return null;
        }
    }
}
