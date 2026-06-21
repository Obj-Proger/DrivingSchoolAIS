using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.AddLeadNote;

/// <summary>Handles <see cref="AddLeadNoteCommand"/>.</summary>
internal sealed class AddLeadNoteCommandHandler : ICommandHandler<AddLeadNoteCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddLeadNoteCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        AddLeadNoteCommand command,
        CancellationToken cancellationToken)
    {
        var lead = await _unitOfWork.Leads.GetByIdAsync(command.LeadId, cancellationToken);
        if (lead is null) return Result.Failure(DomainErrors.Lead.NotFound);

        lead.AddNote(command.Text, command.AuthorId);

        _unitOfWork.Leads.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}