using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface representing the base filter condition builder.
    /// </summary>
    public interface IFilterConditionBuilder
    {
        /// <summary>
        /// Creates a complete filter clause. Example: "filterName CLAUSE ...values" or
        /// "(filterName CLAUSE ...values OR filterName IS NULL)" or "filterName IS NULL".
        /// </summary>
        /// <returns>A <see cref="IFilterConditionSection"/> object.</returns>
        IFilterConditionSection CreateFilterClause();

        /// <summary>
        /// Creates a field name for the WHERE clause. 
        /// </summary>
        /// <param name="filter">A <seealso cref="IFilter"/> object.</param>
        /// <returns>A string representing the field name of the filter. This could be the ID version of the field name.
        /// We have description columns with corresponding ID columns on some fields so this method will return the ID
        /// name if it exists in the current filter.</returns>
        string GetFilterName(IFilter filter);
    }
}