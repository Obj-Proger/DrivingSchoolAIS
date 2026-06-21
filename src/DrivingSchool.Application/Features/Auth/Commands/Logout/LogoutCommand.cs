using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Revokes the specified refresh token, ending the session on the current device.
/// </summary>
public sealed record LogoutCommand(string RefreshToken) : ICommand;