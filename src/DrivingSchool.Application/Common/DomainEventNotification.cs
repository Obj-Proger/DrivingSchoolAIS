namespace DrivingSchool.Application.Common;

/// <summary>
/// Wraps a domain event so it can be published through MediatR's notification pipeline.
/// </summary>
/// <typeparam name="TDomainEvent">The domain event type.</typeparam>
/// <param name="DomainEvent">The domain event instance to dispatch.</param>
/// <remarks>
/// This wrapper preserves Domain layer independence from MediatR.
/// Domain events implement <see cref="IDomainEvent"/> (no external dependencies),
/// while this wrapper, defined in the Application layer, bridges to MediatR's
/// <see cref="INotification"/> pipeline.
/// </remarks>
public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent)
    : INotification
    where TDomainEvent : IDomainEvent;