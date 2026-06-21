using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Auth.DTOs;

namespace DrivingSchool.Application.Features.Auth.Commands.Login;

/// <summary>
/// Authenticates a user with email and password credentials.
/// Returns a new access/refresh token pair on success.
/// </summary>
public sealed record LoginCommand(
    string Email,
    string Password,
    string IpAddress) : ICommand<AuthResponseDto>;