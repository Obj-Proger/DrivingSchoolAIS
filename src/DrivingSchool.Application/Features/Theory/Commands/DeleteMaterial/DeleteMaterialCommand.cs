using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Theory.Commands.DeleteMaterial;

/// <summary>
/// Removes a material from a lesson and deletes the file from storage.
/// </summary>
public sealed record DeleteMaterialCommand(
    Guid LessonId,
    Guid MaterialId) : ICommand;