namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Represents an item in a shopping list
    /// </summary>
    public class ShoppingListItem
    {
        public string Name { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public bool IsOptional { get; set; } = false;
        public string Notes { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
    }
}
