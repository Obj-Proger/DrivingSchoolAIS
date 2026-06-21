using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Auth.DTOs;

namespace DrivingSchool.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Exchanges a valid refresh token for a new access/refresh token pair.
/// The old refresh token is revoked on success (token rotation).
/// </summary>
public sealed record RefreshTokenCommand(
    string RefreshToken,
    string IpAddress) : ICommand<AuthResponseDto>;