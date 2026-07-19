using DrivingSchool.Application.Features.Questions.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Queries.GetTestHistory;

/// <summary>Handles <see cref="GetTestHistoryQuery"/>.</summary>
internal sealed class GetTestHistoryQueryHandler
    : IQueryHandler<GetTestHistoryQuery, IReadOnlyList<TestSessionSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTestHistoryQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<TestSessionSummaryDto>>> Handle(
        GetTestHistoryQuery query,
        CancellationToken cancellationToken)
    {
        var sessions = await _unitOfWork.TestSessions
            .GetHistoryByContractAsync(query.ContractId, cancellationToken);

        var dtos = new List<TestSessionSummaryDto>();
        foreach (var session in sessions)
        {
            var template = await _unitOfWork.TestTemplates
                .GetByIdAsync(session.TemplateId, cancellationToken);

            dtos.Add(new TestSessionSummaryDto(
                session.Id,
                session.TemplateId,
                template?.Name ?? "Unknown",
                session.Status,
                session.Score,
                session.TotalQuestions,
                session.IsPassed,
                session.StartedAt,
                session.FinishedAt));
        }

        return Result.Success<IReadOnlyList<TestSessionSummaryDto>>(dtos);
    }
}