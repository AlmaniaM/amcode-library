using System.Text;
using AMCode.Common.Extensions.Strings;

namespace AMCode.Sql.From
{
    /// <summary>
    /// A class designed to build the table name for a FROM clause.
    /// </summary>
    public class TableClauseCommand : ITableClauseCommand
    {
        /// <inheritdoc/>
        public string Alias { get; set; }

        /// <inheritdoc/>
        public string TableName { get; set; }

        /// <inheritdoc/>
        public string CommandType => "TABLE";

        /// <inheritdoc/>
        public bool IsValid => !TableName.IsNullEmptyOrWhiteSpace() && TableName.Split(' ').Length == 1;

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (IsValid)
                {
                    return string.Empty;
                }

                var tableNameValue = TableName is null ? "null" : TableName;
                return $"The value \"'{tableNameValue}'\" is not valid for a table name.";
            }
        }

        /// <inheritdoc/>
        public string Schema { get; set; }

        /// <inheritdoc/>
        public string CreateCommand()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            return GetCommandValue();
        }

        /// <inheritdoc/>
        public string GetCommandValue()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            var alias = Alias.IsNullEmptyOrWhiteSpace() ? "" : $" AS {Alias}";
            var schema = createSchema(Schema);

            return new StringBuilder()
                .Append(schema)
                .Append(TableName)
                .Append(alias)
                .ToString();
        }

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();

        /// <summary>
        /// Check the provided schema to make sure it can be used and return a valid prepend-ready schema.
        /// </summary>
        /// <param name="schema">The <see cref="string"/> schema to validate.</param>
        /// <returns>A <see cref="string"/> schema name with a period (.) appended to it.</returns>
        private string createSchema(string schema)
        {
            if (schema.IsNullEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            if (schema[schema.Length - 1] == '.')
            {
                return schema;
            }

            return $"{schema}.";
        }
    }
}