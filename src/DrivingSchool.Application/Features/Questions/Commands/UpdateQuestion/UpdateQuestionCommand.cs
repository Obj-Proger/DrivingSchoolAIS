using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.UpdateQuestion;

/// <summary>
/// Updates a question's text, image, explanation, and correct answer.
/// The set of answer options themselves cannot be changed after creation —
/// deactivate the question and create a new one instead if the options
/// need to change.
/// Restricted to administrators and instructors.
/// </summary>
public sealed record UpdateQuestionCommand(
    Guid QuestionId,
    string Text,
    Guid CorrectOptionId,
    string? ImageUrl = null,
    string? Explanation = null) : ICommand;