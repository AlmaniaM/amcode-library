using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.Select
{
    /// <summary>
    /// A delegate for retrieving the name of a <see cref="IDataQueryExpression"/> based on the current
    /// <see cref="IGetQueryExpression"/> object.
    /// </summary>
    /// <param name="getQueryExpression">The <see cref="IGetQueryExpression"/> object currently being worked on.</param>
    /// <returns>The <see cref="string"/> name of the <see cref="IDataQueryExpression"/> to fetch.</returns>
    public delegate string GetQueryExpressionNameFunction(IGetQueryExpression getQueryExpression);
}