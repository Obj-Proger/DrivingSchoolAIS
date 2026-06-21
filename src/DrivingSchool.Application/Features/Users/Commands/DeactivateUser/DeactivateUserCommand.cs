using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Users.Commands.DeactivateUser;

/// <summary>
/// Deactivates a user account, preventing further logins.
/// Restricted to administrators.
/// </summary>
public sealed record DeactivateUserCommand(Guid UserId) : ICommand;