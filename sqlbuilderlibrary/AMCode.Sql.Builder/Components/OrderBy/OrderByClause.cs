using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.OrderBy.Exceptions;
using AMCode.Sql.OrderBy.Models;

namespace AMCode.Sql.OrderBy
{
    /// <summary>
    /// A class designed to create a ORDER BY clause.
    /// </summary>
    public class OrderByClause : IOrderByClause
    {
        /// <inheritdoc/>
        public IOrderByClauseCommand CreateClause(IEnumerable<ISortProvider> sortProviders)
        {
            if (sortProviders is null)
            {
                return default;
            }

            return new OrderByClauseCommand(getValidSortProviders(sortProviders)
                .Select(sortProvider => sortProvider.GetSort()));
        }

        /// <inheritdoc/>
        /// <exception cref="NoGetSortFormatterNameFunctionProvidedException"></exception>
        public IOrderByClauseCommand CreateClause(IEnumerable<ISortProvider> sortProviders, GetSortFormatterNameFunction getSortFormatterName)
        {
            if (sortProviders is null)
            {
                return default;
            }

            if (getSortFormatterName is null)
            {
                throw new NoGetSortFormatterNameFunctionProvidedException(
                    $"[{nameof(OrderByClause)}][{nameof(CreateClause)}]({nameof(sortProviders)}, {nameof(getSortFormatterName)})",
                    string.Empty
                );
            }

            return new OrderByClauseCommand(getValidSortProviders(sortProviders)
                .Select(sortProvider => sortProvider.GetSort(getSortFormatterName(sortProvider))));
        }

        /// <summary>
        /// Get a valid <see cref="IEnumerable{T}"/> of <see cref="ISortProvider"/>s.
        /// </summary>
        /// <param name="sortProviders">The <see cref="ISortProvider"/>s to filter through.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ISortProvider"/> objects.</returns>
        private IEnumerable<ISortProvider> getValidSortProviders(IEnumerable<ISortProvider> sortProviders)
            => sortProviders
                .Where(sortProvider => sortProvider.SortIndex != null && sortProvider.IsVisible)
                .OrderBy(sortProvider => sortProvider.SortIndex);
    }
}