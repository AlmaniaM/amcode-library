using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMCode.Exports.Recipes.Interfaces;
using AMCode.Exports.Recipes.Models;

namespace AMCode.Exports.Recipes
{
    /// <summary>
    /// CSV recipe export builder with proper escaping, configurable delimiter, UTF-8 BOM support,
    /// and support for both flattened and expanded ingredient formats.
    /// </summary>
    public class RecipeCsvBuilder : IRecipeCsvBuilder
    {
        private readonly List<RecipeExportData> _recipes = new List<RecipeExportData>();
        private RecipeColumnConfiguration _columns = RecipeColumnConfiguration.Default;
        private char _delimiter = ',';
        private bool _expandedIngredients = false;

        /// <inheritdoc/>
        public IRecipeCsvBuilder AddRecipe(RecipeExportData recipe)
        {
            if (recipe != null)
            {
                _recipes.Add(recipe);
            }

            return this;
        }

        /// <inheritdoc/>
        public IRecipeCsvBuilder AddRecipes(IEnumerable<RecipeExportData> recipes)
        {
            if (recipes != null)
            {
                _recipes.AddRange(recipes.Where(r => r != null));
            }

            return this;
        }

        /// <inheritdoc/>
        public IRecipeCsvBuilder WithColumns(RecipeColumnConfiguration columns)
        {
            _columns = columns ?? RecipeColumnConfiguration.Default;
            return this;
        }

        /// <inheritdoc/>
        public IRecipeCsvBuilder WithDelimiter(char delimiter)
        {
            _delimiter = delimiter;
            return this;
        }

        /// <inheritdoc/>
        public IRecipeCsvBuilder WithExpandedIngredients(bool expanded = true)
        {
            _expandedIngredients = expanded;
            return this;
        }

        /// <inheritdoc/>
        public byte[] Build()
        {
            using var stream = BuildToStream();
            return ((MemoryStream)stream).ToArray();
        }

        /// <inheritdoc/>
        public Stream BuildToStream()
        {
            var stream = new MemoryStream();

            // UTF-8 BOM for Excel compatibility
            var bom = Encoding.UTF8.GetPreamble();
            stream.Write(bom, 0, bom.Length);

            using var writer = new StreamWriter(stream, new UTF8Encoding(false), 4096, leaveOpen: true);

            if (_expandedIngredients)
            {
                WriteExpandedFormat(writer);
            }
            else
            {
                WriteFlattenedFormat(writer);
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void WriteFlattenedFormat(StreamWriter writer)
        {
            // Write header row
            var headers = BuildFlattenedHeaders();
            writer.WriteLine(string.Join(_delimiter.ToString(), headers.Select(h => EscapeField(h))));

            // Write data rows — one row per recipe
            foreach (var recipe in _recipes)
            {
                var fields = BuildFlattenedRow(recipe);
                writer.WriteLine(string.Join(_delimiter.ToString(), fields.Select(f => EscapeField(f))));
            }
        }

        private void WriteExpandedFormat(StreamWriter writer)
        {
            // Expanded: one row per ingredient per recipe
            // Headers always include recipe name + ingredient columns
            var headers = new List<string>();
            if (_columns.IncludeTitle) headers.Add("Recipe Name");
            if (_columns.IncludeCategory) headers.Add("Category");
            headers.Add("Ingredient");
            headers.Add("Quantity");
            headers.Add("Unit");
            headers.Add("Notes");

            writer.WriteLine(string.Join(_delimiter.ToString(), headers.Select(h => EscapeField(h))));

            foreach (var recipe in _recipes)
            {
                if (recipe.Ingredients == null || !recipe.Ingredients.Any())
                {
                    var emptyFields = new List<string>();
                    if (_columns.IncludeTitle) emptyFields.Add(recipe.Title);
                    if (_columns.IncludeCategory) emptyFields.Add(recipe.Category);
                    emptyFields.Add(string.Empty);
                    emptyFields.Add(string.Empty);
                    emptyFields.Add(string.Empty);
                    emptyFields.Add(string.Empty);
                    writer.WriteLine(string.Join(_delimiter.ToString(), emptyFields.Select(f => EscapeField(f))));
                    continue;
                }

                foreach (var ingredient in recipe.Ingredients)
                {
                    var fields = new List<string>();
                    if (_columns.IncludeTitle) fields.Add(recipe.Title);
                    if (_columns.IncludeCategory) fields.Add(recipe.Category);
                    fields.Add(ingredient.Name);
                    fields.Add(ingredient.Amount);
                    fields.Add(ingredient.Unit);
                    fields.Add(ingredient.Notes);
                    writer.WriteLine(string.Join(_delimiter.ToString(), fields.Select(f => EscapeField(f))));
                }
            }
        }

        private IEnumerable<string> BuildFlattenedHeaders()
        {
            var headers = new List<string>();
            if (_columns.IncludeTitle) headers.Add("Recipe Name");
            if (_columns.IncludeAuthor) headers.Add("Author");
            if (_columns.IncludeCategory) headers.Add("Category");
            if (_columns.IncludePrepTime) headers.Add("Prep Time (min)");
            if (_columns.IncludeCookTime) headers.Add("Cook Time (min)");
            if (_columns.IncludeServings) headers.Add("Servings");
            if (_columns.IncludeDietaryTags) headers.Add("Dietary Tags");
            if (_columns.IncludeIngredients) headers.Add("Ingredients");
            if (_columns.IncludeInstructions) headers.Add("Instructions");
            if (_columns.IncludeSource) headers.Add("Source");
            if (_columns.IncludeCreatedDate) headers.Add("Created Date");
            return headers;
        }

        private IEnumerable<string> BuildFlattenedRow(RecipeExportData recipe)
        {
            var fields = new List<string>();
            if (_columns.IncludeTitle) fields.Add(recipe.Title);
            if (_columns.IncludeAuthor) fields.Add(recipe.Author);
            if (_columns.IncludeCategory) fields.Add(recipe.Category);
            if (_columns.IncludePrepTime) fields.Add(recipe.PrepTimeMinutes.ToString());
            if (_columns.IncludeCookTime) fields.Add(recipe.CookTimeMinutes.ToString());
            if (_columns.IncludeServings) fields.Add(recipe.Servings.ToString());

            if (_columns.IncludeDietaryTags)
            {
                fields.Add(string.Join("; ", recipe.DietaryTags ?? new List<string>()));
            }

            if (_columns.IncludeIngredients)
            {
                var ingredientSummaries = recipe.Ingredients?
                    .Select(i => string.IsNullOrEmpty(i.Amount)
                        ? i.Name
                        : $"{i.Amount} {i.Unit} {i.Name}".Trim())
                    ?? Enumerable.Empty<string>();
                fields.Add(string.Join("; ", ingredientSummaries));
            }

            if (_columns.IncludeInstructions)
            {
                fields.Add(string.Join(" | ", recipe.InstructionSteps ?? new List<string>()));
            }

            if (_columns.IncludeSource) fields.Add(recipe.Source);
            if (_columns.IncludeCreatedDate) fields.Add(recipe.CreatedAt.ToString("yyyy-MM-dd"));
            return fields;
        }

        /// <summary>
        /// Escapes a CSV field value per RFC 4180:
        /// - Fields containing the delimiter, double-quote, or newlines are wrapped in double quotes.
        /// - Double quotes within the field are doubled.
        /// </summary>
        private string EscapeField(string value)
        {
            if (value == null) return string.Empty;

            bool needsQuoting = value.Contains(_delimiter)
                || value.Contains('"')
                || value.Contains('\n')
                || value.Contains('\r');

            if (!needsQuoting) return value;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
