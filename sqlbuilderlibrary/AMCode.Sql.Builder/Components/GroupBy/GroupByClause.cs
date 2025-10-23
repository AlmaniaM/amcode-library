using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.GroupBy
{
    /// <summary>
    /// A class designed for creating a SQL GROUP By clause.
    /// </summary>
    public class GroupByClause : IGroupByClause
    {
        /// <inheritdoc/>
        public IGroupByClauseCommand CreateClause(IEnumerable<IGroupable> groupables, bool onlyPrimaryColumn)
        {
            if (groupables.Count() == 0)
            {
                return default;
            }

            return new GroupByClauseCommand(groupables
                .Where(groupable => groupable.IsPrimaryColumn() && groupable.IsGroupable)
                .Aggregate(
                new List<string>(),
                (groupByDict, groupable) =>
                {
                    if (onlyPrimaryColumn)
                    {
                        if (groupable.IsPrimaryColumn() && groupable.IsVisible)
                        {
                            groupByDict.Add(groupable.FieldName());
                        }
                    }
                    else
                    {
                        var columnDefinitionLink = groupable.CurrentColumnDefinition();
                        while (columnDefinitionLink != null)
                        {
                            if (columnDefinitionLink.IsVisible)
                            {
                                groupByDict.Add(columnDefinitionLink.FieldName());
                            }

                            columnDefinitionLink = columnDefinitionLink.NextColumnDefinition();
                        }
                    }

                    return groupByDict;
                }));
        }
    }
}