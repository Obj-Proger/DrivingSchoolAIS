namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>
/// State of a test session currently in progress, returned to the student's client
/// so it can render the question list and countdown timer.
/// </summary>
public sealed record TestSessionInProgressDto(
    Guid Id,
    Guid TemplateId,
    string TemplateName,
    DateTime StartedAt,
    DateTime ExpiresAt,
    int TotalQuestions,
    IReadOnlyList<Guid> AnsweredQuestionIds,
    IReadOnlyList<TestQuestionDto> Questions);