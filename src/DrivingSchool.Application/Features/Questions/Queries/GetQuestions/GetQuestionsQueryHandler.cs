using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Questions.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Queries.GetQuestions;

/// <summary>Handles <see cref="GetQuestionsQuery"/>.</summary>
internal sealed class GetQuestionsQueryHandler
    : IQueryHandler<GetQuestionsQuery, PaginatedResult<QuestionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetQuestionsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<QuestionDto>>> Handle(
        GetQuestionsQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Questions.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            query.TopicId,
            query.Category,
            query.Source,
            query.Search,
            cancellationToken);

        var dtos = paginated.Items
            .Select(q => new QuestionDto(
                q.Id,
                q.TopicId,
                q.Category,
                q.Text,
                q.ImageUrl,
                q.Explanation,
                q.Source,
                q.IsActive,
                q.CorrectOptionId,
                q.Options.Select(o => new QuestionOptionDto(o.Id, o.Text)).ToList()))
            .ToList();

        return Result.Success(new PaginatedResult<QuestionDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }
}