using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Documents.Commands.DeleteDocument;

/// <summary>Handles <see cref="DeleteDocumentCommand"/>.</summary>
internal sealed class DeleteDocumentCommandHandler : ICommandHandler<DeleteDocumentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorage;

    public DeleteDocumentCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _fileStorage = fileStorage;
    }

    public async Task<Result> Handle(
        DeleteDocumentCommand command,
        CancellationToken cancellationToken)
    {
        var document = await _unitOfWork.Documents
            .GetByIdAsync(command.DocumentId, cancellationToken);

        if (document is null)
            return Result.Failure(DomainErrors.Document.NotFound);

        _unitOfWork.Documents.Delete(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Delete the file from storage — fire-and-forget to not block the response
        _ = _fileStorage.DeleteAsync(document.FileUrl, cancellationToken);

        return Result.Success();
    }
}