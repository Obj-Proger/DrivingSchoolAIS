using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Auth.DTOs;

namespace DrivingSchool.Application.Features.Auth.Commands.Register;

/// <summary>
/// Creates a new user account and sends an email confirmation link.
/// Returns the access and refresh tokens so the user is logged in immediately.
/// </summary>
public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Phone,
    string Password,

    /// <summary>
    /// Role to assign. Only <see cref="UserRole.Student"/> is accepted from the
    /// public registration endpoint. Other roles are assigned by an Admin.
    /// </summary>
    UserRole Role = UserRole.Student,

    /// <summary>IP address of the requesting client, injected by the controller.</summary>
    string IpAddress = "") : ICommand<AuthResponseDto>;