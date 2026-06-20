using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DrivingSchool.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that measures the execution time of each request.
/// Emits a warning log when a request takes longer than the configured threshold.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Requests exceeding this duration are logged as warnings.
    /// </summary>
    private static readonly TimeSpan WarningThreshold = TimeSpan.FromMilliseconds(500);

    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    /// <param name="logger">Logger for performance warnings.</param>
    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        if (stopwatch.Elapsed > WarningThreshold)
        {
            _logger.LogWarning(
                "Slow request detected: {RequestName} took {ElapsedMilliseconds}ms " +
                "(threshold: {ThresholdMs}ms). Request: {@Request}",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds,
                WarningThreshold.TotalMilliseconds,
                request);
        }

        return response;
    }
}