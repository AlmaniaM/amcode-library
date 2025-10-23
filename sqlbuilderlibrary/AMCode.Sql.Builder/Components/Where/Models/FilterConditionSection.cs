using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to create WHERE clause condition sections for <see cref="IFilterCondition"/>s.
    /// </summary>
    public class FilterConditionSection : IFilterConditionSection
    {
        /// <summary>
        /// Clone a this object.
        /// </summary>
        /// <returns>A <see cref="IFilterConditionSection"/> clone of this object.</returns>
        public IFilterConditionSection Clone()
        {
            var sectionToClone = (IFilterConditionSection)this;

            var clonedFilerInSection = new FilterConditionSection
            {
                OperatorSeparator = sectionToClone.OperatorSeparator,
                FilterCondition = sectionToClone.FilterCondition.Clone(),
                FilterSections = sectionToClone.FilterSections?
                    .Select((IFilterConditionSection section) => section.Clone()).ToList()
            };

            return clonedFilerInSection;
        }

        /// <summary>
        /// Creates a Filter CLAUSE ...values <see cref="string"/>.
        /// </summary>
        /// <param name="isFirstInClause">Provide true if this is the first
        /// Filter CLAUSE ...values <see cref="string"/> in the where clause.</param>
        /// <returns>A <see cref="string"/> Filter CLAUSE ...values.</returns>
        public string CreateFilterClauseString(bool isFirstInClause = false)
        {
            var sb = new StringBuilder();

            if (!isFirstInClause)
            {
                sb.Append(OperatorSeparator);
                sb.Append(' ');
            }

            if (FilterSections != null && FilterSections.Count > 0)
            {
                sb.Append('(');
            }

            sb.Append(FilterCondition.CreateFilterCondition());

            if (FilterSections != null && FilterSections.Count > 0)
            {
                FilterSections.ToList().ForEach((IFilterConditionSection section, int index) =>
                {
                    var hasMoreThanOneChildSection = section.FilterSections != null && section.FilterSections.Count > 1;
                    sb.Append(' ');
                    sb.Append(section.CreateFilterClauseString(hasMoreThanOneChildSection && index == 0));
                });
            }

            if (FilterSections != null && FilterSections.Count > 0)
            {
                sb.Append(')');
            }

            return sb.ToString();
        }

        /// <summary>
        /// An instance of a <see cref="IFilterCondition"/> type object that creates a filter condition <see cref="string"/>.
        /// </summary>
        public IFilterCondition FilterCondition { get; set; }

        /// <summary>
        /// An <see cref="IList{T}"/> of <see cref="IFilterConditionSection"/> objects. Each section defines
        /// its filterIn clause and 
        /// </summary>
        public IList<IFilterConditionSection> FilterSections { get; set; }

        /// <inheritdoc/>
        public bool IsValid => FilterCondition != null && FilterCondition.IsValid && getInvalidFilterInSectionsCount() == 0;

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (FilterCondition is null)
                {
                    return $"The main {nameof(FilterConditionSection)} cannot be null.";
                }

                if (!FilterCondition.IsValid)
                {
                    return $"The main {nameof(FilterConditionSection)} is not valid. Message '{FilterCondition.InvalidCommandMessage}'.";
                }

                if (getInvalidFilterInSectionsCount() > 0)
                {
                    var invalidMessages = string.Join(", ", getInvalidFilterInSections().Select(section => section is null ? "'null'" : $"'{section.InvalidCommandMessage}'"));
                    return $"Invalid filter IN sections. Values are {invalidMessages}.";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// A <see cref="string"/> separator. Should be either "AND" or "OR".
        /// </summary>
        public string OperatorSeparator { get; set; } = string.Empty;

        /// <summary>
        /// Get all invalid <see cref="IFilterConditionSection"/>s.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of invalid <see cref="IFilterConditionSection"/>s.</returns>
        private IEnumerable<IFilterConditionSection> getInvalidFilterInSections() => FilterSections?.Where(section => section is null || !section.IsValid);

        /// <summary>
        /// Get the number of invalid <see cref="getInvalidFilterInSections"/>.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the number of invalid <see cref="IFilterConditionSection"/>s or
        /// zero if there are not <see cref="FilterSections"/> or there are no invalid ones.</returns>
        private int getInvalidFilterInSectionsCount()
        {
            if (FilterSections is null)
            {
                return 0;
            }

            return getInvalidFilterInSections().Count();
        }
    }
}