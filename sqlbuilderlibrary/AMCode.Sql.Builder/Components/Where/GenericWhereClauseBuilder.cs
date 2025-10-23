using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Where.Exceptions;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class responsible for constructing a SQL WHERE clause which contains a simple
    /// "Filter IN OR Filter IN" rule.
    /// </summary>
    public class GenericWhereClauseBuilder : IWhereClauseBuilder
    {
        private readonly IList<IWhereClauseSection> whereClauseSections;

        /// <summary>
        /// Create an instance of the <see cref="GenericWhereClauseBuilder"/> class.
        /// </summary>
        /// <param name="whereClauseSections">A <see cref="IList{T}"/> of <see cref="IWhereClauseSection"/>s to build a WHERE clause from.</param>
        public GenericWhereClauseBuilder(IList<IWhereClauseSection> whereClauseSections)
        {
            this.whereClauseSections = whereClauseSections;
        }

        /// <inheritdoc/>
        public void AddFilterCondition(IFilterConditionSection filterConditionSection, FilterConditionSectionType filterConditionSectionType)
        {
            if (!Enum.IsDefined(typeof(FilterConditionSectionType), filterConditionSectionType))
            {
                throw new NoSuchFilterConditionSectionTypeException(
                    $"[{nameof(GenericWhereClauseBuilder)}][{nameof(AddFilterCondition)}]({nameof(filterConditionSection)}, {nameof(filterConditionSectionType)})",
                    $"{filterConditionSectionType}"
                );
            }

            var whereClauseSection = whereClauseSections.Where(section => section.SectionType == filterConditionSectionType)?.FirstOrDefault();
            if (whereClauseSection != null)
            {
                whereClauseSection.FilterConditionSections.Add(filterConditionSection);
            }
        }

        /// <inheritdoc/>
        public IWhereClauseCommand CreateWhereClause()
            => new WhereClauseCommand(whereClauseSections.Where(whereClauseSection => whereClauseSection.HasAny));
    }
}