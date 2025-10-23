using System;

namespace AMCode.Common
{
    /// <summary>
    /// An interface designed for cloning classes.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface ICloneable<out TResult>
    {
        /// <summary>
        /// Clone the current object.
        /// </summary>
        /// <returns>A <typeparamref name="TResult"/> <see cref="Type"/> object.</returns>
        TResult Clone();
    }
}