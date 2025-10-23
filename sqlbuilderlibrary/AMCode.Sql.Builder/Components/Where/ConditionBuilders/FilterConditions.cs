using AMCode.Common.Extensions.Strings;
using AMCode.Sql.Helpers;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to build WHERE clause "Filter IN (...values)" conditions.
    /// </summary>
    public class FilterInCondition : IFilterCondition
    {
        private readonly string alias;
        private readonly string filterName;
        private readonly IFilterConditionValueBuilder valueBuilder;

        /// <summary>
        /// Create an instance of the <see cref="FilterInCondition"/> class.
        /// </summary>
        /// <param name="filterName">The <see cref="string"/> filter name to use in the clause.</param>
        /// <param name="valueBuilder">A <see cref="IFilterConditionOrganizer"/> object to build the "(...values)" section.</param>
        /// <param name="alias">An alias to attach to the filter name. Provide null to not attach anything.</param>
        public FilterInCondition(string filterName, IFilterConditionValueBuilder valueBuilder, string alias)
        {
            this.alias = alias ?? string.Empty;
            this.filterName = filterName;
            this.valueBuilder = valueBuilder;
        }

        /// <summary>
        /// Clones the current instance of the <see cref="IFilterCondition"/> object.
        /// </summary>
        /// <returns>A new <see cref="IFilterCondition"/> object.</returns>
        public IFilterCondition Clone() => new FilterInCondition(filterName, valueBuilder, alias);

        /// <summary>
        /// Create a single "Filter IN (...values)" condition for the WHERE clause.
        /// </summary>
        /// <returns>A single <see cref="string"/> WHERE clause condition section.</returns>
        public string CreateFilterCondition()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            var filterInClause = $"{AliasUtils.GetUpdatedAlias(alias)}{filterName} IN ({valueBuilder.CreateFilterConditionValue()})";
            return filterInClause;
        }

        /// <inheritdoc/>
        public bool IsValid
        {
            get
            {
                var value = valueBuilder.CreateFilterConditionValue();
                return !value.IsNullEmptyOrWhiteSpace();
            }
        }

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (!IsValid)
                {
                    return $"No values provided for evaluation. Values are '{valueBuilder.CreateFilterConditionValue()}'.";
                }

                return string.Empty;
            }
        }
    }

    /// <summary>
    /// A class designed to build WHERE clause "Filter IS value" conditions.
    /// </summary>
    public class FilterIsCondition : IFilterCondition
    {
        private readonly string alias;
        private readonly string filterName;
        private readonly FilterConditionValueDelegate getValue;

        /// <summary>
        /// Create an instance of the <see cref="FilterIsCondition"/> class.
        /// </summary>
        /// <param name="filterName">The <see cref="string"/> filter name to use in the clause.</param>
        /// <param name="getValue">The <see cref="FilterConditionValueDelegate"/> for retrieving the value.</param>
        /// <param name="alias">An alias to attach to the filter name. Provide null to not attach anything.</param>
        public FilterIsCondition(string filterName, FilterConditionValueDelegate getValue, string alias)
        {
            this.alias = alias ?? string.Empty;
            this.filterName = filterName;
            this.getValue = getValue;
        }

        /// <summary>
        /// Clones the current instance of the <see cref="IFilterCondition"/> object.
        /// </summary>
        /// <returns>A new <see cref="IFilterCondition"/> object.</returns>
        public IFilterCondition Clone() => new FilterIsCondition(filterName, getValue, alias);

        /// <summary>
        /// Create a single "Filter IS value" condition for the WHERE clause.
        /// </summary>
        /// <returns>A single <see cref="string"/> WHERE clause condition section.</returns>
        public string CreateFilterCondition()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            var filterInClause = $"{AliasUtils.GetUpdatedAlias(alias)}{filterName} IS {getValue()}";
            return filterInClause;
        }

        /// <inheritdoc/>
        public bool IsValid
        {
            get
            {
                var value = getValue();
                return !value.IsNullEmptyOrWhiteSpace();
            }
        }

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (!IsValid)
                {
                    return $"No value provided for evaluation. Value is '{getValue()}'.";
                }

                return string.Empty;
            }
        }
    }

    /// <summary>
    /// A class designed to build WHERE clause "Filter BETWEEN left AND right" conditions.
    /// </summary>
    public class FilterBetweenCondition : IFilterCondition
    {
        private readonly string alias;
        private readonly string filterName;
        private readonly bool greaterOrEqualAsDefault;
        private readonly IFilterConditionValueBuilder valueBuilder;

        /// <summary>
        /// Create an instance of the <see cref="FilterInCondition"/> class.
        /// </summary>
        /// <param name="filterName">The <see cref="string"/> filter name to use in the clause.</param>
        /// <param name="valueBuilder">A <see cref="IFilterConditionOrganizer"/> object to build the "(...values)" section.</param>
        /// <param name="alias">An alias to attach to the filter name. Provide null to not attach anything.</param>
        public FilterBetweenCondition(string filterName, IFilterConditionValueBuilder valueBuilder, string alias)
            : this(filterName, valueBuilder, alias, true) { }

        /// <inheritdoc cref="FilterBetweenCondition"/>
        /// <param name="filterName">The <see cref="string"/> filter name to use in the clause.</param>
        /// <param name="valueBuilder">A <see cref="IFilterConditionOrganizer"/> object to build the "(...values)" section.</param>
        /// <param name="alias">An alias to attach to the filter name. Provide null to not attach anything.</param>
        /// <param name="greaterOrEqualAsDefault">If <c>true</c> then "greater-than-equal" will be used if only one value exists. If <c>false</c>
        /// then "less-than-equal" will be used if only one value exists from the <see cref="IFilterConditionOrganizer.AddFilterCondition(IWhereClauseBuilder)"/>
        /// invocation.</param>
        public FilterBetweenCondition(string filterName, IFilterConditionValueBuilder valueBuilder, string alias, bool greaterOrEqualAsDefault)
        {
            this.alias = alias ?? string.Empty;
            this.filterName = filterName;
            this.valueBuilder = valueBuilder;
            this.greaterOrEqualAsDefault = greaterOrEqualAsDefault;
        }

        /// <summary>
        /// Clones the current instance of the <see cref="IFilterCondition"/> object.
        /// </summary>
        /// <returns>A new <see cref="IFilterCondition"/> object.</returns>
        public IFilterCondition Clone() => new FilterBetweenCondition(filterName, valueBuilder, alias, greaterOrEqualAsDefault);

        /// <summary>
        /// Create a single "Filter BETWEEN left AND right" condition for the WHERE clause.
        /// </summary>
        /// <returns>A single <see cref="string"/> WHERE clause condition section.</returns>
        public string CreateFilterCondition()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            if (valueBuilder.Count == 1)
            {
                var comparitor = greaterOrEqualAsDefault ? ">=" : "<=";
                return $"{AliasUtils.GetUpdatedAlias(alias)}{filterName} {comparitor} {valueBuilder.CreateFilterConditionValue()}";
            }

            return $"{AliasUtils.GetUpdatedAlias(alias)}{filterName} BETWEEN {valueBuilder.CreateFilterConditionValue()}";
        }

        /// <inheritdoc/>
        public bool IsValid => !valueBuilder.CreateFilterConditionValue().IsNullEmptyOrWhiteSpace();

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (!IsValid)
                {
                    return $"No values provided for evaluation. Values are '{valueBuilder.CreateFilterConditionValue()}'.";
                }

                return string.Empty;
            }
        }
    }
}