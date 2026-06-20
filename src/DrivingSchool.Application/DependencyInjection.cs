using System.Reflection;
using DrivingSchool.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DrivingSchool.Application;

/// <summary>
/// Provides the <see cref="AddApplication"/> extension method for registering
/// all Application layer services with the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers Application layer services:
    /// MediatR handlers, FluentValidation validators, and pipeline behaviors.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR
        // Scans the assembly for IRequestHandler and INotificationHandler implementations.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            // Pipeline behaviors execute in registration order (outermost first).
            // Logging → Performance → Validation → Handler
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // FluentValidation
        // Scans the assembly for all IValidator<T> implementations.
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        // Mapster
        // Type mappings are configured per feature using TypeAdapterConfig
        // and applied via the .Adapt<T>() extension method.
        // No centralised registration is required.

        return services;
    }
}