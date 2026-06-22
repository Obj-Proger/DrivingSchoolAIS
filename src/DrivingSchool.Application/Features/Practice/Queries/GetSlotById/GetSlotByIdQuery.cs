using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetSlotById;

/// <summary>Returns the detail view of a single practice slot.</summary>
public sealed record GetSlotByIdQuery(Guid SlotId) : IQuery<PracticeSlotDetailDto>;