using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a user account is deactivated by an administrator.
/// Can be used to revoke active sessions or notify the user.
/// </summary>
/// <param name="UserId">The unique identifier of the deactivated user.</param>
public sealed record UserDeactivatedEvent(Guid UserId) : IDomainEvent;