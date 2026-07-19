using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.UpdateQuestion;

/// <summary>Handles <see cref="UpdateQuestionCommand"/>.</summary>
internal sealed class UpdateQuestionCommandHandler : ICommandHandler<UpdateQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Questions
            .GetByIdAsync(command.QuestionId, cancellationToken);

        if (question is null)
            return Result.Failure(DomainErrors.Question.NotFound);

        // Question.Update trusts the caller for CorrectOptionId — guard here so an
        // id belonging to a different question can never silently corrupt this one.
        var optionBelongsToQuestion = question.Options.Any(o => o.Id == command.CorrectOptionId);
        if (!optionBelongsToQuestion)
            return Result.Failure(DomainErrors.Question.OptionNotFound);

        question.Update(
            command.Text,
            command.ImageUrl,
            command.Explanation,
            command.CorrectOptionId);

        _unitOfWork.Questions.Update(question);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}