using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMCode.Web.Behaviors
{
    /// <summary>
    /// MediatR behavior for automatic validation of requests
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the ValidationBehavior class
        /// </summary>
        /// <param name="validators">Collection of validators for the request type</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Handles the request by validating it before passing to the next handler
        /// </summary>
        /// <param name="request">The request to validate</param>
        /// <param name="next">The next handler in the pipeline</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The response from the next handler</returns>
        /// <exception cref="ValidationException">Thrown when validation fails</exception>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
