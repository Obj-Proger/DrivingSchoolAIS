namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>
/// Full representation of a question for question-bank management screens.
/// Includes <see cref="CorrectOptionId"/> — never send this DTO to a student
/// taking an active test; use <see cref="TestQuestionDto"/> instead.
/// </summary>
public sealed record QuestionDto(
    Guid Id,
    Guid TopicId,
    LicenseCategory Category,
    string Text,
    string? ImageUrl,
    string? Explanation,
    QuestionSource Source,
    bool IsActive,
    Guid CorrectOptionId,
    IReadOnlyList<QuestionOptionDto> Options);