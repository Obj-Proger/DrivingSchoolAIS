using Microsoft.Extensions.Logging;

namespace DrivingSchool.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs the start and completion
/// (or failure) of every request passing through the pipeline.
/// Positioned as the outermost behavior so it captures
/// the total execution time including validation.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <param name="logger">Logger for structured output.</param>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {RequestName}", requestName);

        try
        {
            var response = await next();

            _logger.LogInformation(
                "Handled {RequestName} successfully", requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Request {RequestName} failed with exception: {ExceptionMessage}",
                requestName,
                ex.Message);

            throw;
        }
    }
}