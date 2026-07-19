using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Questions.DTOs;

namespace DrivingSchool.Application.Features.Questions.Commands.FinishTestSession;

/// <summary>
/// Finalises an in-progress test session, calculates the score against the
/// template's pass threshold, and returns the full result breakdown.
/// </summary>
public sealed record FinishTestSessionCommand(Guid SessionId) : ICommand<TestResultDto>;