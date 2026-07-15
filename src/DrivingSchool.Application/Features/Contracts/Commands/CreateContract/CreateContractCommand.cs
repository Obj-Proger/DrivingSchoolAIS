using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Contracts.Commands.CreateContract;

/// <summary>
/// Creates a new training contract for a student.
/// Accessible to managers.
/// </summary>
public sealed record CreateContractCommand(
    Guid StudentId,
    Guid CourseId,
    string Number,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalCost,
    Guid? BranchId = null) : ICommand<Guid>;