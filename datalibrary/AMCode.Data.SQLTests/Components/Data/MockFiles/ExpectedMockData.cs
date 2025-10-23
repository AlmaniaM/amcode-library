using System;
using System.Collections.Generic;
using System.Globalization;

namespace AMCode.Data.SQLTests.Data.DataProviderTests.MockFiles
{
    public class ExpectedMockData
    {
        public static IDictionary<string, IList<object>> GetDifferentDataTypes()
        {
            return new Dictionary<string, IList<object>>
            {
                ["HeaderBoolean"] = new List<object> { true, false, true, false, true, false, true },
                ["HeaderChar"] = new List<object> { 'c', 'h', 'a', 'r', 'b', 'd', 'e' },
                ["HeaderDate"] = new List<object> {
                    ParseDate("2010-01-11"),
                    ParseDate("2020-02-12"),
                    ParseDate("2030-03-13"),
                    ParseDate("2040-04-14"),
                    ParseDate("2050-05-15"),
                    ParseDate("2060-06-16"),
                    ParseDate("2070-07-17")
                },
                ["HeaderInteger"] = new List<object> { 1, 20, 300, 4000, 50000, 6000000, 70000000 },
                ["HeaderNumeric"] = new List<object> { 1.10, 20.20, 300.30, 4000.40, 50000.50, 6000000.60, 70000000.70 },
                ["HeaderString"] = new List<object> { "Row 1 Value 6", "Row 2 Value 6", "Row 3 Value 6", "Row 4 Value 6", "Row 5 Value 6", "Row 6 Value 6", "Row 7 Value 6" },
                ["HeaderTimeStamp"] = new List<object> {
                    ParseTimeStamp("2010-01-11 01:11:11"),
                    ParseTimeStamp("2020-02-12 02:12:12"),
                    ParseTimeStamp("2030-03-13 03:13:13"),
                    ParseTimeStamp("2040-04-14 04:14:14"),
                    ParseTimeStamp("2050-05-15 05:15:15"),
                    ParseTimeStamp("2060-06-16 06:16:16"),
                    ParseTimeStamp("2070-07-17 07:17:17")
                }
            };
        }

        public static IDictionary<string, IList<object>> GetDifferentDataTypesWithNulls()
        {
            return new Dictionary<string, IList<object>>
            {
                ["HeaderBoolean"] = new List<object> { default(bool?), false, true, false, true, false, true },
                ["HeaderChar"] = new List<object> { 'c', default(char?), 'a', 'r', 'b', 'd', 'e' },
                ["HeaderDate"] = new List<object> {
                    ParseDate("2010-01-11"),
                    ParseDate("2020-02-12"),
                    default(DateTime?),
                    ParseDate("2040-04-14"),
                    ParseDate("2050-05-15"),
                    ParseDate("2060-06-16"),
                    ParseDate("2070-07-17")
                },
                ["HeaderInteger"] = new List<object> { 1, 20, 300, default(int?), 50000, 6000000, 70000000 },
                ["HeaderNumeric"] = new List<object> { 1.10, 20.20, 300.30, 4000.40, default(double?), 6000000.60, 70000000.70 },
                ["HeaderString"] = new List<object> { "Row 1 Value 6", "Row 2 Value 6", "Row 3 Value 6", "Row 4 Value 6", "Row 5 Value 6", default(string), "Row 7 Value 6" },
                ["HeaderTimeStamp"] = new List<object> {
                    ParseTimeStamp("2010-01-11 01:11:11"),
                    ParseTimeStamp("2020-02-12 02:12:12"),
                    ParseTimeStamp("2030-03-13 03:13:13"),
                    ParseTimeStamp("2040-04-14 04:14:14"),
                    ParseTimeStamp("2050-05-15 05:15:15"),
                    ParseTimeStamp("2060-06-16 06:16:16"),
                    default(DateTime?)
                }
            };
        }

        public static DateTime ParseDate(string date) => DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        public static DateTime ParseTimeStamp(string date) => DateTime.ParseExact(date, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
    }
}