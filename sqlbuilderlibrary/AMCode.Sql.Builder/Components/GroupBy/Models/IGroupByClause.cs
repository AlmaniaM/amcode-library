using System.Collections.Generic;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.GroupBy
{
    /// <summary>
    /// An interface which that represents a GROUP BY clause.
    /// </summary>
    public interface IGroupByClause
    {
        /// <summary>
        /// Create a <see cref="IGroupByClauseCommand"/> object.
        /// </summary>
        /// <param name="groupables">A <see cref="IEnumerable{T}"/> of <see cref="IGroupable"/>s that you
        /// want to create a <see cref="IGroupByClauseCommand"/> object..</param>
        /// <param name="onlyPrimaryColumn">Whether or not the GROUP BY should only include primary columns.</param>
        /// <returns>A <see cref="IGroupByClauseCommand"/> object..</returns>
        IGroupByClauseCommand CreateClause(IEnumerable<IGroupable> groupables, bool onlyPrimaryColumn);
    }
}