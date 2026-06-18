namespace DrivingSchool.Domain.Common;

/// <summary>
/// Base class for all domain entities.
/// Provides a unique identifier, audit timestamps, and domain event support.
/// </summary>
public abstract class BaseEntity : IHasDomainEvents, IAuditable
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>Gets the unique identifier of the entity.</summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <inheritdoc />
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Registers a domain event to be dispatched after the unit of work completes.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    /// <inheritdoc />
    public void ClearDomainEvents()
        => _domainEvents.Clear();
}