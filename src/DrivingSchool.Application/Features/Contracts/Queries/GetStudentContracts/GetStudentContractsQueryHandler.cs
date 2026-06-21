using DrivingSchool.Application.Features.Contracts.DTOs;
using DrivingSchool.Application.Features.Contracts.Queries.GetContracts;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetStudentContracts;

/// <summary>
/// Handles <see cref="GetStudentContractsQuery"/>.
/// </summary>
internal sealed class GetStudentContractsQueryHandler
    : IQueryHandler<GetStudentContractsQuery, IReadOnlyList<ContractDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly GetContractsQueryHandler _mapper;

    public GetStudentContractsQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _mapper = new GetContractsQueryHandler(unitOfWork);
    }

    public async Task<Result<IReadOnlyList<ContractDto>>> Handle(
        GetStudentContractsQuery query,
        CancellationToken cancellationToken)
    {
        var contracts = await _unitOfWork.Contracts
            .GetByStudentIdAsync(_currentUser.UserId, cancellationToken);

        var dtos = new List<ContractDto>();
        foreach (var contract in contracts)
        {
            var dto = await _mapper.MapToDtoAsync(contract, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success<IReadOnlyList<ContractDto>>(dtos);
    }
}