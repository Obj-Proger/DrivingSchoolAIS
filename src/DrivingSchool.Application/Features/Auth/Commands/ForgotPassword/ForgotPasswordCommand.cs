using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Auth.Commands.ForgotPassword;

/// <summary>
/// Initiates a password reset flow by sending a reset link to the user's email address.
/// Always returns success to prevent user enumeration attacks.
/// </summary>
public sealed record ForgotPasswordCommand(string Email) : ICommand;