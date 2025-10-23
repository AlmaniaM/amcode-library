using System.Collections.Generic;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.Where.Internal;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// An interface representing a WHERE clause command.
    /// </summary>
    public interface IWhereClauseCommand : IClauseCommand
    {
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> of <see cref="string"/> where clause sections.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/>s.</returns>
        IEnumerable<string> GetWhereClauseSections();

        /// <summary>
        /// Add an <see cref="IWhereClauseSection"/> object.
        /// </summary>
        /// <param name="section">The <see cref="IWhereClauseSection"/> that you want to add.</param>
        void AddWhereClauseSection(IWhereClauseSection section);
    }
}