namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>
/// Full result of a finished (completed or timed-out) test session,
/// including a per-question breakdown with correct answers revealed.
/// </summary>
public sealed record TestResultDto(
    Guid SessionId,
    Guid TemplateId,
    string TemplateName,
    SessionStatus Status,
    int Score,
    int TotalQuestions,
    bool IsPassed,
    DateTime StartedAt,
    DateTime? FinishedAt,
    IReadOnlyList<TestResultAnswerDto> Answers);

/// <summary>
/// A single answered (or unanswered, in the case of a timeout) question
/// within a finished test session, with the correct answer revealed.
/// </summary>
public sealed record TestResultAnswerDto(
    Guid QuestionId,
    string QuestionText,
    string? ImageUrl,
    IReadOnlyList<QuestionOptionDto> Options,
    Guid? SelectedOptionId,
    Guid CorrectOptionId,
    bool IsCorrect,
    string? Explanation);