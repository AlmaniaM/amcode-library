using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// An interface meant for constructing a <see cref="IWhereClauseBuilder"/>.
    /// </summary>
    public interface IWhereClauseBuilderFactory
    {
        /// <summary>
        /// Create a <see cref="IWhereClauseBuilder"/> based on the provided <see cref="WhereClauseBuilderType"/>.
        /// </summary>
        /// <param name="whereClauseBuilderType">The <see cref="WhereClauseBuilderType"/> to build.</param>
        /// <returns>An instance of an object implementing a <see cref="IWhereClauseBuilder"/>.</returns>
        IWhereClauseBuilder Create(WhereClauseBuilderType whereClauseBuilderType);
    }
}