using System;
using System.Collections.Generic;

namespace AMCode.Data.UnitTests.Extensions.Models
{
    public class DataReaderExtensionsMock
    {
        public string HeaderOne { get; set; }
        public string HeaderTwo { get; set; }
        public string HeaderThree { get; set; }
    }

    public class DataReaderExtensionsColumnPropertyMock
    {
        public string ColumnOne { get; set; }
        public string ColumnTwo { get; set; }
        public string ColumnThree { get; set; }
    }

    public class DataReaderExtensionsColumnPropertyObjecMock
    {
        public DataReaderValueMock ColumnOne { get; set; }
        public int[] ColumnTwo { get; set; }
        public List<string> ColumnThree { get; set; }
    }

    public class DataReaderExtensionsColumnTypePropertyMock
    {
        public Type DataType { get; set; }
    }

    public class DataReaderValueMock
    {
        public string Value { get; set; }
    }

    public class TestPropertyMock
    {
        public string TestProperty { get; set; }
    }
}