using DrivingSchool.Application.Features.Questions.DTOs;

namespace DrivingSchool.Application.Features.Questions.Queries.GetActiveTestSession;

/// <summary>
/// Returns the student's currently in-progress test session, if any.
/// A <c>null</c> value is a normal, successful result — it simply means
/// no session is active — and is not treated as an error.
/// </summary>
public sealed record GetActiveTestSessionQuery(Guid ContractId)
    : IQuery<TestSessionInProgressDto?>;