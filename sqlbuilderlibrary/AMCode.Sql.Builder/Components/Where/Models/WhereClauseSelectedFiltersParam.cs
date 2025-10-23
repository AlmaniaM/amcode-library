using System.Collections.Generic;
using AMCode.Common.FilterStructures;

namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// A class designed to contain information regarding a selected filters instance.
    /// </summary>
    public class WhereClauseSelectedFiltersParam : IWhereClauseParam
    {
        /// <summary>
        /// A <see cref="IList{T}"/> of <see cref="IFilter"/> objects that should represent
        /// a list of selected filters.
        /// </summary>
        public IList<IFilter> SelectedFilters { get; set; }

        /// <summary>
        /// The field name of the last selected filter.
        /// </summary>
        public string LastSelectedFilter { get; set; }
    }
}