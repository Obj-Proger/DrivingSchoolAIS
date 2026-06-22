using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Theory.Commands.UploadMaterial;

/// <summary>
/// Handles <see cref="UploadMaterialCommand"/>.
/// Uploads the file to storage and attaches the resulting record
/// to either a lesson or a topic.
/// </summary>
internal sealed class UploadMaterialCommandHandler
    : ICommandHandler<UploadMaterialCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorage;

    public UploadMaterialCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _fileStorage = fileStorage;
    }

    public async Task<Result<Guid>> Handle(
        UploadMaterialCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Upload file
        var uploadResult = await _fileStorage.UploadAsync(
            command.FileStream,
            command.FileName,
            command.ContentType,
            folder: "materials",
            cancellationToken);

        // 2. Create material entity and attach to lesson or topic
        LessonMaterial material;

        if (command.LessonId.HasValue)
        {
            var lesson = await _unitOfWork.TheoryLessons
                .GetByIdAsync(command.LessonId.Value, cancellationToken);

            if (lesson is null)
                return Result.Failure<Guid>(DomainErrors.TheoryLesson.NotFound);

            material = LessonMaterial.ForLesson(
                command.LessonId.Value,
                command.Title,
                uploadResult.FileUrl,
                uploadResult.ContentType,
                uploadResult.SizeBytes,
                command.IsPublic);

            var addResult = lesson.AddMaterial(material);
            if (addResult.IsFailure) return Result.Failure<Guid>(addResult.Error);

            _unitOfWork.TheoryLessons.Update(lesson);
        }
        else
        {
            material = LessonMaterial.ForTopic(
                command.TopicId!.Value,
                command.Title,
                uploadResult.FileUrl,
                uploadResult.ContentType,
                uploadResult.SizeBytes);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(material.Id);
    }
}