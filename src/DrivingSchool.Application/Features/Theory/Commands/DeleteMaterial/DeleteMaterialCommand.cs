using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Theory.Commands.DeleteMaterial;

/// <summary>
/// Removes a material and deletes the associated file from storage.
/// </summary>
/// <param name="MaterialId">The identifier of the material to remove.</param>
/// <param name="LessonId">
/// The identifier of the owning lesson, when deleting a lesson-attached material.
/// Leave <c>null</c> when deleting a topic-level material.
/// </param>
public sealed record DeleteMaterialCommand(
    Guid MaterialId,
    Guid? LessonId = null) : ICommand;