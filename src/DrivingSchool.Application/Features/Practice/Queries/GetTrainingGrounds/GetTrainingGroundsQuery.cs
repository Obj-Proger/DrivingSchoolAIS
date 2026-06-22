using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetTrainingGrounds;

/// <summary>Returns all active training grounds available for booking.</summary>
public sealed record GetTrainingGroundsQuery : IQuery<IReadOnlyList<TrainingGroundDto>>;