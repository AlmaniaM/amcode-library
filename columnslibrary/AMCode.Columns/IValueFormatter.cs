using System;

namespace AMCode.Columns.Core
{
    /// <summary>
    /// Mock interface for value formatting
    /// </summary>
    public interface IColumnValueFormatter<TInput, TOutput>
    {
        TOutput FormatToObject(TInput input);
    }
}
