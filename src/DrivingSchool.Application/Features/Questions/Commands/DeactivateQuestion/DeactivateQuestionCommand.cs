using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.DeactivateQuestion;

/// <summary>
/// Deactivates a question, excluding it from future test sessions
/// while preserving it in the history of already-completed sessions.
/// Restricted to administrators and instructors.
/// </summary>
public sealed record DeactivateQuestionCommand(Guid QuestionId) : ICommand;