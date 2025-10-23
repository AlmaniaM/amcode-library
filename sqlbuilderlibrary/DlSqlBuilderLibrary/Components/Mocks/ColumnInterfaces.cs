using System;

namespace AMCode.Sql.Commands.Models
{
    /// <summary>
    /// Interface for data query column definitions.
    /// </summary>
    public interface IDataQueryColumnDefinition : IGroupable
    {
        /// <summary>
        /// Gets the field name of the column.
        /// </summary>
        string FieldName();
        
        /// <summary>
        /// Gets whether this column is selected.
        /// </summary>
        bool IsSelected();
        
        /// <summary>
        /// Gets whether this column is visible.
        /// </summary>
        bool IsVisible { get; }
        
        /// <summary>
        /// Gets the next column definition.
        /// </summary>
        IDataQueryColumnDefinition NextColumnDefinition();
        
        /// <summary>
        /// Gets the display name of the column.
        /// </summary>
        string DisplayName { get; }
    }
    
    /// <summary>
    /// Interface for sort providers.
    /// </summary>
    public interface ISortProvider
    {
        /// <summary>
        /// Gets the sort expression.
        /// </summary>
        /// <returns>The sort expression.</returns>
        string GetSort();
        
        /// <summary>
        /// Gets the sort expression with ascending parameter.
        /// </summary>
        /// <param name="ascending">Whether to sort ascending.</param>
        /// <returns>The sort expression.</returns>
        string GetSort(bool? ascending);
        
        /// <summary>
        /// Gets the sort expression with a name parameter.
        /// </summary>
        /// <param name="name">The name parameter.</param>
        /// <returns>The sort expression.</returns>
        string GetSort(string name);
        
        /// <summary>
        /// Gets the sort expression with name and ascending parameters.
        /// </summary>
        /// <param name="name">The name parameter.</param>
        /// <param name="ascending">Whether to sort ascending.</param>
        /// <returns>The sort expression.</returns>
        string GetSort(string name, bool? ascending);
        
        /// <summary>
        /// Gets the sort index.
        /// </summary>
        int SortIndex { get; }
        
        /// <summary>
        /// Gets whether this sort provider is visible.
        /// </summary>
        bool IsVisible { get; }
    }
    
    /// <summary>
    /// Interface for query expressions.
    /// </summary>
    public interface IGetQueryExpression
    {
        /// <summary>
        /// Gets the query expression.
        /// </summary>
        /// <returns>The query expression.</returns>
        string GetQueryExpression();
        
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <returns>The expression.</returns>
        string GetExpression();
        
        /// <summary>
        /// Gets the expression with a name parameter.
        /// </summary>
        /// <param name="name">The name parameter.</param>
        /// <returns>The expression.</returns>
        string GetExpression(string name);
        
        /// <summary>
        /// Gets whether this query expression is visible.
        /// </summary>
        bool IsVisible { get; }
    }
    
    /// <summary>
    /// Interface for groupable items.
    /// </summary>
    public interface IGroupable
    {
        /// <summary>
        /// Gets whether this item is groupable.
        /// </summary>
        bool IsGroupable { get; }
        
        /// <summary>
        /// Gets the field name.
        /// </summary>
        string FieldName();
        
        /// <summary>
        /// Gets whether this is a primary column.
        /// </summary>
        bool IsPrimaryColumn();
        
        /// <summary>
        /// Gets whether this item is visible.
        /// </summary>
        bool IsVisible { get; }
        
        /// <summary>
        /// Gets the current column definition.
        /// </summary>
        IDataQueryColumnDefinition CurrentColumnDefinition();
    }
}
