using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Users.Commands.AssignRole;

/// <summary>
/// Assigns a new role to the specified user.
/// Restricted to administrators.
/// </summary>
public sealed record AssignRoleCommand(
    Guid UserId,
    UserRole NewRole) : ICommand;