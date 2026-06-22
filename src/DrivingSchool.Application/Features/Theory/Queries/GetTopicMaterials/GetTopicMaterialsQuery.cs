using DrivingSchool.Application.Features.Theory.DTOs;

namespace DrivingSchool.Application.Features.Theory.Queries.GetTopicMaterials;

/// <summary>
/// Returns all publicly available materials attached to a specific course topic.
/// Used by students in the online learning section.
/// </summary>
public sealed record GetTopicMaterialsQuery(Guid TopicId)
    : IQuery<IReadOnlyList<LessonMaterialDto>>;