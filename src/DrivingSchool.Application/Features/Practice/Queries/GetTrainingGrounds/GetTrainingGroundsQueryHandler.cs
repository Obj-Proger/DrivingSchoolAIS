using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetTrainingGrounds;

/// <summary>Handles <see cref="GetTrainingGroundsQuery"/>.</summary>
internal sealed class GetTrainingGroundsQueryHandler
    : IQueryHandler<GetTrainingGroundsQuery, IReadOnlyList<TrainingGroundDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTrainingGroundsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<TrainingGroundDto>>> Handle(
        GetTrainingGroundsQuery query,
        CancellationToken cancellationToken)
    {
        var grounds = await _unitOfWork.TrainingGrounds
            .GetAllAsync(activeOnly: true, cancellationToken);

        var dtos = grounds
            .Select(g => new TrainingGroundDto(
                g.Id,
                g.Name,
                g.Address,
                g.Description,
                g.IsActive))
            .ToList();

        return Result.Success<IReadOnlyList<TrainingGroundDto>>(dtos);
    }
}