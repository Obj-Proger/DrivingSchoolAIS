using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Tasks.DTOs;
using DrivingSchool.Application.Features.Tasks.Queries.GetMyTasks;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Tasks.Queries.GetAllTasks;

/// <summary>Handles <see cref="GetAllTasksQuery"/>.</summary>
internal sealed class GetAllTasksQueryHandler
    : IQueryHandler<GetAllTasksQuery, PaginatedResult<StaffTaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTasksQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<StaffTaskDto>>> Handle(
        GetAllTasksQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.StaffTasks.GetAllAsync(
            query.Page,
            query.PageSize,
            query.AssignedToId,
            query.Status,
            cancellationToken);

        // Reuse mapping helper from GetMyTasks
        var handler = new GetMyTasksQueryHandler(_unitOfWork,
            null!); // mapping helper doesn't use currentUser

        var userIds = paginated.Items
            .SelectMany(t => new[] { t.AssignedToId, t.CreatedById })
            .Distinct()
            .ToList();

        var users = new Dictionary<Guid, string>();
        foreach (var id in userIds)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
            if (user is not null) users[id] = user.FullName.ShortName;
        }

        var dtos = paginated.Items.Select(t => new StaffTaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.AssignedToId,
            users.GetValueOrDefault(t.AssignedToId, "Unknown"),
            t.CreatedById,
            users.GetValueOrDefault(t.CreatedById, "Unknown"),
            t.DueDate,
            t.Status,
            t.Priority,
            t.IsRecurring,
            t.RecurringDays,
            t.LinkedEntityType,
            t.LinkedEntityId,
            t.CreatedAt)).ToList();

        return Result.Success(new PaginatedResult<StaffTaskDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }
}