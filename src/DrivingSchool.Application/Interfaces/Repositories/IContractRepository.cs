using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Contract"/> aggregate.
/// </summary>
public interface IContractRepository
{
    /// <summary>Returns the contract with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all contracts belonging to the specified student,
    /// ordered by <see cref="Contract.SignedAt"/> descending.
    /// </summary>
    Task<IReadOnlyList<Contract>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);

    /// <summary>Returns a paginated, optionally filtered list of contracts.</summary>
    Task<PaginatedResult<Contract>> GetPaginatedAsync(
        int page,
        int pageSize,
        ContractStatus? status = null,
        Guid? studentId = null,
        bool? hasDebt = null,
        CancellationToken ct = default);

    /// <summary>
    /// Returns all active contracts with an outstanding debt greater than zero,
    /// ordered by debt amount descending.
    /// </summary>
    Task<IReadOnlyList<Contract>> GetDebtorsAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns <c>true</c> if the specified contract number is already in use.
    /// </summary>
    Task<bool> ExistsByNumberAsync(string number, CancellationToken ct = default);

    /// <summary>Adds a new contract to the repository.</summary>
    Task AddAsync(Contract contract, CancellationToken ct = default);

    /// <summary>Marks an existing contract as modified.</summary>
    void Update(Contract contract);
}