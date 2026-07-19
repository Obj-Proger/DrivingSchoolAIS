using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Questions.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.FinishTestSession;

/// <summary>Handles <see cref="FinishTestSessionCommand"/>.</summary>
internal sealed class FinishTestSessionCommandHandler
    : ICommandHandler<FinishTestSessionCommand, TestResultDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public FinishTestSessionCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<TestResultDto>> Handle(
        FinishTestSessionCommand command,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.TestSessions
            .GetByIdAsync(command.SessionId, cancellationToken);

        if (session is null)
            return Result.Failure<TestResultDto>(DomainErrors.TestSession.NotFound);

        var template = await _unitOfWork.TestTemplates
            .GetByIdAsync(session.TemplateId, cancellationToken);

        if (template is null)
            return Result.Failure<TestResultDto>(DomainErrors.TestTemplate.NotFound);

        var finishResult = session.Finish(template.PassScore);
        if (finishResult.IsFailure)
            return Result.Failure<TestResultDto>(finishResult.Error);

        _unitOfWork.TestSessions.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var answerDtos = new List<TestResultAnswerDto>();
        foreach (var questionId in session.QuestionIds)
        {
            var question = await _unitOfWork.Questions
                .GetByIdAsync(questionId, cancellationToken);

            if (question is null) continue;

            var answer = session.Answers.FirstOrDefault(a => a.QuestionId == questionId);

            answerDtos.Add(new TestResultAnswerDto(
                question.Id,
                question.Text,
                question.ImageUrl,
                question.Options.Select(o => new QuestionOptionDto(o.Id, o.Text)).ToList(),
                answer?.SelectedOptionId,
                question.CorrectOptionId,
                answer?.IsCorrect ?? false,
                question.Explanation));
        }

        return Result.Success(new TestResultDto(
            session.Id,
            template.Id,
            template.Name,
            session.Status,
            session.Score,
            session.TotalQuestions,
            session.IsPassed ?? false,
            session.StartedAt,
            session.FinishedAt,
            answerDtos));
    }
}