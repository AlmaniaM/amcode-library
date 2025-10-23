using System.Collections.Generic;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface representing a WHERE clause section.
    /// </summary>
    public interface IWhereClauseSection : IValidCommand
    {
        /// <summary>
        /// Holds a list of filter conditions that will be used as a single condition in
        /// higher level where clause conditions.
        /// </summary>
        IList<IFilterConditionSection> FilterConditionSections { get; }

        /// <summary>
        /// Check to see if there are any <see cref="IFilterConditionSection"/>s.
        /// </summary>
        bool HasAny { get; }

        /// <summary>
        /// A <see cref="string"/> separator. Should be either "AND" or "OR".
        /// </summary>
        string OperatorSeparator { get; }

        /// <summary>
        /// Determines what filter condition section this where clause section belongs.
        /// </summary>
        FilterConditionSectionType SectionType { get; }

        /// <summary>
        /// Creates a string of filter conditions, all separated by their defined operators. This
        /// <see cref="IWhereClauseSection"/> itself will be separated by its operator as well if
        /// it's not the first section after the "WHERE" clause.
        /// </summary>
        /// <param name="isFirstInClause">True if this section is the first one to appear after the "WHERE" clause. False if not.</param>
        /// <returns>A <see cref="string"/> representing a single complete section of a WHERE clause condition.</returns>
        string CreateWhereClauseSectionString(bool isFirstInClause);
    }
}