using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Theory.Commands.DeleteMaterial;

/// <summary>
/// Handles <see cref="DeleteMaterialCommand"/>.
/// Removes the material record (lesson-attached or topic-level) and deletes
/// the associated file from storage.
/// </summary>
internal sealed class DeleteMaterialCommandHandler : ICommandHandler<DeleteMaterialCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorage;

    public DeleteMaterialCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _fileStorage = fileStorage;
    }

    public async Task<Result> Handle(
        DeleteMaterialCommand command,
        CancellationToken cancellationToken)
    {
        if (command.LessonId.HasValue)
            return await DeleteLessonMaterialAsync(
                command.LessonId.Value, command.MaterialId, cancellationToken);

        return await DeleteTopicMaterialAsync(command.MaterialId, cancellationToken);
    }

    private async Task<Result> DeleteLessonMaterialAsync(
        Guid lessonId,
        Guid materialId,
        CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.TheoryLessons
            .GetByIdWithDetailsAsync(lessonId, cancellationToken);

        if (lesson is null)
            return Result.Failure(DomainErrors.TheoryLesson.NotFound);

        // Find the material before removing it so we have the FileUrl
        var material = lesson.Materials.FirstOrDefault(m => m.Id == materialId);
        if (material is null)
            return Result.Failure(DomainErrors.TheoryLesson.MaterialNotFound);

        var removeResult = lesson.RemoveMaterial(materialId);
        if (removeResult.IsFailure) return removeResult;

        // Delete file from storage — fire-and-forget to not block response
        _ = _fileStorage.DeleteAsync(material.FileUrl, cancellationToken);

        _unitOfWork.TheoryLessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result> DeleteTopicMaterialAsync(
        Guid materialId,
        CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.LessonMaterials
            .GetByIdAsync(materialId, cancellationToken);

        if (material is null)
            return Result.Failure(DomainErrors.TheoryLesson.MaterialNotFound);

        _unitOfWork.LessonMaterials.Delete(material);

        // Delete file from storage — fire-and-forget to not block response
        _ = _fileStorage.DeleteAsync(material.FileUrl, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}