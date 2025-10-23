using System.Collections.Generic;
using AMCode.Sql.Where.Exceptions;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// A class for creating instances of <see cref="IWhereClauseBuilder"/>'s.
    /// </summary>
    public class WhereClauseBuilderFactory : IWhereClauseBuilderFactory
    {
        /// <summary>
        /// Create a <see cref="IWhereClauseBuilder"/> based on the provided <see cref="WhereClauseBuilderType"/>.
        /// </summary>
        /// <param name="whereClauseBuilderType">The <see cref="WhereClauseBuilderType"/> to build.</param>
        /// <returns>An instance of an object implementing a <see cref="IWhereClauseBuilder"/>.</returns>
        public IWhereClauseBuilder Create(WhereClauseBuilderType whereClauseBuilderType)
        {
            if (whereClauseBuilderType == WhereClauseBuilderType.Data)
            {
                return new GenericWhereClauseBuilder(new List<IWhereClauseSection>
                {
                    new WhereClauseSection
                    {
                        FilterConditionSections = new List<IFilterConditionSection>(),
                        OperatorSeparator = "OR",
                        SectionType = FilterConditionSectionType.Default
                    }
                });
            }
            else if (whereClauseBuilderType == WhereClauseBuilderType.GlobalFilters)
            {
                return new GenericWhereClauseBuilder(new List<IWhereClauseSection>
                {
                    new WhereClauseSection
                    {
                        FilterConditionSections = new List<IFilterConditionSection>(),
                        OperatorSeparator = "OR",
                        SectionType = FilterConditionSectionType.LastSelected
                    },
                    new WhereClauseSection
                    {
                        FilterConditionSections = new List<IFilterConditionSection>(),
                        OperatorSeparator = "OR",
                        SectionType = FilterConditionSectionType.Default
                    }
                });
            }
            else
            {
                throw new NoSuchWhereClauseBuilderException(
                    $"[{nameof(WhereClauseBuilderFactory)}][{nameof(Create)}]({nameof(whereClauseBuilderType)})",
                    $"{whereClauseBuilderType}"
                );
            }
        }
    }
}