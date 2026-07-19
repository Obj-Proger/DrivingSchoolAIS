using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.DeactivateQuestion;

/// <summary>Handles <see cref="DeactivateQuestionCommand"/>.</summary>
internal sealed class DeactivateQuestionCommandHandler : ICommandHandler<DeactivateQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateQuestionCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeactivateQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Questions
            .GetByIdAsync(command.QuestionId, cancellationToken);

        if (question is null)
            return Result.Failure(DomainErrors.Question.NotFound);

        question.Deactivate();

        _unitOfWork.Questions.Update(question);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}