using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Main MongoDB data provider interface that combines all MongoDB operations.
    /// Provides high-level abstractions for MongoDB document operations that mirror
    /// the existing SQL provider capabilities.
    /// </summary>
    public interface IMongoDataProvider : IMongoExecute, IMongoExpandoObjectDataProvider, IMongoGenericDataProvider
    {
    }
}
