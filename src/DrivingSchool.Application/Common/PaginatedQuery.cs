namespace DrivingSchool.Application.Common;

/// <summary>
/// Base record for paginated queries.
/// Provides standard pagination and sorting parameters
/// inherited by all list-returning query types.
/// </summary>
/// <param name="Page">The one-based page number. Defaults to 1.</param>
/// <param name="PageSize">The number of items per page. Defaults to 20.</param>
/// <param name="SortBy">The property name to sort by, or <c>null</c> for default ordering.</param>
/// <param name="SortDescending">Whether to sort in descending order.</param>
public record PaginatedQuery(
    int Page = 1,
    int PageSize = 20,
    string? SortBy = null,
    bool SortDescending = false);