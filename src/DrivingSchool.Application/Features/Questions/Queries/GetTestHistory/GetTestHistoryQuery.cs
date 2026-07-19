using DrivingSchool.Application.Features.Questions.DTOs;

namespace DrivingSchool.Application.Features.Questions.Queries.GetTestHistory;

/// <summary>Returns the completed and timed-out test sessions for a student's contract.</summary>
public sealed record GetTestHistoryQuery(Guid ContractId)
    : IQuery<IReadOnlyList<TestSessionSummaryDto>>;