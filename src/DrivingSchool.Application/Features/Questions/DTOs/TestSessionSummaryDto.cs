namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>Summary representation of a past test session used in history list views.</summary>
public sealed record TestSessionSummaryDto(
    Guid Id,
    Guid TemplateId,
    string TemplateName,
    SessionStatus Status,
    int Score,
    int TotalQuestions,
    bool? IsPassed,
    DateTime StartedAt,
    DateTime? FinishedAt);