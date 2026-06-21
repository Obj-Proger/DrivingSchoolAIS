using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Auth.Commands.ResetPassword;

/// <summary>
/// Resets the user's password using a valid reset token.
/// All existing refresh tokens are revoked, forcing re-authentication on all devices.
/// </summary>
public sealed record ResetPasswordCommand(
    string Token,
    string NewPassword) : ICommand;