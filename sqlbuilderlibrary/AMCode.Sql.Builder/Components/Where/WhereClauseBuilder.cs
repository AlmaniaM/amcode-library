using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Where.Exceptions;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class that creates a WHERE clause which favors the last selected filter. The WHERE clause
    /// constructed by this builder will look something like so:
    /// WHERE filter1 IN (...values) OR filter1 IN (...valeus) AND filter2 IN (...values).
    /// </summary>
    public class WhereClauseBuilder : IWhereClauseBuilder
    {
        private readonly IDictionary<FilterConditionSectionType, WhereClauseSection> filterInClauseSections;

        /// <summary>
        /// Creates an instance of the <see cref="WhereClauseBuilder"/> class.
        /// </summary>
        public WhereClauseBuilder()
        {
            filterInClauseSections = new Dictionary<FilterConditionSectionType, WhereClauseSection>
            {
                [FilterConditionSectionType.LastSelected] = new WhereClauseSection
                {
                    FilterConditionSections = new List<IFilterConditionSection>(),
                    OperatorSeparator = "OR"
                },
                [FilterConditionSectionType.Default] = new WhereClauseSection
                {
                    FilterConditionSections = new List<IFilterConditionSection>(),
                    OperatorSeparator = "OR"
                }
            };
        }

        /// <inheritdoc/>
        public void AddFilterCondition(IFilterConditionSection filterConditionSection, FilterConditionSectionType filterConditionSectionType)
        {
            void throwNoSuchFilterConditionSectionException() => throw new NoSuchFilterConditionSectionTypeException(
                    $"[{nameof(WhereClauseBuilder)}][{nameof(AddFilterCondition)}]",
                    $"{filterConditionSectionType}"
                );

            if (!Enum.IsDefined(typeof(FilterConditionSectionType), filterConditionSectionType))
            {
                throwNoSuchFilterConditionSectionException();
            }

            if (filterInClauseSections.TryGetValue(filterConditionSectionType, out var whereClauseSection))
            {
                whereClauseSection.FilterConditionSections.Add(filterConditionSection);
            }
            else
            {
                throwNoSuchFilterConditionSectionException();
            }
        }

        /// <inheritdoc/>
        public IWhereClauseCommand CreateWhereClause()
        {
            var lastSelectedSectionExists = filterInClauseSections.TryGetValue(FilterConditionSectionType.LastSelected, out var lastSelectedWhereClauseSection);
            var defaultSectionExists = filterInClauseSections.TryGetValue(FilterConditionSectionType.Default, out var defaultWhereClauseSection);
            var whereClauseSections = new List<IWhereClauseSection>();

            if (lastSelectedSectionExists && lastSelectedWhereClauseSection.HasAny)
            {
                whereClauseSections.Add(lastSelectedWhereClauseSection);
            }

            if (defaultSectionExists && defaultWhereClauseSection.HasAny)
            {
                whereClauseSections.Add(defaultWhereClauseSection);
            }

            return new WhereClauseCommand(whereClauseSections.ToList());
        }
    }
}