using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a new user account is successfully created.
/// Triggers sending of an email confirmation message.
/// </summary>
/// <param name="UserId">The unique identifier of the newly created user.</param>
/// <param name="Email">The user's email address used for confirmation.</param>
/// <param name="FullName">The user's display name for personalising the email.</param>
public sealed record UserRegisteredEvent(
    Guid UserId,
    string Email,
    string FullName) : IDomainEvent;