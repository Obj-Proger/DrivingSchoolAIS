using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Leads.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Queries.GetLeads;

/// <summary>
/// Handles <see cref="GetLeadsQuery"/>.
/// </summary>
internal sealed class GetLeadsQueryHandler
    : IQueryHandler<GetLeadsQuery, PaginatedResult<LeadDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLeadsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<LeadDto>>> Handle(
        GetLeadsQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Leads.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            query.Status,
            query.Source,
            query.ResponsibleManagerId,
            query.Search,
            cancellationToken);

        // Resolve manager names in a single batch query
        var managerIds = paginated.Items
            .Where(l => l.ResponsibleManagerId.HasValue)
            .Select(l => l.ResponsibleManagerId!.Value)
            .Distinct()
            .ToList();

        var managers = new Dictionary<Guid, string>();
        foreach (var managerId in managerIds)
        {
            var manager = await _unitOfWork.Users.GetByIdAsync(managerId, cancellationToken);
            if (manager is not null)
                managers[managerId] = manager.FullName.ShortName;
        }

        var dtos = paginated.Items
            .Select(l => MapToDto(l, managers))
            .ToList();

        return Result.Success(new PaginatedResult<LeadDto>(
            dtos,
            paginated.TotalCount,
            paginated.Page,
            paginated.PageSize));
    }

    internal static LeadDto MapToDto(Lead lead, Dictionary<Guid, string>? managers = null)
        => new(
            lead.Id,
            lead.FullName.DisplayName,
            lead.Phone.Value,
            lead.Email?.Value,
            lead.Source,
            lead.Status,
            lead.ResponsibleManagerId.HasValue && managers is not null
                ? managers.GetValueOrDefault(lead.ResponsibleManagerId.Value)
                : null,
            lead.ResponsibleManagerId,
            lead.CourseInterest,
            lead.Comment,
            lead.ContractId,
            lead.BranchId,
            lead.CreatedAt,
            lead.UpdatedAt);
}