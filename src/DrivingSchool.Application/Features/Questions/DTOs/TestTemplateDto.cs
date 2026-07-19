namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>Represents the configuration of a test template.</summary>
public sealed record TestTemplateDto(
    Guid Id,
    string Name,
    TestType Type,
    Guid? CourseId,
    IReadOnlyList<Guid> TopicIds,
    LicenseCategory? Category,
    int QuestionCount,
    int TimeLimitMinutes,
    int PassScore,
    ErrorHandlingMode ErrorHandling,
    int AddQuestionsOnError,
    int AddMinutesOnError,
    bool IsAutoAssigned,
    int? AutoAssignEveryNLessons,
    bool IsActive);