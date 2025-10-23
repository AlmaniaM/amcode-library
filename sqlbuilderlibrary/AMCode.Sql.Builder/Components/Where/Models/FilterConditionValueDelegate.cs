namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// A delegate to build a value for a WHERE clause condition section.
    /// </summary>
    /// <returns>A <see cref="string"/> value for a WHERE clause condition.</returns>
    public delegate string FilterConditionValueDelegate();
}