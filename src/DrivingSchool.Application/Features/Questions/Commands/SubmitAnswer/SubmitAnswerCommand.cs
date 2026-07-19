using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.SubmitAnswer;

/// <summary>
/// Submits a student's answer to one question within an active test session.
/// If the answer is incorrect and the template's error-handling mode is
/// <see cref="ErrorHandlingMode.AddQuestions"/>, bonus questions and time
/// are appended to the session automatically.
/// </summary>
public sealed record SubmitAnswerCommand(
    Guid SessionId,
    Guid QuestionId,
    Guid SelectedOptionId) : ICommand;