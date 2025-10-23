using System.Collections.Generic;
using AMCode.Common.FilterStructures;

namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// An interface that represents a WHERE clause builder param.
    /// </summary>
    public interface IWhereClauseParam
    {
        /// <summary>
        /// A <see cref="IList{T}"/> of <see cref="IFilter"/> objects that should represent
        /// a list of selected filters.
        /// </summary>
        IList<IFilter> SelectedFilters { get; }

        /// <summary>
        /// The field name of the last selected filter.
        /// </summary>
        string LastSelectedFilter { get; }
    }
}