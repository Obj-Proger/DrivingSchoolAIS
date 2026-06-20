using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace DrivingSchool.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that runs all registered FluentValidation validators
/// for the incoming request before the handler is invoked.
/// Throws <see cref="ValidationException"/> if any validators report failures,
/// which is caught by the API's global exception handler and returned as HTTP 400.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    /// <param name="validators">
    /// All <see cref="IValidator{T}"/> instances registered for <typeparamref name="TRequest"/>.
    /// Injected by the DI container; empty collection when no validators are registered.
    /// </param>
    /// <param name="logger">Logger for diagnostic output.</param>
    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        // Run all validators concurrently
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var requestName = typeof(TRequest).Name;

        _logger.LogWarning(
            "Validation failed for {RequestName}. Errors: {@ValidationErrors}",
            requestName,
            failures.Select(f => new { f.PropertyName, f.ErrorMessage }));

        throw new ValidationException(failures);
    }
}