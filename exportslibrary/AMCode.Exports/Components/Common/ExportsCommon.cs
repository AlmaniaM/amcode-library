using System;
using System.Collections.Generic;
using System.Linq;

namespace AMCode.Exports.Common
{
    /// <summary>
    /// A class designed to hold common methods and properties for the <see cref="BookBuilder"/> <c>namestapce</c>.
    /// </summary>
    internal static class ExportsCommon
    {
        /// <summary>
        /// Calculate the starting record numbers for all files.
        /// </summary>
        /// <param name="startIndex">The first starting row.</param>
        /// <param name="recordsPerChunk">The maximum number or rows per chunk.</param>
        /// <param name="totalRecords">The total row count for each .</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="int"/> starting row values.</returns>
        public static IEnumerable<int> CalculateChunkStartingPoints(int startIndex, int recordsPerChunk, int totalRecords)
        {
            var numberOfChunks = CalculateNumberOfChunks(totalRecords, recordsPerChunk);
            return Enumerable.Range(0, (int)numberOfChunks).Select(index =>
            {
                if (index == 0)
                {
                    return startIndex;
                }

                return startIndex + (recordsPerChunk * index);
            });
        }

        /// <summary>
        /// Calculate the number of books needed to be built.
        /// </summary>
        /// <param name="totalRecordCount">The total number of records.</param>
        /// <param name="maxRecordsPerChunk">The number of allowed records per chunk.</param>
        /// <returns>A <see cref="int"/> representing the number of books needed to be built.</returns>
        public static int CalculateNumberOfChunks(int totalRecordCount, int maxRecordsPerChunk) => (int)Math.Ceiling((double)totalRecordCount / maxRecordsPerChunk);
    }
}