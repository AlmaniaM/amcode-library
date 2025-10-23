using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Components.Where.Comparers;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A factory class designed to return an <see cref="IComparer{T}"/> of type <see cref="IFilterItem"/>.
    /// </summary>
    public class ComparerFactory : IComparerFactory
    {
        private readonly string dateFormat;

        /// <summary>
        /// Create an instance of the <see cref="ComparerFactory"/> class.
        /// </summary>
        /// <param name="dateFormat">A string date format for <see cref="DateTime"/> comparers.</param>
        public ComparerFactory(string dateFormat)
        {
            this.dateFormat = dateFormat;
        }

        /// <inheritdoc/>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The interface requires it but we don't yet have a use for it.")]
        public IComparer<IFilterItem> Create(IFilter filter, bool isIdFilter) => new CompareDateTimeFilterItem(dateFormat, isIdFilter);
    }
}