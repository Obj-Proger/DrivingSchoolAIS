using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.CreateQuestion;

/// <summary>
/// Creates a new question in the test bank.
/// Restricted to administrators and instructors.
/// </summary>
public sealed record CreateQuestionCommand(
    Guid TopicId,
    LicenseCategory Category,
    string Text,
    IReadOnlyList<string> Options,
    int CorrectOptionIndex,
    string? ImageUrl = null,
    string? Explanation = null) : ICommand<Guid>;