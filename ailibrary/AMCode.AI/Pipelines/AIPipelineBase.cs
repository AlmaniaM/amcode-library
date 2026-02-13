using AMCode.AI.Factories;
using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Pipelines;

/// <summary>
/// Base class for AI pipelines. Handles provider resolution from configuration,
/// retry logic, and fallback provider execution.
///
/// Subclasses implement ExecuteWithProviderAsync to define the actual AI call.
/// </summary>
/// <typeparam name="TInput">The input type for the pipeline</typeparam>
/// <typeparam name="TOutput">The output type for the pipeline</typeparam>
public abstract class AIPipelineBase<TInput, TOutput> : IAIPipeline<TInput, TOutput>
{
    protected readonly IAIProviderFactory _providerFactory;
    protected readonly PipelineConfiguration _config;
    protected readonly ILogger _logger;

    /// <summary>
    /// Name of this pipeline, used for configuration lookup and logging
    /// </summary>
    public abstract string PipelineName { get; }

    protected AIPipelineBase(
        IAIProviderFactory providerFactory,
        PipelineConfiguration config,
        ILogger logger)
    {
        _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Resolves the primary AI provider from pipeline configuration.
    /// Falls back to the global AI:Provider if pipeline config has no provider set.
    /// </summary>
    protected IAIProvider GetProvider()
    {
        if (!string.IsNullOrEmpty(_config.Provider))
        {
            var provider = _providerFactory.CreateProviderByName(_config.Provider, _config.Model);
            if (provider != null)
            {
                return provider;
            }

            _logger.LogWarning(
                "Pipeline '{PipelineName}': configured provider '{Provider}' not found, falling back to default",
                PipelineName, _config.Provider);
        }

        // Fall back to default provider
        return _providerFactory.CreateProvider();
    }

    /// <summary>
    /// Resolves the fallback AI provider from pipeline configuration.
    /// Returns null if no fallback is configured.
    /// </summary>
    protected IAIProvider? GetFallbackProvider()
    {
        if (string.IsNullOrEmpty(_config.FallbackProvider))
        {
            return null;
        }

        var provider = _providerFactory.CreateProviderByName(_config.FallbackProvider, _config.FallbackModel);
        if (provider == null)
        {
            _logger.LogWarning(
                "Pipeline '{PipelineName}': fallback provider '{FallbackProvider}' not found",
                PipelineName, _config.FallbackProvider);
        }

        return provider;
    }

    /// <summary>
    /// Execute the pipeline: try primary provider, fall back on failure.
    /// </summary>
    public virtual async Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken cancellationToken = default)
    {
        var provider = GetProvider();

        _logger.LogInformation(
            "Pipeline '{PipelineName}': executing with provider '{ProviderName}'",
            PipelineName, provider.ProviderName);

        var result = await ExecuteWithRetryAsync(provider, input, cancellationToken);

        if (!result.IsSuccess && _config.FallbackProvider != null)
        {
            var fallback = GetFallbackProvider();
            if (fallback != null)
            {
                _logger.LogWarning(
                    "Pipeline '{PipelineName}': primary provider '{Primary}' failed ({Error}), trying fallback '{Fallback}'",
                    PipelineName, provider.ProviderName, result.Error, fallback.ProviderName);

                result = await ExecuteWithRetryAsync(fallback, input, cancellationToken);
            }
        }

        return result;
    }

    /// <summary>
    /// Execute with retry logic based on MaxRetries configuration.
    /// </summary>
    private async Task<Result<TOutput>> ExecuteWithRetryAsync(
        IAIProvider provider, TInput input, CancellationToken cancellationToken)
    {
        Result<TOutput>? lastResult = null;

        for (var attempt = 0; attempt <= _config.MaxRetries; attempt++)
        {
            if (attempt > 0)
            {
                _logger.LogWarning(
                    "Pipeline '{PipelineName}': retry attempt {Attempt}/{MaxRetries}",
                    PipelineName, attempt, _config.MaxRetries);

                // Exponential backoff: 1s, 2s, 4s...
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)), cancellationToken);
            }

            try
            {
                lastResult = await ExecuteWithProviderAsync(provider, input, cancellationToken);
                if (lastResult.IsSuccess)
                {
                    return lastResult;
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Pipeline '{PipelineName}': attempt {Attempt} failed with exception",
                    PipelineName, attempt);

                lastResult = Result<TOutput>.Failure($"{ex.GetType().Name}: {ex.Message}");
            }
        }

        return lastResult ?? Result<TOutput>.Failure("Pipeline execution failed with no result");
    }

    /// <summary>
    /// Subclass implements the actual AI call for this pipeline.
    /// </summary>
    /// <param name="provider">The resolved AI provider to use</param>
    /// <param name="input">The input to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing the output or an error</returns>
    protected abstract Task<Result<TOutput>> ExecuteWithProviderAsync(
        IAIProvider provider, TInput input, CancellationToken cancellationToken);
}
