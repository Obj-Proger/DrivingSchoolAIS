using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Tasks.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Tasks.Queries.GetMyTasks;

/// <summary>Handles <see cref="GetMyTasksQuery"/>.</summary>
internal sealed class GetMyTasksQueryHandler
    : IQueryHandler<GetMyTasksQuery, PaginatedResult<StaffTaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetMyTasksQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<PaginatedResult<StaffTaskDto>>> Handle(
        GetMyTasksQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.StaffTasks.GetByAssigneeAsync(
            _currentUser.UserId,
            query.Page,
            query.PageSize,
            query.Status,
            cancellationToken);

        var dtos = await MapToDtosAsync(paginated.Items, cancellationToken);

        return Result.Success(new PaginatedResult<StaffTaskDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }

    private async Task<List<StaffTaskDto>> MapToDtosAsync(
        IReadOnlyList<StaffTask> tasks,
        CancellationToken ct)
    {
        var userIds = tasks
            .SelectMany(t => new[] { t.AssignedToId, t.CreatedById })
            .Distinct()
            .ToList();

        var users = new Dictionary<Guid, string>();
        foreach (var id in userIds)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, ct);
            if (user is not null) users[id] = user.FullName.ShortName;
        }

        return tasks.Select(t => new StaffTaskDto(
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
    }
}