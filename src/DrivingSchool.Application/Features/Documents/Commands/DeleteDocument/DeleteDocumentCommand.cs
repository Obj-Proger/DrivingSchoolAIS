using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Documents.Commands.DeleteDocument;

/// <summary>Deletes a document record and its underlying file.</summary>
public sealed record DeleteDocumentCommand(Guid DocumentId) : ICommand;