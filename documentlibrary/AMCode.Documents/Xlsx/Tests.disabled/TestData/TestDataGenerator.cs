using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AMCode.Documents.Xlsx.Tests.TestData
{
    /// <summary>
    /// Generator for creating test data arrays, tables, formulas, and formatting
    /// </summary>
    public static class TestDataGenerator
    {
        #region Test Data Arrays

        /// <summary>
        /// Creates a test data array with specified dimensions
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="prefix">Prefix for data values</param>
        /// <returns>2D array with test data</returns>
        public static object[,] CreateTestDataArray(int rows, int columns, string prefix = "R")
        {
            var data = new object[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    data[i, j] = $"{prefix}{i + 1}C{j + 1}";
                }
            }
            return data;
        }

        /// <summary>
        /// Creates a test data array with numeric values
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="startValue">Starting value</param>
        /// <param name="increment">Increment value</param>
        /// <returns>2D array with numeric test data</returns>
        public static object[,] CreateNumericTestDataArray(int rows, int columns, int startValue = 1, int increment = 1)
        {
            var data = new object[rows, columns];
            var value = startValue;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    data[i, j] = value;
                    value += increment;
                }
            }
            return data;
        }

        /// <summary>
        /// Creates a test data array with mixed data types
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>2D array with mixed test data</returns>
        public static object[,] CreateMixedTestDataArray(int rows, int columns)
        {
            var data = new object[rows, columns];
            var random = new Random();
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    switch (j % 4)
                    {
                        case 0:
                            data[i, j] = $"Text{i + 1}{j + 1}";
                            break;
                        case 1:
                            data[i, j] = random.Next(1, 100);
                            break;
                        case 2:
                            data[i, j] = DateTime.Now.AddDays(i + j);
                            break;
                        case 3:
                            data[i, j] = random.NextDouble() * 100;
                            break;
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Creates a test data array with specific patterns
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="pattern">Pattern type</param>
        /// <returns>2D array with patterned test data</returns>
        public static object[,] CreatePatternedTestDataArray(int rows, int columns, DataPattern pattern)
        {
            var data = new object[rows, columns];
            
            switch (pattern)
            {
                case DataPattern.Sequential:
                    var value = 1;
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            data[i, j] = value++;
                        }
                    }
                    break;
                    
                case DataPattern.Alternating:
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            data[i, j] = (i + j) % 2 == 0 ? "Even" : "Odd";
                        }
                    }
                    break;
                    
                case DataPattern.Random:
                    var random = new Random();
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            data[i, j] = random.Next(1, 1000);
                        }
                    }
                    break;
                    
                case DataPattern.Fibonacci:
                    var fib = new int[rows * columns];
                    fib[0] = 0;
                    fib[1] = 1;
                    for (int i = 2; i < fib.Length; i++)
                    {
                        fib[i] = fib[i - 1] + fib[i - 2];
                    }
                    
                    var index = 0;
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            data[i, j] = fib[index++];
                        }
                    }
                    break;
            }
            
            return data;
        }

        #endregion

        #region Test Data Lists

        /// <summary>
        /// Creates a test data list with specified dimensions
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="prefix">Prefix for data values</param>
        /// <returns>List of lists with test data</returns>
        public static List<List<object>> CreateTestDataList(int rows, int columns, string prefix = "R")
        {
            var data = new List<List<object>>();
            for (int i = 0; i < rows; i++)
            {
                var row = new List<object>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add($"{prefix}{i + 1}C{j + 1}");
                }
                data.Add(row);
            }
            return data;
        }

        /// <summary>
        /// Creates a test data list with numeric values
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="startValue">Starting value</param>
        /// <param name="increment">Increment value</param>
        /// <returns>List of lists with numeric test data</returns>
        public static List<List<object>> CreateNumericTestDataList(int rows, int columns, int startValue = 1, int increment = 1)
        {
            var data = new List<List<object>>();
            var value = startValue;
            for (int i = 0; i < rows; i++)
            {
                var row = new List<object>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add(value);
                    value += increment;
                }
                data.Add(row);
            }
            return data;
        }

        /// <summary>
        /// Creates a test data list with mixed data types
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>List of lists with mixed test data</returns>
        public static List<List<object>> CreateMixedTestDataList(int rows, int columns)
        {
            var data = new List<List<object>>();
            var random = new Random();
            
            for (int i = 0; i < rows; i++)
            {
                var row = new List<object>();
                for (int j = 0; j < columns; j++)
                {
                    switch (j % 4)
                    {
                        case 0:
                            row.Add($"Text{i + 1}{j + 1}");
                            break;
                        case 1:
                            row.Add(random.Next(1, 100));
                            break;
                        case 2:
                            row.Add(DateTime.Now.AddDays(i + j));
                            break;
                        case 3:
                            row.Add(random.NextDouble() * 100);
                            break;
                    }
                }
                data.Add(row);
            }
            return data;
        }

        #endregion

        #region Test Data Tables

        /// <summary>
        /// Creates a test DataTable with specified dimensions
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="prefix">Prefix for data values</param>
        /// <returns>DataTable with test data</returns>
        public static DataTable CreateTestDataTable(int rows, int columns, string prefix = "R")
        {
            var table = new DataTable();
            
            // Add columns
            for (int j = 0; j < columns; j++)
            {
                table.Columns.Add($"Column{j + 1}", typeof(string));
            }
            
            // Add rows
            for (int i = 0; i < rows; i++)
            {
                var row = table.NewRow();
                for (int j = 0; j < columns; j++)
                {
                    row[j] = $"{prefix}{i + 1}C{j + 1}";
                }
                table.Rows.Add(row);
            }
            
            return table;
        }

        /// <summary>
        /// Creates a test DataTable with numeric values
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="startValue">Starting value</param>
        /// <param name="increment">Increment value</param>
        /// <returns>DataTable with numeric test data</returns>
        public static DataTable CreateNumericTestDataTable(int rows, int columns, int startValue = 1, int increment = 1)
        {
            var table = new DataTable();
            
            // Add columns
            for (int j = 0; j < columns; j++)
            {
                table.Columns.Add($"Column{j + 1}", typeof(int));
            }
            
            // Add rows
            var value = startValue;
            for (int i = 0; i < rows; i++)
            {
                var row = table.NewRow();
                for (int j = 0; j < columns; j++)
                {
                    row[j] = value;
                    value += increment;
                }
                table.Rows.Add(row);
            }
            
            return table;
        }

        /// <summary>
        /// Creates a test DataTable with mixed data types
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>DataTable with mixed test data</returns>
        public static DataTable CreateMixedTestDataTable(int rows, int columns)
        {
            var table = new DataTable();
            var random = new Random();
            
            // Add columns with different types
            for (int j = 0; j < columns; j++)
            {
                switch (j % 4)
                {
                    case 0:
                        table.Columns.Add($"TextColumn{j + 1}", typeof(string));
                        break;
                    case 1:
                        table.Columns.Add($"IntColumn{j + 1}", typeof(int));
                        break;
                    case 2:
                        table.Columns.Add($"DateColumn{j + 1}", typeof(DateTime));
                        break;
                    case 3:
                        table.Columns.Add($"DoubleColumn{j + 1}", typeof(double));
                        break;
                }
            }
            
            // Add rows
            for (int i = 0; i < rows; i++)
            {
                var row = table.NewRow();
                for (int j = 0; j < columns; j++)
                {
                    switch (j % 4)
                    {
                        case 0:
                            row[j] = $"Text{i + 1}{j + 1}";
                            break;
                        case 1:
                            row[j] = random.Next(1, 100);
                            break;
                        case 2:
                            row[j] = DateTime.Now.AddDays(i + j);
                            break;
                        case 3:
                            row[j] = random.NextDouble() * 100;
                            break;
                    }
                }
                table.Rows.Add(row);
            }
            
            return table;
        }

        #endregion

        #region Test Formulas

        /// <summary>
        /// Creates an array of test formulas
        /// </summary>
        /// <param name="count">Number of formulas to create</param>
        /// <param name="formulaType">Type of formulas to create</param>
        /// <returns>Array of test formulas</returns>
        public static string[] CreateTestFormulas(int count, FormulaType formulaType = FormulaType.Basic)
        {
            var formulas = new string[count];
            
            switch (formulaType)
            {
                case FormulaType.Basic:
                    for (int i = 0; i < count; i++)
                    {
                        formulas[i] = $"=A{i + 1}+B{i + 1}";
                    }
                    break;
                    
                case FormulaType.Advanced:
                    var advancedFormulas = new[]
                    {
                        "=SUM(A1:A10)",
                        "=AVERAGE(B1:B10)",
                        "=MAX(C1:C10)",
                        "=MIN(D1:D10)",
                        "=COUNT(E1:E10)",
                        "=IF(A1>5,\"Yes\",\"No\")",
                        "=VLOOKUP(A1,Table1,2,FALSE)",
                        "=INDEX(A1:C10,2,3)",
                        "=MATCH(A1,B1:B10,0)",
                        "=CONCATENATE(A1,\" \",B1)"
                    };
                    
                    for (int i = 0; i < count; i++)
                    {
                        formulas[i] = advancedFormulas[i % advancedFormulas.Length];
                    }
                    break;
                    
                case FormulaType.Statistical:
                    var statisticalFormulas = new[]
                    {
                        "=STDEV(A1:A10)",
                        "=VAR(A1:A10)",
                        "=CORREL(A1:A10,B1:B10)",
                        "=COVAR(A1:A10,B1:B10)",
                        "=SLOPE(A1:A10,B1:B10)",
                        "=INTERCEPT(A1:A10,B1:B10)",
                        "=RSQ(A1:A10,B1:B10)",
                        "=FORECAST(A1,A2:A10,B2:B10)",
                        "=TREND(A1:A10,B1:B10,C1:C10)",
                        "=LINEST(A1:A10,B1:B10)"
                    };
                    
                    for (int i = 0; i < count; i++)
                    {
                        formulas[i] = statisticalFormulas[i % statisticalFormulas.Length];
                    }
                    break;
                    
                case FormulaType.Financial:
                    var financialFormulas = new[]
                    {
                        "=PV(0.05,10,1000)",
                        "=FV(0.05,10,1000)",
                        "=PMT(0.05,10,10000)",
                        "=RATE(10,1000,10000)",
                        "=NPV(0.05,A1:A10)",
                        "=IRR(A1:A10)",
                        "=MIRR(A1:A10,0.05,0.05)",
                        "=XNPV(0.05,A1:A10,B1:B10)",
                        "=XIRR(A1:A10,B1:B10)",
                        "=DB(10000,1000,5,1)"
                    };
                    
                    for (int i = 0; i < count; i++)
                    {
                        formulas[i] = financialFormulas[i % financialFormulas.Length];
                    }
                    break;
            }
            
            return formulas;
        }

        /// <summary>
        /// Creates a specific test formula
        /// </summary>
        /// <param name="formulaType">Type of formula to create</param>
        /// <param name="parameters">Parameters for the formula</param>
        /// <returns>Test formula string</returns>
        public static string CreateSpecificTestFormula(SpecificFormulaType formulaType, params object[] parameters)
        {
            switch (formulaType)
            {
                case SpecificFormulaType.Sum:
                    return $"=SUM({parameters[0]}:{parameters[1]})";
                    
                case SpecificFormulaType.Average:
                    return $"=AVERAGE({parameters[0]}:{parameters[1]})";
                    
                case SpecificFormulaType.Count:
                    return $"=COUNT({parameters[0]}:{parameters[1]})";
                    
                case SpecificFormulaType.Max:
                    return $"=MAX({parameters[0]}:{parameters[1]})";
                    
                case SpecificFormulaType.Min:
                    return $"=MIN({parameters[0]}:{parameters[1]})";
                    
                case SpecificFormulaType.If:
                    return $"=IF({parameters[0]},{parameters[1]},{parameters[2]})";
                    
                case SpecificFormulaType.VLookup:
                    return $"=VLOOKUP({parameters[0]},{parameters[1]},{parameters[2]},FALSE)";
                    
                case SpecificFormulaType.HLookup:
                    return $"=HLOOKUP({parameters[0]},{parameters[1]},{parameters[2]},FALSE)";
                    
                case SpecificFormulaType.Index:
                    return $"=INDEX({parameters[0]},{parameters[1]},{parameters[2]})";
                    
                case SpecificFormulaType.Match:
                    return $"=MATCH({parameters[0]},{parameters[1]},0)";
                    
                default:
                    return "=A1+B1";
            }
        }

        #endregion

        #region Test Formatting

        /// <summary>
        /// Creates test formatting data
        /// </summary>
        /// <param name="count">Number of formatting items to create</param>
        /// <param name="formatType">Type of formatting</param>
        /// <returns>Array of test formatting data</returns>
        public static object[] CreateTestFormatting(int count, FormatType formatType)
        {
            var formatting = new object[count];
            
            switch (formatType)
            {
                case FormatType.Fonts:
                    var fonts = new[] { "Arial", "Times New Roman", "Courier New", "Calibri", "Verdana" };
                    for (int i = 0; i < count; i++)
                    {
                        formatting[i] = fonts[i % fonts.Length];
                    }
                    break;
                    
                case FormatType.Colors:
                    var colors = new uint[] { 0xFF0000, 0x00FF00, 0x0000FF, 0xFFFF00, 0xFF00FF, 0x00FFFF, 0x000000, 0xFFFFFF };
                    for (int i = 0; i < count; i++)
                    {
                        formatting[i] = colors[i % colors.Length];
                    }
                    break;
                    
                case FormatType.Alignments:
                    var alignments = new[] { AlignmentType.Left, AlignmentType.Center, AlignmentType.Right, AlignmentType.Justify };
                    for (int i = 0; i < count; i++)
                    {
                        formatting[i] = alignments[i % alignments.Length];
                    }
                    break;
                    
                case FormatType.VerticalAlignments:
                    var verticalAlignments = new[] { VerticalAlignmentType.Top, VerticalAlignmentType.Middle, VerticalAlignmentType.Bottom };
                    for (int i = 0; i < count; i++)
                    {
                        formatting[i] = verticalAlignments[i % verticalAlignments.Length];
                    }
                    break;
                    
                case FormatType.Borders:
                    var borders = new[] { BorderType.Thin, BorderType.Medium, BorderType.Thick, BorderType.Double };
                    for (int i = 0; i < count; i++)
                    {
                        formatting[i] = borders[i % borders.Length];
                    }
                    break;
                    
                case FormatType.Fills:
                    var fills = new[] { FillType.Solid, FillType.Gradient, FillType.Pattern, FillType.Texture };
                    for (int i = 0; i < count; i++)
                    {
                        formatting[i] = fills[i % fills.Length];
                    }
                    break;
            }
            
            return formatting;
        }

        /// <summary>
        /// Creates test number formats
        /// </summary>
        /// <param name="count">Number of number formats to create</param>
        /// <returns>Array of test number formats</returns>
        public static string[] CreateTestNumberFormats(int count)
        {
            var formats = new[]
            {
                "0",
                "0.00",
                "#,##0",
                "#,##0.00",
                "0%",
                "0.00%",
                "0.00E+00",
                "mm/dd/yyyy",
                "dd/mm/yyyy",
                "yyyy-mm-dd",
                "h:mm AM/PM",
                "h:mm:ss AM/PM",
                "mm/dd/yyyy h:mm",
                "dd/mm/yyyy h:mm:ss",
                "yyyy-mm-dd h:mm:ss",
                "General",
                "Text",
                "Currency",
                "Accounting",
                "Date",
                "Time",
                "Percentage",
                "Fraction",
                "Scientific",
                "Custom"
            };
            
            var result = new string[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = formats[i % formats.Length];
            }
            
            return result;
        }

        #endregion

        #region Test Data Validation

        /// <summary>
        /// Creates test data validation rules
        /// </summary>
        /// <param name="count">Number of validation rules to create</param>
        /// <returns>Array of test data validation rules</returns>
        public static DataValidationRule[] CreateTestDataValidationRules(int count)
        {
            var rules = new DataValidationRule[count];
            
            var validationTypes = new[] { "List", "WholeNumber", "Decimal", "Date", "Time", "TextLength", "Custom" };
            var operators = new[] { "between", "notBetween", "equal", "notEqual", "greaterThan", "lessThan", "greaterThanOrEqual", "lessThanOrEqual" };
            var formulas = new[] { "1,100", "0,1000", "1/1/2020,12/31/2030", "0:00,23:59", "1,50", "=A1>0" };
            
            for (int i = 0; i < count; i++)
            {
                rules[i] = new DataValidationRule
                {
                    Type = validationTypes[i % validationTypes.Length],
                    Operator = operators[i % operators.Length],
                    Formula = formulas[i % formulas.Length]
                };
            }
            
            return rules;
        }

        #endregion

        #region Test Conditional Formatting

        /// <summary>
        /// Creates test conditional formatting rules
        /// </summary>
        /// <param name="count">Number of conditional formatting rules to create</param>
        /// <returns>Array of test conditional formatting rules</returns>
        public static ConditionalFormattingRule[] CreateTestConditionalFormattingRules(int count)
        {
            var rules = new ConditionalFormattingRule[count];
            
            var conditions = new[] { "CellValue", "Formula", "ColorScale", "DataBar", "IconSet" };
            var operators = new[] { "greaterThan", "lessThan", "equal", "notEqual", "between", "notBetween" };
            var values = new[] { "5", "10", "=A1>0", "1,10", "0,100" };
            
            for (int i = 0; i < count; i++)
            {
                rules[i] = new ConditionalFormattingRule
                {
                    Condition = conditions[i % conditions.Length],
                    Operator = operators[i % operators.Length],
                    Value = values[i % values.Length]
                };
            }
            
            return rules;
        }

        #endregion
    }

    #region Enums and Helper Classes

    /// <summary>
    /// Data pattern types for test data generation
    /// </summary>
    public enum DataPattern
    {
        Sequential,
        Alternating,
        Random,
        Fibonacci
    }

    /// <summary>
    /// Formula types for test formula generation
    /// </summary>
    public enum FormulaType
    {
        Basic,
        Advanced,
        Statistical,
        Financial
    }

    /// <summary>
    /// Specific formula types for targeted formula generation
    /// </summary>
    public enum SpecificFormulaType
    {
        Sum,
        Average,
        Count,
        Max,
        Min,
        If,
        VLookup,
        HLookup,
        Index,
        Match
    }

    /// <summary>
    /// Format types for test formatting generation
    /// </summary>
    public enum FormatType
    {
        Fonts,
        Colors,
        Alignments,
        VerticalAlignments,
        Borders,
        Fills
    }

    /// <summary>
    /// Data validation rule for test data generation
    /// </summary>
    public class DataValidationRule
    {
        public string Type { get; set; }
        public string Operator { get; set; }
        public string Formula { get; set; }
    }

    /// <summary>
    /// Conditional formatting rule for test data generation
    /// </summary>
    public class ConditionalFormattingRule
    {
        public string Condition { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    #endregion
}
