using MediatR;
using Microsoft.Extensions.Logging;

namespace AMCode.Web.Behaviors
{
    /// <summary>
    /// MediatR behavior for logging requests and responses
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestGuid = Guid.NewGuid().ToString();

            _logger.LogInformation(
                "[START] {RequestName} with {RequestGuid}",
                requestName,
                requestGuid);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var response = await next();
                
                stopwatch.Stop();
                
                _logger.LogInformation(
                    "[END] {RequestName} with {RequestGuid} completed in {ElapsedMilliseconds}ms",
                    requestName,
                    requestGuid,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                _logger.LogError(ex,
                    "[ERROR] {RequestName} with {RequestGuid} failed after {ElapsedMilliseconds}ms: {ErrorMessage}",
                    requestName,
                    requestGuid,
                    stopwatch.ElapsedMilliseconds,
                    ex.Message);

                throw;
            }
        }
    }
}
