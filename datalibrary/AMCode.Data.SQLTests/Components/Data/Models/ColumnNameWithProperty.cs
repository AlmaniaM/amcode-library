using AMCode.Columns.Core;

namespace AMCode.Data.SQLTests.Data.Models
{
    public class ColumnNameWithProperty : ColumnName
    {
        public string PropertyName { get; set; }
    }
}