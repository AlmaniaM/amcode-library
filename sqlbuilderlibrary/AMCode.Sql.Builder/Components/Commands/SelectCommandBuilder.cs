using System.Collections.Generic;
using AMCode.Sql.From;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;
using AMCode.Sql.Where;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// A class designed for building a <see cref="ISelectCommand"/> object.
    /// </summary>
    public class SelectCommandBuilder : ISelectCommandBuilder
    {
        private readonly ISelectCommand selectCommand;

        /// <summary>
        /// Create an instance of the <see cref="SelectCommandBuilder"/> class.
        /// </summary>
        public SelectCommandBuilder()
        {
            selectCommand = new SelectCommand();
        }

        /// <summary>
        /// Create an instance of the <see cref="SelectCommandBuilder"/> class.
        /// </summary>
        /// <param name="selectCommand">A <see cref="ISelectCommand"/> to use for storing configurations.</param>
        public SelectCommandBuilder(ISelectCommand selectCommand)
        {
            this.selectCommand = selectCommand;
        }

        /// <inheritdoc/>
        public ISelectCommand Build() => new SelectCommand(selectCommand);

        /// <inheritdoc/>
        public ISelectCommandBuilder End(bool end)
        {
            selectCommand.EndCommand = end;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder GroupBy(IGroupByClauseCommand groupBy)
        {
            selectCommand.GroupBy = groupBy;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder Indent(int indentLevel)
        {
            selectCommand.IndentLevel = indentLevel;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder Limit(int? limit)
        {
            selectCommand.Limit = limit;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder Offset(int? offset)
        {
            selectCommand.Offset = offset;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder OrderBy(IOrderByClauseCommand orderBy)
        {
            selectCommand.OrderBy = orderBy;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder Select(ISelectClauseCommand select)
        {
            selectCommand.Select = select;
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder Select(IEnumerable<string> queryExpressions)
        {
            selectCommand.Select = new SelectClauseCommand(queryExpressions);
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder From(string schema, string table)
        {
            selectCommand.From = new FromClauseCommand(schema, table);
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder From(string schema, string table, string alias)
        {
            selectCommand.From = new FromClauseCommand(schema, table, alias);
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder From(ISelectCommand selectCommand, string subQueryAlias)
        {
            this.selectCommand.From = new FromClauseCommand(selectCommand, subQueryAlias);
            return this;
        }

        /// <inheritdoc/>
        public ISelectCommandBuilder Where(IWhereClauseCommand where)
        {
            selectCommand.Where = where;
            return this;
        }
    }
}