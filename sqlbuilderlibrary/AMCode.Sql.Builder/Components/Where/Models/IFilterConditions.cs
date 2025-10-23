using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface representing a single filter condition or WHERE clause section
    /// </summary>
    public interface IFilterCondition : IValidCommand
    {
        /// <summary>
        /// Clones the current instance of the <see cref="IFilterCondition"/> object.
        /// </summary>
        /// <returns>A new <see cref="IFilterCondition"/> object.</returns>
        IFilterCondition Clone();

        /// <summary>
        /// Create a single condition for the WHERE clause.
        /// </summary>
        /// <returns>A single <see cref="string"/> WHERE clause condition section.</returns>
        string CreateFilterCondition();
    }
}