using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.UpdateTestTemplate;

/// <summary>Handles <see cref="UpdateTestTemplateCommand"/>.</summary>
internal sealed class UpdateTestTemplateCommandHandler
    : ICommandHandler<UpdateTestTemplateCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTestTemplateCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateTestTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.TestTemplates
            .GetByIdAsync(command.TemplateId, cancellationToken);

        if (template is null)
            return Result.Failure(DomainErrors.TestTemplate.NotFound);

        // TestTemplate.Update performs no validation itself (unlike Create), so the
        // same invariants enforced at creation time are re-checked here to prevent
        // an update from silently corrupting the template into an invalid state.
        if (command.QuestionCount <= 0)
            return Result.Failure(DomainErrors.TestTemplate.InvalidQuestionCount);

        if (command.TimeLimitMinutes <= 0)
            return Result.Failure(DomainErrors.TestTemplate.InvalidTimeLimit);

        if (command.PassScore <= 0 || command.PassScore > command.QuestionCount)
            return Result.Failure(DomainErrors.TestTemplate.InvalidPassScore);

        template.Update(
            command.Name,
            command.QuestionCount,
            command.TimeLimitMinutes,
            command.PassScore,
            command.ErrorHandling,
            command.AddQuestionsOnError,
            command.AddMinutesOnError);

        _unitOfWork.TestTemplates.Update(template);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}