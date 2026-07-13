namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Coordinates multiple repository operations within a single transaction.
/// Exposes all repositories as properties so handlers work with a single injected dependency.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    IUserRepository Users { get; }
    ILeadRepository Leads { get; }
    IContractRepository Contracts { get; }
    IGroupRepository Groups { get; }
    ITheoryLessonRepository TheoryLessons { get; }
    IPracticeSlotRepository PracticeSlots { get; }
    IPracticeBookingRepository PracticeBookings { get; }
    IQuestionRepository Questions { get; }
    ITestSessionRepository TestSessions { get; }
    IExamEventRepository ExamEvents { get; }
    IChatRepository Chats { get; }
    IPaymentRepository Payments { get; }
    INotificationRepository Notifications { get; }
    IStaffTaskRepository StaffTasks { get; }
    IBranchRepository Branches { get; }

    /// <summary>
    /// Persists all pending changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>Begins a database transaction.</summary>
    Task BeginTransactionAsync(CancellationToken ct = default);

    /// <summary>Commits the current transaction.</summary>
    Task CommitTransactionAsync(CancellationToken ct = default);

    /// <summary>Rolls back the current transaction.</summary>
    Task RollbackTransactionAsync(CancellationToken ct = default);
}