using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Recipes.Interfaces;
using AMCode.Exports.Recipes.Models;

namespace AMCode.Exports.Recipes
{
    /// <summary>
    /// Handles large-scale recipe exports efficiently using streaming and batching to
    /// avoid out-of-memory errors when exporting 100+ recipes.
    /// </summary>
    public class BulkRecipeExporter
    {
        private readonly int _batchSize;

        /// <summary>
        /// Creates a <see cref="BulkRecipeExporter"/> with the specified batch size.
        /// </summary>
        /// <param name="batchSize">Number of recipes processed per batch. Default: 50.</param>
        public BulkRecipeExporter(int batchSize = 50)
        {
            if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero.");
            _batchSize = batchSize;
        }

        /// <summary>
        /// Export recipes to an Excel workbook stream using batched processing.
        /// </summary>
        /// <param name="recipes">The recipes to export.</param>
        /// <param name="columns">Column configuration.</param>
        /// <param name="formatting">Excel formatting options.</param>
        /// <param name="includeNutrition">Whether to include the Nutrition sheet.</param>
        /// <param name="progress">Optional progress callback — called with (processed, total) after each batch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Stream"/> containing the Excel workbook bytes.</returns>
        public async Task<Stream> ExportToExcelAsync(
            IEnumerable<RecipeExportData> recipes,
            RecipeColumnConfiguration columns = null,
            ExcelFormattingOptions formatting = null,
            bool includeNutrition = false,
            Action<int, int> progress = null,
            CancellationToken cancellationToken = default)
        {
            if (recipes == null) throw new ArgumentNullException(nameof(recipes));

            var recipeList = recipes.ToList();
            var total = recipeList.Count;
            var builder = new RecipeExcelBuilder()
                .WithColumns(columns ?? RecipeColumnConfiguration.Default)
                .WithFormatting(formatting ?? ExcelFormattingOptions.Default)
                .IncludeNutritionSheet(includeNutrition);

            int processed = 0;
            foreach (var batch in Batch(recipeList, _batchSize))
            {
                cancellationToken.ThrowIfCancellationRequested();
                builder.AddRecipes(batch);
                processed += batch.Count;
                progress?.Invoke(processed, total);
                await Task.Yield(); // Yield to avoid blocking the thread on large batches
            }

            return builder.BuildToStream();
        }

        /// <summary>
        /// Export recipes to a CSV stream using batched processing.
        /// </summary>
        /// <param name="recipes">The recipes to export.</param>
        /// <param name="columns">Column configuration.</param>
        /// <param name="delimiter">Field delimiter. Default: comma.</param>
        /// <param name="expandedIngredients">Whether to use expanded ingredient format (one row per ingredient).</param>
        /// <param name="progress">Optional progress callback — called with (processed, total) after each batch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Stream"/> containing the CSV bytes (UTF-8 BOM included).</returns>
        public async Task<Stream> ExportToCsvAsync(
            IEnumerable<RecipeExportData> recipes,
            RecipeColumnConfiguration columns = null,
            char delimiter = ',',
            bool expandedIngredients = false,
            Action<int, int> progress = null,
            CancellationToken cancellationToken = default)
        {
            if (recipes == null) throw new ArgumentNullException(nameof(recipes));

            var recipeList = recipes.ToList();
            var total = recipeList.Count;
            var builder = new RecipeCsvBuilder()
                .WithColumns(columns ?? RecipeColumnConfiguration.Default)
                .WithDelimiter(delimiter)
                .WithExpandedIngredients(expandedIngredients);

            int processed = 0;
            foreach (var batch in Batch(recipeList, _batchSize))
            {
                cancellationToken.ThrowIfCancellationRequested();
                builder.AddRecipes(batch);
                processed += batch.Count;
                progress?.Invoke(processed, total);
                await Task.Yield();
            }

            return builder.BuildToStream();
        }

        /// <summary>
        /// Partition a list into batches of the specified size.
        /// </summary>
        private static IEnumerable<List<T>> Batch<T>(List<T> source, int batchSize)
        {
            for (int i = 0; i < source.Count; i += batchSize)
            {
                yield return source.GetRange(i, Math.Min(batchSize, source.Count - i));
            }
        }
    }
}
