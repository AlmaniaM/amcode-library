using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMCode.Sql.Where.Internal;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// A class designed to represent a WHERE clause.
    /// </summary>
    public class WhereClauseCommand : IWhereClauseCommand
    {
        /// <summary>
        /// The WHERE command that's prepended to the clause.
        /// </summary>
        public static readonly string CommandName = "WHERE";

        /// <summary>
        /// Create an instance of the <see cref="WhereClauseCommand"/> class.
        /// </summary>
        public WhereClauseCommand()
        {
            whereClauseSections = new List<IWhereClauseSection>();
        }

        /// <summary>
        /// Create an instance of the <see cref="WhereClauseCommand"/> class.
        /// </summary>
        /// <param name="whereClauseSections"></param>
        public WhereClauseCommand(IEnumerable<IWhereClauseSection> whereClauseSections)
        {
            this.whereClauseSections = whereClauseSections.ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetWhereClauseSections()
            => whereClauseSections.Select((section, i) => section.CreateWhereClauseSectionString(i == 0));

        /// <summary>
        /// Creates a WHERE clause.
        /// </summary>
        /// <inheritdoc/>
        public string CreateCommand()
        {
            var commandValue = GetCommandValue();
            if (commandValue.Equals(string.Empty))
            {
                return string.Empty;
            }

            return new StringBuilder()
                .Append(CommandName)
                .Append(" ")
                .Append(commandValue)
                .ToString();
        }

        /// <summary>
        /// A WHERE clause command.
        /// </summary>
        public string CommandType => CommandName;

        /// <summary>
        /// A string of WHERE clause sections.
        /// </summary>
        public string GetCommandValue()
        {
            var anyPopulatedSections = whereClauseSections.Where(section => section.HasAny).Any();
            if (!anyPopulatedSections)
            {
                return string.Empty;
            }

            return whereClauseSections
                .Where(whereClauseSection => whereClauseSection.HasAny)
                .Aggregate(new StringBuilder(),
                    (updatedStringBuilder, whereClauseSection) => updatedStringBuilder.Append(whereClauseSection.CreateWhereClauseSectionString(updatedStringBuilder.Length == 0))
                )
                .ToString();
        }

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                var cleanWhereClauseSections = getValidWhereClauseSections().ToList();

                if (whereClauseSections.Count == 0)
                {
                    return "There are no where clause sections to build a WHERE clause with.";
                }

                if (cleanWhereClauseSections.Count != whereClauseSections.Count)
                {
                    var invalidWhereClauseSections = string.Join(", ", getInValidWhereClauseSections().Select(section => section is null ? "'null'" : $"'{section.InvalidCommandMessage}'"));
                    return $"There are invalid where clause sections. Values are {invalidWhereClauseSections}.";
                }

                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public bool IsValid => getInValidWhereClauseSections().Count() == 0 && whereClauseSections.Count > 0;

        /// <inheritdoc/>
        public void AddWhereClauseSection(IWhereClauseSection section) => whereClauseSections.Add(section);

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();

        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> of valid <see cref="IWhereClauseSection"/>s.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="IWhereClauseSection"/>s.</returns>
        private IEnumerable<IWhereClauseSection> getValidWhereClauseSections()
            => whereClauseSections.Where(section => !(section is null) && section.IsValid);

        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> of invalid <see cref="IWhereClauseSection"/>s.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="IWhereClauseSection"/>s.</returns>
        private IEnumerable<IWhereClauseSection> getInValidWhereClauseSections()
            => whereClauseSections.Where(section => section is null || !section.IsValid);

        /// <summary>
        /// A <see cref="IList{T}"/> of <see cref="IWhereClauseSection"/>s.
        /// </summary>
        private IList<IWhereClauseSection> whereClauseSections { get; set; }
    }
}