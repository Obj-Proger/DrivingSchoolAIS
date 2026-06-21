namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="User"/> aggregate.
/// </summary>
public interface IUserRepository
{
    /// <summary>Returns the user with the specified identifier, or <c>null</c> if not found.</summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns the active user with the specified email, or <c>null</c> if not found.</summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Returns the user whose refresh token collection contains a token
    /// matching the specified SHA-256 hash, or <c>null</c> if not found.
    /// </summary>
    Task<User?> GetByRefreshTokenHashAsync(string tokenHash, CancellationToken ct = default);

    /// <summary>Returns a paginated, optionally filtered list of users.</summary>
    /// <param name="page">One-based page number.</param>
    /// <param name="pageSize">Items per page.</param>
    /// <param name="search">Optional search term matched against name, email, or phone.</param>
    /// <param name="role">Optional role filter.</param>
    /// <param name="isActive">Optional active-status filter.</param>
    Task<PaginatedResult<User>> GetPaginatedAsync(
        int page,
        int pageSize,
        string? search = null,
        UserRole? role = null,
        bool? isActive = null,
        CancellationToken ct = default);

    /// <summary>Returns all users with the <see cref="UserRole.Instructor"/> role.</summary>
    Task<IReadOnlyList<User>> GetInstructorsAsync(CancellationToken ct = default);

    /// <summary>Returns all users with the specified role.</summary>
    Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken ct = default);

    /// <summary>Returns <c>true</c> if any active user has the specified email address.</summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>Adds a new user to the repository.</summary>
    Task AddAsync(User user, CancellationToken ct = default);

    /// <summary>Marks an existing user as modified.</summary>
    void Update(User user);

    /// <summary>
    /// Returns the user whose email confirmation token matches the specified value,
    /// or <c>null</c> if not found or already confirmed.
    /// </summary>
    Task<User?> GetByEmailConfirmationTokenAsync(string token, CancellationToken ct = default);

    /// <summary>
    /// Returns the user whose password reset token matches the specified value,
    /// or <c>null</c> if not found or expired.
    /// </summary>
    Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct = default);
}