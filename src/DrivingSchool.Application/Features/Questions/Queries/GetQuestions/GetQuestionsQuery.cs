using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Questions.DTOs;

namespace DrivingSchool.Application.Features.Questions.Queries.GetQuestions;

/// <summary>
/// Returns a paginated, filterable list of questions for bank management screens.
/// </summary>
public sealed record GetQuestionsQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? TopicId = null,
    LicenseCategory? Category = null,
    QuestionSource? Source = null,
    string? Search = null) : IQuery<PaginatedResult<QuestionDto>>;