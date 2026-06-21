using DrivingSchool.Application.Features.Leads.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Queries.GetLeadById;

/// <summary>
/// Handles <see cref="GetLeadByIdQuery"/>.
/// </summary>
internal sealed class GetLeadByIdQueryHandler
    : IQueryHandler<GetLeadByIdQuery, LeadDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLeadByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<LeadDetailDto>> Handle(
        GetLeadByIdQuery query,
        CancellationToken cancellationToken)
    {
        var lead = await _unitOfWork.Leads
            .GetByIdWithNotesAsync(query.LeadId, cancellationToken);

        if (lead is null)
            return Result.Failure<LeadDetailDto>(DomainErrors.Lead.NotFound);

        // Resolve author names for notes
        var authorIds = lead.Notes.Select(n => n.AuthorId).Distinct().ToList();
        var authors = new Dictionary<Guid, string>();

        foreach (var authorId in authorIds)
        {
            var author = await _unitOfWork.Users.GetByIdAsync(authorId, cancellationToken);
            if (author is not null)
                authors[authorId] = author.FullName.ShortName;
        }

        string? managerName = null;
        if (lead.ResponsibleManagerId.HasValue)
        {
            var manager = await _unitOfWork.Users
                .GetByIdAsync(lead.ResponsibleManagerId.Value, cancellationToken);
            managerName = manager?.FullName.ShortName;
        }

        var noteDtos = lead.Notes
            .OrderBy(n => n.CreatedAt)
            .Select(n => new LeadNoteDto(
                n.Id,
                n.Text,
                n.AuthorId,
                authors.GetValueOrDefault(n.AuthorId, "Unknown"),
                n.CreatedAt))
            .ToList();

        return Result.Success(new LeadDetailDto(
            lead.Id,
            lead.FullName.DisplayName,
            lead.Phone.Value,
            lead.Email?.Value,
            lead.Source,
            lead.Status,
            managerName,
            lead.ResponsibleManagerId,
            lead.CourseInterest,
            lead.Comment,
            lead.ContractId,
            noteDtos,
            lead.CreatedAt,
            lead.UpdatedAt));
    }
}