using AMCode.Sql.Commands.Models;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.OrderBy.Models
{
    /// <summary>
    /// A delegate that retrieves returns the name of the <see cref="IColumnSortFormatter"/>
    /// to use for the given <see cref="ISortProvider"/> object.
    /// </summary>
    /// <param name="sortProvider">The <see cref="ISortProvider"/> currently being worked on.</param>
    /// <returns>The <see cref="string"/> name of the <see cref="IColumnSortFormatter"/> to fetch.</returns>
    public delegate string GetSortFormatterNameFunction(ISortProvider sortProvider);
}