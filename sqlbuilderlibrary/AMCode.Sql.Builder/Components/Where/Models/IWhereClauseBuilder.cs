using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface for constructing a WHERE clause. A class that implements this interface
    /// is responsible for constructing the actual WHERE clause "filter IN", "filter =", "filter is/is not", etc...
    /// conditional rules and placement of those rules.
    /// </summary>
    public interface IWhereClauseBuilder
    {
        /// <summary>
        /// Add a <see cref="IFilterConditionSection"/> to be included in the WHERE clause.
        /// </summary>
        /// <param name="filterConditionSection">The <see cref="IFilterConditionSection"/> to be included.</param>
        /// <param name="filterConditionSectionType">The <see cref="FilterConditionSectionType"/> of the provided
        /// <see cref="FilterConditionSection"/>.</param>
        void AddFilterCondition(IFilterConditionSection filterConditionSection, FilterConditionSectionType filterConditionSectionType);

        /// <summary>
        /// Construct a <see cref="IWhereClauseCommand"/> object.
        /// </summary>
        /// <returns>A <see cref="IWhereClauseCommand"/> object constructed from the provided <see cref="IFilterConditionSection"/>'s.</returns>
        IWhereClauseCommand CreateWhereClause();
    }
}