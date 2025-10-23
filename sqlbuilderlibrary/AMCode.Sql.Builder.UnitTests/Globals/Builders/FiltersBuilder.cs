using System;
using System.Collections.Generic;
using AMCode.Common.FilterStructures;

namespace AMCode.Sql.Builder.UnitTests.Globals.Builders
{
    public class TestFixtureFiltersBuilder : TestFixture
    {
        public TestFixtureFiltersBuilder(IList<IFilter> filters)
        {
            this.filters = filters;
        }

        /// <summary>
        /// Creates an instance of <see cref="TestFixtureFilterBuilder"/> to build a <see cref="Filter"/>. You must call
        /// <see cref="TestFixtureFilterBuilder.Save"/> when you are done in order to apply any changes you have
        /// made to the <see cref="Filter"/> and add it to the <see cref="Filter"/> <see cref="IList{T}"/>.
        /// </summary>
        public TestFixtureFilterBuilder AddFilterWith => new(this);

        public class TestFixtureFilterBuilder
        {
            protected TestFixtureFiltersBuilder testFixtureFilters;
            protected Filter filter = new();
            private readonly List<FilterItem> filterItems = new();

            public TestFixtureFilterBuilder(TestFixtureFiltersBuilder testFixtureFilters)
            {
                this.testFixtureFilters = testFixtureFilters;
            }

            /// <summary>
            /// Adds a name to this filter.
            /// </summary>
            /// <param name="fieldName">The back-end name of the column for this filter.</param>
            /// <param name="displayName">The front-end name of this filter.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFilterBuilder FilterName(string fieldName, string displayName)
            {
                filter.FilterName = new FilterName { FieldName = fieldName, DisplayName = displayName };
                return this;
            }

            /// <summary>
            /// Adds an ID name to this filter.
            /// </summary>
            /// <param name="fieldName">The back-end name of the column for this filter.</param>
            /// <param name="displayName">The front-end name of this filter.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFilterBuilder FilterIdName(string fieldName, string displayName)
            {
                filter.FilterIdName = new FilterName { FieldName = fieldName, DisplayName = displayName };
                return this;
            }

            /// <summary>
            /// Add a single <see cref="FilterItem"/> to the <see cref="Filter"/>.
            /// </summary>
            /// <param name="value">The filter value.</param>
            /// <param name="idValue">The filter ID value.</param>
            /// <param name="selected">Provide <see cref="true"/> if the filter is selected and <see cref="false"/> if not.</param>
            /// <param name="disabled">Provide false if the filter is disabled and true if not.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFilterBuilder FilterItem(string value, string idValue, bool? selected, bool disabled)
            {
                filterItems.Add(new FilterItem
                {
                    FilterVal = value,
                    FilterId = idValue,
                    Selected = (bool)selected,
                    Disabled = disabled
                });
                return this;
            }

            /// <summary>
            /// Adds a list of filters to the current <see cref="Filter"/>.
            /// </summary>
            /// <param name="filterItemsGenerator">Provide a <see cref="Func{TResult}"/> that takes not arguments and
            /// returns a <see cref="List{T}"/> of <see cref="FilterItem"/>'s.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFilterBuilder FilterItemList(Func<List<FilterItem>> filterItemsGenerator)
            {
                filterItems.AddRange(filterItemsGenerator());
                return this;
            }

            /// <summary>
            /// Set the current <see cref="Filter"/> as required or not.
            /// </summary>
            /// <param name="isRequired">Provide <see cref="true"/> if required and <see cref="false"/> if not.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFilterBuilder SetRequired(bool isRequired)
            {
                filter.Required = isRequired;
                return this;
            }

            /// <summary>
            /// Save the current <see cref="Filter"/> that you have built.
            /// </summary>
            /// <returns>A <see cref="TestFixtureFiltersBuilder"/> to build another <see cref="Filter"/> or
            /// move onto other things.</returns>
            public TestFixtureFiltersBuilder Save()
            {
                filter.FilterItems = filterItems.ToArray();
                testFixtureFilters.filters.Add(filter);
                return testFixtureFilters;
            }
        }

        public class TestFixtureFiscalWkBuilder
        {
            private readonly TestFixtureFiltersBuilder testFixtureFilters;
            private readonly Filter dlFilter = new()
            {
                FilterName = new FilterName
                {
                    FieldName = "fiscalWk",
                    DisplayName = "Fiscal Wk"
                },
                Required = false
            };

            private List<FilterItem> filterItems = new();

            public TestFixtureFiscalWkBuilder(TestFixtureFiltersBuilder testFixtureFilters)
            {
                this.testFixtureFilters = testFixtureFilters;
            }

            /// <summary>
            /// Add a range of weeks <see cref="dl_history.Components.GlobalFilters.Models.FilterItem"/>'s.
            /// </summary>
            /// <param name="start">An <see cref="int"/> week to start from. Index is inclusive.</param>
            /// <param name="end">An <see cref="int"/> week to end with. Index is inclusive.</param>
            /// <param name="selected">If <see cref="true"/> then all weeks will be selected. Default is <see cref="false"/>.</param>
            /// <param name="disabled">If true then all filters will be marked as disabled. Default is false.</param>
            /// <returns>A <see cref="TestFixtureFiscalWkBuilder"/> in order to keep building the current FiscalWk <see cref="Filter"/>.</returns>
            public TestFixtureFiscalWkBuilder Range(int start = 1, int end = 52, bool selected = false, bool disabled = false)
            {
                var filterItemsList = new List<FilterItem>();

                for (var i = start; i <= end; i++)
                {
                    filterItemsList.Add(new FilterItem
                    {
                        FilterVal = $"{i}",
                        Selected = selected,
                        Disabled = disabled
                    });
                }

                filterItems = filterItemsList;
                return this;
            }

            /// <summary>
            /// Add a single <see cref="AMCode.Common.FilterStructures.FilterItem"/> to the FiscalWk <see cref="Filter"/>.
            /// </summary>
            /// <param name="value">The filter value.</param>
            /// <param name="idValue">The filter ID value.</param>
            /// <param name="selected">Provide <see cref="true"/> if the filter is selected and <see cref="false"/> if not.</param>
            /// <param name="disabled">Provide false if the filter is not disabled and true if disabled.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFiscalWkBuilder FilterItem(string value, string idValue, bool selected, bool disabled)
            {
                filterItems.Add(new FilterItem
                {
                    FilterVal = value,
                    FilterId = idValue,
                    Selected = selected,
                    Disabled = disabled
                });
                return this;
            }

            /// <summary>
            /// Adds a list of weeks to the FiscalWk <see cref="Filter"/>.
            /// </summary>
            /// <param name="filterItemsGenerator">Provide a <see cref="Func{TResult}"/> that takes not arguments and
            /// returns a <see cref="List{T}"/> of <see cref="FilterItem"/>'s.</param>
            /// <returns>A <see cref="TestFixtureFilterBuilder"/> to keep building the current <see cref="Filter"/>.</returns>
            public TestFixtureFiscalWkBuilder FilterItemList(Func<List<FilterItem>> filterItemsGenerator)
            {
                filterItems.AddRange(filterItemsGenerator());
                return this;
            }

            /// <summary>
            /// Save the current FiscalWk <see cref="Filter"/> that you have built.
            /// </summary>
            /// <returns>A <see cref="TestFixtureGlobalFilterModelBuilder"/> to build another <see cref="Filter"/> or
            /// move onto other things.</returns>
            public TestFixtureFiltersBuilder Save()
            {
                dlFilter.FilterItems = filterItems.ToArray();
                return testFixtureFilters;
            }
        }
    }
}