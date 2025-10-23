using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.FilterConditionSectionTests
{
    [TestFixture]
    public class FilterConditionSectionTest
    {
        [Test]
        public void ShouldCreateASingleFilterInSection()
        {
            var filterInSection = new FilterConditionSection
            {
                FilterCondition = createFilterCondition("Column1 IN ('1', '2', '3')", true).Object,
                OperatorSeparator = "AND"
            };

            Assert.AreEqual("Column1 IN ('1', '2', '3')", filterInSection.CreateFilterClauseString(true));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldCreateTwoFilterInSectionsWithParenthesesAroundIt(bool isFirstInLine)
        {
            var filterInSection = new FilterConditionSection
            {
                FilterCondition = createFilterCondition("Column1 IN ('1', '2', '3')", true).Object,
                FilterSections = new List<IFilterConditionSection> {
                    new FilterConditionSection
                    {
                        FilterCondition = createFilterCondition("Column2 IS NULL", true).Object,
                        OperatorSeparator = "OR"
                    }
                },
                OperatorSeparator = "AND"
            };

            if (isFirstInLine)
            {
                Assert.AreEqual("(Column1 IN ('1', '2', '3') OR Column2 IS NULL)", filterInSection.CreateFilterClauseString(isFirstInLine));
            }
            else
            {
                Assert.AreEqual("AND (Column1 IN ('1', '2', '3') OR Column2 IS NULL)", filterInSection.CreateFilterClauseString(isFirstInLine));
            }
        }

        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(10, true)]
        [TestCase(10, false)]
        public void ShouldCreateNFilterInSectionsWithParenthesesAroundIt(int filterCount, bool isFirstInLine)
        {
            var filterInSection = new FilterConditionSection
            {
                FilterCondition = createFilterCondition("Column1 IN ('1', '2', '3')", true).Object,
                FilterSections = Enumerable.Range(2, filterCount).Select(i => new FilterConditionSection
                {
                    FilterCondition = createFilterCondition($"Column{i} IN ('1', '2', '3')", true).Object,
                    OperatorSeparator = "OR"
                }).Cast<IFilterConditionSection>().ToList(),
                OperatorSeparator = "AND"
            };

            var filterInConditionList = Enumerable.Range(1, filterCount + 1).Select(index => $"Column{index} IN ('1', '2', '3')").ToList();

            var filterInSectionString = string.Join(" OR ", filterInConditionList);

            if (isFirstInLine)
            {
                Assert.AreEqual($"({filterInSectionString})", filterInSection.CreateFilterClauseString(isFirstInLine));
            }
            else
            {
                Assert.AreEqual($"AND ({filterInSectionString})", filterInSection.CreateFilterClauseString(isFirstInLine));
            }
        }

        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(10, true)]
        [TestCase(10, false)]
        public void ShouldCreateNFilterInSectionsWithParenthesesAroundItAndChildren(int filterCount, bool isFirstInLine)
        {
            var filterInSection = new FilterConditionSection
            {
                FilterCondition = createFilterCondition("Column1 IN ('1', '2', '3')", true).Object,
                FilterSections = Enumerable.Range(2, filterCount).Select(i => new FilterConditionSection
                {
                    FilterCondition = createFilterCondition($"Column{i} IN ('1', '2', '3')", true).Object,
                    FilterSections = new List<IFilterConditionSection>
                        {
                            new FilterConditionSection
                            {
                                FilterCondition = createFilterCondition("Column1 IN ('1', '2', '3')", true).Object,
                                OperatorSeparator = "AND"
                            }
                        },
                    OperatorSeparator = "OR"
                }).Cast<IFilterConditionSection>().ToList(),
                OperatorSeparator = "AND"
            };

            var filterInConditionList = Enumerable.Range(2, filterCount).Select(index => $"(Column{index} IN ('1', '2', '3') AND Column1 IN ('1', '2', '3'))").ToList();

            var filterInSectionString = string.Join(" OR ", filterInConditionList);

            if (isFirstInLine)
            {
                var actual = filterInSection.CreateFilterClauseString(isFirstInLine);
                var expected = $"(Column1 IN ('1', '2', '3') OR {filterInSectionString})";
                Assert.AreEqual(expected, actual);
            }
            else
            {
                var actual = filterInSection.CreateFilterClauseString(isFirstInLine);
                var expected = $"AND (Column1 IN ('1', '2', '3') OR {filterInSectionString})";
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ShouldBeAnInvalidCommand()
        {
            var filterConditionSection = new FilterConditionSection();
            Assert.IsFalse(filterConditionSection.IsValid);
        }

        [Test]
        public void ShouldBeAnInvalidCommandMessageAboutANullFilterCondition()
        {
            var filterConditionSection = new FilterConditionSection();
            Assert.AreEqual($"The main {nameof(FilterConditionSection)} cannot be null.", filterConditionSection.InvalidCommandMessage);
        }

        [Test]
        public void ShouldBeAnInvalidCommandWhenInvalidFilterCondition()
        {
            var filterConditionSection = new FilterConditionSection();
            var filterConditionMoq = createFilterCondition(string.Empty, false);
            filterConditionSection.FilterCondition = filterConditionMoq.Object;

            Assert.IsFalse(filterConditionSection.IsValid);
        }

        [Test]
        public void ShouldBeAnInvalidCommandMessageAboutInvalidFilterCondition()
        {
            var filterConditionSection = new FilterConditionSection();
            var filterConditionMoq = createFilterCondition(string.Empty, false, "Error message");
            filterConditionSection.FilterCondition = filterConditionMoq.Object;

            Assert.AreEqual($"The main {nameof(FilterConditionSection)} is not valid. Message 'Error message'.", filterConditionSection.InvalidCommandMessage);
        }

        [Test]
        public void ShouldBeAnInvalidCommandWhenInvalidFilterConditionSections()
        {
            var filterConditionSection = new FilterConditionSection();
            var filterConditionMoq = createFilterCondition(string.Empty, true);
            filterConditionSection.FilterCondition = filterConditionMoq.Object;
            filterConditionSection.FilterSections = new List<IFilterConditionSection>
            {
                createFilterConditionSection(createFilterCondition(string.Empty, false, "Invalid condition 1").Object, false, "Invalid condition section 1").Object
            };

            Assert.IsFalse(filterConditionSection.IsValid);
        }

        [Test]
        public void ShouldBeAnInvalidCommandMessageAboutInvalidFilterConditionSections()
        {
            var filterConditionSection = new FilterConditionSection();
            var filterConditionMoq = createFilterCondition(string.Empty, true);
            filterConditionSection.FilterCondition = filterConditionMoq.Object;
            filterConditionSection.FilterSections = new List<IFilterConditionSection>
            {
                createFilterConditionSection(createFilterCondition(string.Empty, false, string.Empty).Object, false, "Invalid condition section 1").Object,
                createFilterConditionSection(createFilterCondition(string.Empty, false, string.Empty).Object, false, "Invalid condition section 2").Object
            };

            Assert.AreEqual($"Invalid filter IN sections. Values are 'Invalid condition section 1', 'Invalid condition section 2'.", filterConditionSection.InvalidCommandMessage);
        }

        [Test]
        public void ShouldBeAnInvalidCommandMessageAboutInvalidFilterConditionSectionsWithNulls()
        {
            var filterConditionSection = new FilterConditionSection();
            var filterConditionMoq = createFilterCondition(string.Empty, true);
            filterConditionSection.FilterCondition = filterConditionMoq.Object;
            filterConditionSection.FilterSections = new List<IFilterConditionSection>
            {
                createFilterConditionSection(createFilterCondition(string.Empty, false, string.Empty).Object, false, "Invalid condition section 1").Object,
                null
            };

            Assert.AreEqual($"Invalid filter IN sections. Values are 'Invalid condition section 1', 'null'.", filterConditionSection.InvalidCommandMessage);
        }

        private Mock<IFilterConditionSection> createFilterConditionSection(IFilterCondition filterCondition, bool isValid, string invalidMessage = "")
        {
            var conditionSectionMoq = new Mock<IFilterConditionSection>();
            conditionSectionMoq.Setup(moq => moq.IsValid).Returns(() => isValid);
            conditionSectionMoq.Setup(moq => moq.InvalidCommandMessage).Returns(() => invalidMessage);
            conditionSectionMoq.Setup(moq => moq.FilterCondition).Returns(() => filterCondition);
            return conditionSectionMoq;
        }

        private Mock<IFilterCondition> createFilterCondition(string value, bool isValid, string invalidMessage = "")
        {
            var conditionMoq = new Mock<IFilterCondition>();
            conditionMoq.Setup(moq => moq.IsValid).Returns(() => isValid);
            conditionMoq.Setup(moq => moq.InvalidCommandMessage).Returns(() => invalidMessage);
            conditionMoq.Setup(moq => moq.CreateFilterCondition()).Returns(() => value);
            return conditionMoq;
        }
    }
}