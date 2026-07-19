using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Questions.DTOs;

namespace DrivingSchool.Application.Features.Questions.Commands.StartTestSession;

/// <summary>
/// Starts a new test session for a student, drawing a random set of questions
/// according to the template's rules. Fails if the student already has a
/// session in progress.
/// </summary>
public sealed record StartTestSessionCommand(
    Guid ContractId,
    Guid TemplateId) : ICommand<TestSessionInProgressDto>;