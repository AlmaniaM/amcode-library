namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Specifies the date system used for date serial numbers
    /// </summary>
    public enum DateSystem
    {
        /// <summary>
        /// 1900 date system (Windows default) - January 1, 1900 is day 1
        /// </summary>
        System1900 = 0,

        /// <summary>
        /// 1904 date system (Mac default) - January 1, 1904 is day 0
        /// </summary>
        System1904 = 1
    }
}
