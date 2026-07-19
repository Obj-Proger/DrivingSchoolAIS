using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.CreateTestTemplate;

/// <summary>Handles <see cref="CreateTestTemplateCommand"/>.</summary>
internal sealed class CreateTestTemplateCommandHandler
    : ICommandHandler<CreateTestTemplateCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTestTemplateCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateTestTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var templateResult = TestTemplate.Create(
            command.Name,
            command.Type,
            command.QuestionCount,
            command.TimeLimitMinutes,
            command.PassScore,
            command.ErrorHandling,
            command.AddQuestionsOnError,
            command.AddMinutesOnError,
            command.CourseId,
            command.TopicIds,
            command.Category,
            command.IsAutoAssigned,
            command.AutoAssignEveryNLessons);

        if (templateResult.IsFailure)
            return Result.Failure<Guid>(templateResult.Error);

        await _unitOfWork.TestTemplates.AddAsync(templateResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(templateResult.Value.Id);
    }
}