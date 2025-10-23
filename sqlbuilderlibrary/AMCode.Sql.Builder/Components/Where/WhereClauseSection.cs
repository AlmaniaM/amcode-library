using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to represent an entire WHERE clause section such as "FilterName IN (...values)".
    /// </summary>
    public class WhereClauseSection : IWhereClauseSection
    {
        /// <summary>
        /// Creates a string of filter conditions, all separated by their defined operators. This
        /// <see cref="IWhereClauseSection"/> itself will be separated by its operator as well if
        /// it's not the first section after the "WHERE" clause.
        /// </summary>
        /// <param name="isFirstInClause">True if this section is the first one to appear after the "WHERE" clause. False if not.</param>
        /// <returns>A <see cref="string"/> representing a single complete section of a WHERE clause condition.</returns>
        public string CreateWhereClauseSectionString(bool isFirstInClause)
        {
            if (!HasAny)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            if (!isFirstInClause)
            {
                sb.Append(OperatorSeparator);
                sb.Append(' ');
            }

            FilterConditionSections.ForEach((IFilterConditionSection conditionSection, int index) =>
            {
                sb.Append(conditionSection.CreateFilterClauseString(index == 0));
                sb.Append(' ');
            });

            return sb.ToString();
        }

        /// <summary>
        /// Holds a list of filter conditions that will be used as a single condition in
        /// higher level where clause conditions.
        /// </summary>
        public IList<IFilterConditionSection> FilterConditionSections { get; set; }

        /// <summary>
        /// Check to see if there are any <see cref="FilterConditionSection"/>s.
        /// </summary>
        public bool HasAny => FilterConditionSections != null && FilterConditionSections.Count > 0;

        /// <summary>
        /// A <see cref="string"/> separator. Should be either "AND" or "OR".
        /// </summary>
        public string OperatorSeparator { get; set; } = string.Empty;

        /// <summary>
        /// Determines what filter condition section this where clause section belongs.
        /// </summary>
        public FilterConditionSectionType SectionType { get; set; }

        /// <inheritdoc/>
        public bool IsValid => getInvalidFilterInSections().Count() == 0;

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (!IsValid)
                {
                    return $"Invalid filter IN sections for WHERE clause section. Values are {string.Join(", ", getInvalidFilterInSections().Select(section => $"'{section.InvalidCommandMessage}"))}.";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Get all invalid <see cref="IFilterConditionSection"/>s.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of invalid <see cref="IFilterConditionSection"/>s.</returns>
        private IEnumerable<IFilterConditionSection> getInvalidFilterInSections() => FilterConditionSections.Where(section => !section.IsValid);
    }
}