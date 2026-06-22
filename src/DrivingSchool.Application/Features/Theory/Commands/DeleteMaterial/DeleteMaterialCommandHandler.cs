using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Theory.Commands.DeleteMaterial;

/// <summary>
/// Handles <see cref="DeleteMaterialCommand"/>.
/// Removes the material record and deletes the associated file from storage.
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
        var lesson = await _unitOfWork.TheoryLessons
            .GetByIdWithDetailsAsync(command.LessonId, cancellationToken);

        if (lesson is null)
            return Result.Failure(DomainErrors.TheoryLesson.NotFound);

        // Find the material before removing it so we have the FileUrl
        var material = lesson.Materials.FirstOrDefault(m => m.Id == command.MaterialId);
        if (material is null)
            return Result.Failure(DomainErrors.TheoryLesson.MaterialNotFound);

        var removeResult = lesson.RemoveMaterial(command.MaterialId);
        if (removeResult.IsFailure) return removeResult;

        // Delete file from storage — fire-and-forget to not block response
        _ = _fileStorage.DeleteAsync(material.FileUrl, cancellationToken);

        _unitOfWork.TheoryLessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}