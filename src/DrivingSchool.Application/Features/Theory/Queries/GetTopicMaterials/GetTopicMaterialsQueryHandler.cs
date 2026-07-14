using DrivingSchool.Application.Features.Theory.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Queries.GetTopicMaterials;

/// <summary>
/// Handles <see cref="GetTopicMaterialsQuery"/>.
/// Returns publicly available materials attached directly to the specified course topic.
/// </summary>
internal sealed class GetTopicMaterialsQueryHandler
    : IQueryHandler<GetTopicMaterialsQuery, IReadOnlyList<LessonMaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTopicMaterialsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<LessonMaterialDto>>> Handle(
        GetTopicMaterialsQuery query,
        CancellationToken cancellationToken)
    {
        var materials = await _unitOfWork.LessonMaterials
            .GetByTopicIdAsync(query.TopicId, cancellationToken);

        var dtos = materials
            .Select(m => new LessonMaterialDto(
                m.Id,
                m.LessonId,
                m.TopicId,
                m.Title,
                m.FileUrl,
                m.ContentType,
                m.SizeBytes,
                m.IsPublic,
                m.CreatedAt))
            .OrderBy(m => m.Title)
            .ToList();

        return Result.Success<IReadOnlyList<LessonMaterialDto>>(dtos);
    }
}