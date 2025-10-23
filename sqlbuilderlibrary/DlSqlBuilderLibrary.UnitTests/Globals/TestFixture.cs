using System.Collections.Generic;
using System.IO;
using AMCode.Common.FilterStructures;
using AMCode.Common.IO.JSON;
using DlSqlBuilderLibrary.UnitTests.Globals.Builders;

namespace DlSqlBuilderLibrary.UnitTests.Globals
{
    public abstract class TestFixtureBase
    {
        public abstract TestFixture FromFile(string filePath);
    }

    public class TestFixture : TestFixtureBase
    {
        protected IList<IFilter> filters = new List<IFilter>();

        public override TestFixture FromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Cannot find file at {filePath}");
            }

            if (!Path.GetExtension(filePath).ToLower().Equals(".json"))
            {
                throw new($"File at {filePath} does not have a valid JSON file extension. Please provide a JSON file to build a list of Filters from.");
            }

            filters = (IList<IFilter>)JsonFileReader.Read<List<Filter>>(filePath);
            return this;
        }

        /// <summary>
        /// Build a <see cref="IList{T}"/> of <see cref="IFilter"/> objects.
        /// </summary>
        public TestFixtureFiltersBuilder Filters => new(filters);

        /// <summary>
        /// Build the current <see cref="IList{T}"/> of <see cref="IFilter"/>.
        /// </summary>
        /// <returns>A <see cref="IList{T}"/> of <see cref="IFilter"/> instance that you have built.</returns>
        public IList<IFilter> Build() => filters;
    }
}