namespace DrivingSchool.Domain.Common;

/// <summary>
/// Marker interface for domain events.
/// Domain events are dispatched via MediatR after the aggregate
/// state has been persisted to the database.
/// </summary>
public interface IDomainEvent;