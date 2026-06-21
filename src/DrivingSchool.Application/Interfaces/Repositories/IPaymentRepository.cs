using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Payment"/> aggregate.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>Returns the payment with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all payments for the specified contract,
    /// ordered by creation date descending.
    /// </summary>
    Task<IReadOnlyList<Payment>> GetByContractIdAsync(
        Guid contractId,
        CancellationToken ct = default);

    /// <summary>Returns a paginated, optionally filtered list of all payments.</summary>
    Task<PaginatedResult<Payment>> GetPaginatedAsync(
        int page,
        int pageSize,
        PaymentMethod? method = null,
        PaymentStatus? status = null,
        DateTime? from = null,
        DateTime? to = null,
        Guid? managerId = null,
        CancellationToken ct = default);

    /// <summary>Adds a new payment to the repository.</summary>
    Task AddAsync(Payment payment, CancellationToken ct = default);

    /// <summary>Marks an existing payment as modified.</summary>
    void Update(Payment payment);
}