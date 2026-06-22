using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Theory.Commands.UploadMaterial;

/// <summary>
/// Uploads a file and attaches it as a material to a lesson or topic.
/// Exactly one of <see cref="LessonId"/> or <see cref="TopicId"/> must be provided.
/// </summary>
public sealed record UploadMaterialCommand(
    Guid? LessonId,
    Guid? TopicId,
    string Title,
    Stream FileStream,
    string FileName,
    string ContentType,
    bool IsPublic = false) : ICommand<Guid>;