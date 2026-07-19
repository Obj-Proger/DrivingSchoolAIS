using DrivingSchool.Application.Features.Questions.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Queries.GetTestTemplates;

/// <summary>Handles <see cref="GetTestTemplatesQuery"/>.</summary>
internal sealed class GetTestTemplatesQueryHandler
    : IQueryHandler<GetTestTemplatesQuery, IReadOnlyList<TestTemplateDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTestTemplatesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<TestTemplateDto>>> Handle(
        GetTestTemplatesQuery query,
        CancellationToken cancellationToken)
    {
        var templates = await _unitOfWork.TestTemplates
            .GetAllAsync(query.ActiveOnly, query.CourseId, cancellationToken);

        var dtos = templates
            .Select(t => new TestTemplateDto(
                t.Id,
                t.Name,
                t.Type,
                t.CourseId,
                t.TopicIds,
                t.Category,
                t.QuestionCount,
                t.TimeLimitMinutes,
                t.PassScore,
                t.ErrorHandling,
                t.AddQuestionsOnError,
                t.AddMinutesOnError,
                t.IsAutoAssigned,
                t.AutoAssignEveryNLessons,
                t.IsActive))
            .OrderBy(t => t.Name)
            .ToList();

        return Result.Success<IReadOnlyList<TestTemplateDto>>(dtos);
    }
}