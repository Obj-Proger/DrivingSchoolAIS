namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>
/// Question representation shown to a student during an active test session.
/// Deliberately omits <c>CorrectOptionId</c> and <c>Explanation</c> — these are
/// only revealed after the session is finished, via <see cref="TestResultAnswerDto"/>.
/// </summary>
public sealed record TestQuestionDto(
    Guid Id,
    string Text,
    string? ImageUrl,
    IReadOnlyList<QuestionOptionDto> Options);