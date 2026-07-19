using DrivingSchool.Application.Features.Questions.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Queries.GetActiveTestSession;

/// <summary>Handles <see cref="GetActiveTestSessionQuery"/>.</summary>
internal sealed class GetActiveTestSessionQueryHandler
    : IQueryHandler<GetActiveTestSessionQuery, TestSessionInProgressDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetActiveTestSessionQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<TestSessionInProgressDto?>> Handle(
        GetActiveTestSessionQuery query,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.TestSessions
            .GetActiveByContractAsync(query.ContractId, cancellationToken);

        if (session is null)
            return Result.Success<TestSessionInProgressDto?>(null);

        var template = await _unitOfWork.TestTemplates
            .GetByIdAsync(session.TemplateId, cancellationToken);

        var questionDtos = new List<TestQuestionDto>();
        foreach (var questionId in session.QuestionIds)
        {
            var question = await _unitOfWork.Questions
                .GetByIdAsync(questionId, cancellationToken);

            if (question is null) continue;

            questionDtos.Add(new TestQuestionDto(
                question.Id,
                question.Text,
                question.ImageUrl,
                question.Options.Select(o => new QuestionOptionDto(o.Id, o.Text)).ToList()));
        }

        var answeredIds = session.Answers.Select(a => a.QuestionId).ToList();

        return Result.Success<TestSessionInProgressDto?>(new TestSessionInProgressDto(
            session.Id,
            session.TemplateId,
            template?.Name ?? "Unknown",
            session.StartedAt,
            session.ExpiresAt,
            session.TotalQuestions,
            answeredIds,
            questionDtos));
    }
}