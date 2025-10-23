using System;

namespace AMCode.Data.SQLTests.Data.GenericDataProviderTests.Models
{
    public class HeaderNumbersObject
    {
        public string HeaderOne { get; set; }
        public string HeaderTwo { get; set; }
        public string HeaderThree { get; set; }
        public string HeaderFour { get; set; }
        public string HeaderFive { get; set; }
    }

    public class HeaderNamesObject
    {
        public bool HeaderBoolean { get; set; }
        public char HeaderChar { get; set; }
        public DateTime HeaderDate { get; set; }
        public int HeaderInteger { get; set; }
        public double HeaderNumeric { get; set; }
        public string HeaderString { get; set; }
        public DateTime HeaderTimeStamp { get; set; }
    }

    public class HeaderNamesObjectOptional
    {
        public bool? HeaderBoolean { get; set; }
        public char? HeaderChar { get; set; }
        public DateTime? HeaderDate { get; set; }
        public int? HeaderInteger { get; set; }
        public double? HeaderNumeric { get; set; }
        public string HeaderString { get; set; }
        public DateTime? HeaderTimeStamp { get; set; }
    }

    public class ColumnNamesObjectOptional
    {
        public bool? BooleanColumn { get; set; }
        public char? CharColumn { get; set; }
        public DateTime? DateColumn { get; set; }
        public int? IntegerColumn { get; set; }
        public double? NumericColumn { get; set; }
        public string StringColumn { get; set; }
        public DateTime? TimeStampColumn { get; set; }
    }
}