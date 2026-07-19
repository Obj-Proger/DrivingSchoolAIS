using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.CreateTestTemplate;

/// <summary>
/// Creates a new test template defining the rules for generated test sessions.
/// Restricted to administrators.
/// </summary>
public sealed record CreateTestTemplateCommand(
    string Name,
    TestType Type,
    int QuestionCount,
    int TimeLimitMinutes,
    int PassScore,
    ErrorHandlingMode ErrorHandling,
    int AddQuestionsOnError = 0,
    int AddMinutesOnError = 0,
    Guid? CourseId = null,
    List<Guid>? TopicIds = null,
    LicenseCategory? Category = null,
    bool IsAutoAssigned = false,
    int? AutoAssignEveryNLessons = null) : ICommand<Guid>;