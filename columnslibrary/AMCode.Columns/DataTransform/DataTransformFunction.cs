using System;

namespace AMCode.Columns.DataTransform
{
    /// <summary>
    /// Represents a data transformation function delegate.
    /// </summary>
    /// <typeparam name="T">The type of data to transform.</typeparam>
    /// <param name="value">The value to transform.</param>
    /// <returns>The transformed value.</returns>
    public delegate T DataTransformFunction<T>(T value);
}
