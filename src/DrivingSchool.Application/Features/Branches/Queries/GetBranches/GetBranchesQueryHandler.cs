using DrivingSchool.Application.Features.Branches.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Branches.Queries.GetBranches;

/// <summary>Handles <see cref="GetBranchesQuery"/>.</summary>
internal sealed class GetBranchesQueryHandler
    : IQueryHandler<GetBranchesQuery, IReadOnlyList<BranchDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBranchesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<BranchDto>>> Handle(
        GetBranchesQuery query,
        CancellationToken cancellationToken)
    {
        var branches = await _unitOfWork.Branches
            .GetAllAsync(query.ActiveOnly, cancellationToken);

        var dtos = branches
            .Select(b => new BranchDto(
                b.Id,
                b.Name,
                b.City,
                b.Address,
                b.Phone.Value,
                b.IsActive))
            .OrderBy(b => b.Name)
            .ToList();

        return Result.Success<IReadOnlyList<BranchDto>>(dtos);
    }
}