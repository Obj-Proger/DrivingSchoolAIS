using DrivingSchool.Application.Features.Documents.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Documents.Queries.GetDocuments;

/// <summary>Handles <see cref="GetDocumentsQuery"/>.</summary>
internal sealed class GetDocumentsQueryHandler
    : IQueryHandler<GetDocumentsQuery, IReadOnlyList<DocumentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDocumentsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<DocumentDto>>> Handle(
        GetDocumentsQuery query,
        CancellationToken cancellationToken)
    {
        var documents = await _unitOfWork.Documents
            .GetByOwnerAsync(query.OwnerType, query.OwnerId, cancellationToken);

        var dtos = new List<DocumentDto>();
        foreach (var document in documents)
        {
            var uploader = await _unitOfWork.Users
                .GetByIdAsync(document.UploadedById, cancellationToken);

            dtos.Add(new DocumentDto(
                document.Id,
                document.OwnerType,
                document.OwnerId,
                document.FileName,
                document.FileUrl,
                document.ContentType,
                document.SizeBytes,
                document.UploadedById,
                uploader?.FullName.ShortName ?? "Unknown",
                document.CreatedAt));
        }

        return Result.Success<IReadOnlyList<DocumentDto>>(dtos);
    }
}