using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Auth.Commands.ConfirmEmail;

/// <summary>
/// Confirms the email address of the user identified by the provided token.
/// </summary>
public sealed record ConfirmEmailCommand(string Token) : ICommand;