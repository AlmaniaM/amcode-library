namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// A simple class designed to hold static filter value types.
    /// </summary>
    public class FilterItemValueTypes
    {
        /// <summary>
        /// The default value for a filter that represents the "No filter available" value.
        /// </summary>
        public static string None { get; private set; } = "-";
    }
}