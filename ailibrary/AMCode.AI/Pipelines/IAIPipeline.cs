using AMCode.AI.Models;

namespace AMCode.AI.Pipelines;

/// <summary>
/// Generic pipeline interface for AI task execution.
/// Each AI task (recipe extraction, classification, grocery list generation, etc.)
/// implements this interface with its own input/output types.
/// </summary>
/// <typeparam name="TInput">The input type for the pipeline</typeparam>
/// <typeparam name="TOutput">The output type for the pipeline</typeparam>
public interface IAIPipeline<TInput, TOutput>
{
    /// <summary>
    /// Name of this pipeline, used for configuration lookup (AI:Pipelines:{PipelineName})
    /// </summary>
    string PipelineName { get; }

    /// <summary>
    /// Execute the pipeline with the given input
    /// </summary>
    /// <param name="input">The input to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing the output or an error</returns>
    Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken cancellationToken = default);
}
