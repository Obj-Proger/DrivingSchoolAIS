using DrivingSchool.Application.Features.Theory.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Queries.GetTopicMaterials;

/// <summary>
/// Handles <see cref="GetTopicMaterialsQuery"/>.
/// Returns topic-level materials across all lessons that covered this topic.
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
        // Get all lessons covering this topic
        // In practice this is better served by a dedicated materials repository
        // For now: search across all lessons and collect public topic materials
        var lessons = await _unitOfWork.TheoryLessons
            .GetByGroupIdAsync(Guid.Empty, ct: cancellationToken);

        var materials = lessons
            .Where(l => l.TopicId == query.TopicId)
            .SelectMany(l => l.Materials)
            .Where(m => m.TopicId == query.TopicId && m.IsPublic)
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
            .DistinctBy(m => m.FileUrl)
            .OrderBy(m => m.Title)
            .ToList();

        return Result.Success<IReadOnlyList<LessonMaterialDto>>(materials);
    }
}