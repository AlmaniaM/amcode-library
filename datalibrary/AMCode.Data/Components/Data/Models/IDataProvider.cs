using System.Dynamic;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents an object that can execute and create <see cref="ExpandoObject"/>s
    /// as well as statically defined objects.
    /// </summary>
    public interface IDataProvider : IDbExecute, IExpandoObjectDataProvider, IGenericDataProvider { }
}