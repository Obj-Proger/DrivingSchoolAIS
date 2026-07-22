using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Documents.Commands.UploadDocument;

/// <summary>Handles <see cref="UploadDocumentCommand"/>.</summary>
internal sealed class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand, Guid>
{
    /// <summary>
    /// Maximum accepted file size. Kept as a simple constant here since Document
    /// has no owner-specific size policy today; revisit as a configuration value
    /// if per-document-type limits are needed later.
    /// </summary>
    private const long MaxSizeBytes = 20 * 1024 * 1024; // 20 MB

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "image/jpeg",
        "image/png",
        "image/webp",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileStorageService _fileStorage;

    public UploadDocumentCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _fileStorage = fileStorage;
    }

    public async Task<Result<Guid>> Handle(
        UploadDocumentCommand command,
        CancellationToken cancellationToken)
    {
        if (!AllowedContentTypes.Contains(command.ContentType))
            return Result.Failure<Guid>(DomainErrors.Document.InvalidContentType);

        if (command.FileStream.Length > MaxSizeBytes)
            return Result.Failure<Guid>(DomainErrors.Document.FileTooLarge);

        var uploadResult = await _fileStorage.UploadAsync(
            command.FileStream,
            command.FileName,
            command.ContentType,
            folder: "documents",
            cancellationToken);

        var document = Document.Create(
            command.OwnerType,
            command.OwnerId,
            command.FileName,
            uploadResult.FileUrl,
            command.ContentType,
            uploadResult.SizeBytes,
            _currentUser.UserId);

        await _unitOfWork.Documents.AddAsync(document, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(document.Id);
    }
}