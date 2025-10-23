using System;
using System.Collections.Generic;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;
using AMCode.Sql.Where;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// An interface designed for creating <see cref="ISelectCommand"/> objects.
    /// </summary>
    public interface ISelectCommandBuilder
    {
        /// <summary>
        /// Create an instance of a <see cref="ISelectCommand"/> object.
        /// </summary>
        /// <returns>A <see cref="ISelectCommand"/> object.</returns>
        ISelectCommand Build();

        /// <summary>
        /// Marks this command to be closed off with a semicolon (;).
        /// </summary>
        /// <param name="end"><c>True</c> if a semicolon should be added and <c>false</c> if not.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder End(bool end);

        /// <summary>
        /// Add a <see cref="IGroupByClauseCommand"/> object.
        /// </summary>
        /// <param name="groupBy">The <see cref="IGroupByClauseCommand"/> object to add.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder GroupBy(IGroupByClauseCommand groupBy);

        /// <summary>
        /// Set the indent level for this SELECT command. Default is zero.
        /// </summary>
        /// <param name="indentLevel">The level of indentation.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder Indent(int indentLevel);

        /// <summary>
        /// Add a limit to the amount of records to select.
        /// </summary>
        /// <param name="limit">A <see cref="Nullable"/> <see cref="int"/> limit to the amount of records to select.
        /// If null, then no limit will be added.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder Limit(int? limit);

        /// <summary>
        /// Add the number of records to offset when selecting.
        /// </summary>
        /// <param name="offset">The number of records to offset when selecting. If null
        /// then no offset will be added.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder Offset(int? offset);

        /// <summary>
        /// Add a <see cref="IOrderByClauseCommand"/> object.
        /// </summary>
        /// <param name="orderBy">The ORDER BY clause or comma separated columns.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder OrderBy(IOrderByClauseCommand orderBy);

        /// <summary>
        /// Add an <see cref="ISelectClauseCommand"/> with a collection of query expressions (column names/calculations).
        /// </summary>
        /// <param name="queryExpressions">A collection of query expressions (column names/calculations) to use as the
        /// selection statements.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder Select(IEnumerable<string> queryExpressions);

        /// <summary>
        /// Add a <see cref="ISelectClauseCommand"/> object.
        /// </summary>
        /// <param name="select"></param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder Select(ISelectClauseCommand select);

        /// <summary>
        /// Add a source [SCHEMA].TableName for the command.
        /// </summary>
        /// <param name="schema">The schema where the table lives.</param>
        /// <param name="table">The source TableName.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder From(string schema, string table);

        /// <summary>
        /// Add a source [SCHEMA].TableName for the command.
        /// </summary>
        /// <param name="schema">The schema where the table lives.</param>
        /// <param name="table">The source TableName.</param>
        /// <param name="alias">The alias to give the table.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder From(string schema, string table, string alias);

        /// <summary>
        /// Add a source [SCHEMA].TableName for the command.
        /// </summary>
        /// <param name="selectCommand">An <see cref="ISelectCommand"/> to act as the FROM clause.</param>
        /// <param name="subQueryAlias">The alias to give the subquery.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder From(ISelectCommand selectCommand, string subQueryAlias);

        /// <summary>
        /// Add a <see cref="IWhereClauseCommand"/> object.
        /// </summary>
        /// <param name="where">The <see cref="IWhereClauseCommand"/> object to add.</param>
        /// <returns>An updated <see cref="ISelectCommandBuilder"/> object.</returns>
        ISelectCommandBuilder Where(IWhereClauseCommand where);
    }
}