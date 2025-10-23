using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// An interface representing a factory for creating different behavior types of <see cref="IWhereClause"/> objects.
    /// </summary>
    public interface IWhereClauseTypeFactory
    {
        /// <summary>
        /// Creates an instance of an <see cref="IWhereClause"/>.
        /// </summary>
        /// <param name="whereClauseBuilderType">Provide a <see cref="WhereClauseBuilderType"/> of the type of
        /// <see cref="IWhereClause"/> you want to construct.</param>
        /// <returns>An object instance that implements <see cref="IWhereClause"/>.</returns>
        IWhereClause Create(WhereClauseBuilderType whereClauseBuilderType);
    }
}