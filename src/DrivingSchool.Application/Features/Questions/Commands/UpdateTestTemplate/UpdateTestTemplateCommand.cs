using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.UpdateTestTemplate;

/// <summary>
/// Updates a test template's scoring and timing rules.
/// The question-selection scope (course, topics, category) and auto-assignment
/// settings cannot be changed after creation — deactivate the template and
/// create a new one instead if the scope needs to change.
/// Restricted to administrators.
/// </summary>
public sealed record UpdateTestTemplateCommand(
    Guid TemplateId,
    string Name,
    int QuestionCount,
    int TimeLimitMinutes,
    int PassScore,
    ErrorHandlingMode ErrorHandling,
    int AddQuestionsOnError = 0,
    int AddMinutesOnError = 0) : ICommand;