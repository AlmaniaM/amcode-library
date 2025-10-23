using System;

namespace AMCode.Documents.Common.Enums
{
    /// <summary>
    /// Border sides flags
    /// </summary>
    [Flags]
    public enum BorderSides
    {
        /// <summary>
        /// No borders
        /// </summary>
        None = 0,

        /// <summary>
        /// Top border
        /// </summary>
        Top = 1,

        /// <summary>
        /// Bottom border
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// Left border
        /// </summary>
        Left = 4,

        /// <summary>
        /// Right border
        /// </summary>
        Right = 8,

        /// <summary>
        /// All borders
        /// </summary>
        All = Top | Bottom | Left | Right
    }
}
