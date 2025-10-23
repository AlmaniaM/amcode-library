namespace AMCode.Exports.Common
{
    /// <summary>
    /// A class designed for checking parameter values.
    /// </summary>
    public class ParameterCheck
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// A class designed for checking <see cref="int"/> parameter values.
    /// </summary>
    public class IntParameterCheck : ParameterCheck
    {
        /// <summary>
        /// The parameter value.
        /// </summary>
        public int Value { get; set; }
    }
}