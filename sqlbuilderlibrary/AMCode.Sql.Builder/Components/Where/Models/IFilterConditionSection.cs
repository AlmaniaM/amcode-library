using System.Collections.Generic;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.Where.Internal;

namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// An interface representing a single filter condition section or a collection of WHERE clause sections.
    /// </summary>
    public interface IFilterConditionSection : IValidCommand
    {
        /// <summary>
        /// An instance of a <see cref="IFilterCondition"/> type object that creates a filter condition <see cref="string"/>.
        /// </summary>
        IFilterCondition FilterCondition { get; }

        /// <summary>
        /// An <see cref="IList{T}"/> of <see cref="IFilterConditionSection"/> objects. Each section defines
        /// its filterIn clause and 
        /// </summary>
        IList<IFilterConditionSection> FilterSections { get; }

        /// <summary>
        /// A <see cref="string"/> separator. Should be either "AND" or "OR".
        /// </summary>
        string OperatorSeparator { get; }

        /// <summary>
        /// Clone a this object.
        /// </summary>
        /// <returns>A <see cref="IFilterConditionSection"/> clone of this object.</returns>
        IFilterConditionSection Clone();

        /// <summary>
        /// Creates a Filter IN (...values) clause.
        /// </summary>
        /// <param name="isFirstInClause">Provide true if this is the first
        /// Filter IN (...values) clause in the where clause.</param>
        /// <returns>A <see cref="string"/> Filter IN (...values) clause.</returns>
        string CreateFilterClauseString(bool isFirstInClause = false);
    }
}