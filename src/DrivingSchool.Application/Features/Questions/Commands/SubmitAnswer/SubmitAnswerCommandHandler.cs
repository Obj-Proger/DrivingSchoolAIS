using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.SubmitAnswer;

/// <summary>Handles <see cref="SubmitAnswerCommand"/>.</summary>
internal sealed class SubmitAnswerCommandHandler : ICommandHandler<SubmitAnswerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitAnswerCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        SubmitAnswerCommand command,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.TestSessions
            .GetByIdAsync(command.SessionId, cancellationToken);

        if (session is null)
            return Result.Failure(DomainErrors.TestSession.NotFound);

        var question = await _unitOfWork.Questions
            .GetByIdAsync(command.QuestionId, cancellationToken);

        if (question is null)
            return Result.Failure(DomainErrors.Question.NotFound);

        var isCorrect = question.IsAnswerCorrect(command.SelectedOptionId);

        var submitResult = session.SubmitAnswer(
            command.QuestionId, command.SelectedOptionId, isCorrect);

        if (submitResult.IsFailure)
        {
            // Persist the timeout transition if SubmitAnswer detected an expired session
            _unitOfWork.TestSessions.Update(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return submitResult;
        }

        if (!isCorrect)
        {
            var template = await _unitOfWork.TestTemplates
                .GetByIdAsync(session.TemplateId, cancellationToken);

            if (template is not null &&
                template.ErrorHandling == ErrorHandlingMode.AddQuestions &&
                template.AddQuestionsOnError > 0)
            {
                var bonusQuestions = await _unitOfWork.Questions.GetRandomAsync(
                    template.AddQuestionsOnError,
                    template.TopicIds.Count > 0 ? template.TopicIds : null,
                    template.Category,
                    excludeIds: session.QuestionIds,
                    ct: cancellationToken);

                session.AddBonusResources(
                    bonusQuestions.Select(q => q.Id),
                    template.AddMinutesOnError);
            }
        }

        _unitOfWork.TestSessions.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}