namespace DrivingSchool.Application.Common;

/// <summary>
/// Encapsulates a single page of results from a paginated query.
/// </summary>
/// <typeparam name="T">The type of the items in the page.</typeparam>
/// <param name="Items">The items in the current page.</param>
/// <param name="TotalCount">The total number of items across all pages.</param>
/// <param name="Page">The current one-based page number.</param>
/// <param name="PageSize">The number of items per page.</param>
public sealed record PaginatedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    /// <summary>Gets the total number of pages.</summary>
    public int TotalPages => PageSize > 0
        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
        : 0;

    /// <summary>Gets a value indicating whether a next page exists.</summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>Gets a value indicating whether a previous page exists.</summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>Creates an empty paginated result.</summary>
    public static PaginatedResult<T> Empty(int page = 1, int pageSize = 20)
        => new([], 0, page, pageSize);
}