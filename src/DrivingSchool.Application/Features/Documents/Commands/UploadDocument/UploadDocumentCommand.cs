using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Documents.Commands.UploadDocument;

/// <summary>
/// Uploads a file and attaches it to another entity (e.g. a contract or student
/// profile), identified loosely by <paramref name="OwnerType"/> / <paramref name="OwnerId"/>
/// rather than a strongly-typed foreign key — see <c>Document</c> for why.
/// </summary>
public sealed record UploadDocumentCommand(
    string OwnerType,
    Guid OwnerId,
    Stream FileStream,
    string FileName,
    string ContentType) : ICommand<Guid>;