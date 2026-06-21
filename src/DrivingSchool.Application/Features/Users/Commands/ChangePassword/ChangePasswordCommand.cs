using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Users.Commands.ChangePassword;

/// <summary>
/// Changes the password for the specified user.
/// The current password must be supplied for verification.
/// All existing refresh tokens are revoked on success.
/// </summary>
public sealed record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword) : ICommand;