using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.CreateQuestion;

/// <summary>Handles <see cref="CreateQuestionCommand"/>.</summary>
internal sealed class CreateQuestionCommandHandler : ICommandHandler<CreateQuestionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuestionCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var questionResult = Question.Create(
            command.TopicId,
            command.Category,
            command.Text,
            command.Options,
            command.CorrectOptionIndex,
            QuestionSource.Custom,
            command.ImageUrl,
            command.Explanation);

        if (questionResult.IsFailure)
            return Result.Failure<Guid>(questionResult.Error);

        await _unitOfWork.Questions.AddAsync(questionResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(questionResult.Value.Id);
    }
}