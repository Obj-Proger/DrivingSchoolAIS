using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Lead"/> aggregate.
/// </summary>
public interface ILeadRepository
{
    /// <summary>Returns the lead with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the lead with the specified identifier including its notes,
    /// or <c>null</c> if not found.
    /// </summary>
    Task<Lead?> GetByIdWithNotesAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns a paginated, optionally filtered list of leads.</summary>
    Task<PaginatedResult<Lead>> GetPaginatedAsync(
        int page,
        int pageSize,
        LeadStatus? status = null,
        LeadSource? source = null,
        Guid? responsibleManagerId = null,
        string? search = null,
        CancellationToken ct = default);

    /// <summary>Adds a new lead to the repository.</summary>
    Task AddAsync(Lead lead, CancellationToken ct = default);

    /// <summary>Marks an existing lead as modified.</summary>
    void Update(Lead lead);

    /// <summary>
    /// Returns lead and conversion counts grouped by acquisition source,
    /// for leads created within the specified date range.
    /// </summary>
    Task<IReadOnlyList<LeadSourceStats>> GetStatsBySourceAsync(
        DateTime from,
        DateTime to,
        CancellationToken ct = default);
}

/// <summary>Lead volume and conversion count for a single acquisition source.</summary>
public sealed record LeadSourceStats(LeadSource Source, int TotalCount, int ConvertedCount);