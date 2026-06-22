using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Commands.MarkAttendance;

/// <summary>
/// Handles <see cref="MarkAttendanceCommand"/>.
/// Marks attendance for each submitted contract and increments
/// the theory lessons counter on each corresponding contract.
/// </summary>
internal sealed class MarkAttendanceCommandHandler : ICommandHandler<MarkAttendanceCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkAttendanceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        MarkAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.TheoryLessons
            .GetByIdWithDetailsAsync(command.LessonId, cancellationToken);

        if (lesson is null)
            return Result.Failure(DomainErrors.TheoryLesson.NotFound);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        foreach (var record in command.Records)
        {
            // Mark attendance on the lesson aggregate
            var markResult = lesson.MarkAttendance(
                record.ContractId,
                record.IsPresent,
                record.Note);

            if (markResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return markResult;
            }

            // Increment counter on contract when present
            if (record.IsPresent)
            {
                var contract = await _unitOfWork.Contracts
                    .GetByIdAsync(record.ContractId, cancellationToken);

                if (contract is not null)
                {
                    contract.RegisterTheoryAttendance();
                    _unitOfWork.Contracts.Update(contract);
                }
            }
        }

        // Mark lesson as completed if all students have been recorded
        if (lesson.Status == LessonStatus.Scheduled)
        {
            lesson.Complete();
        }

        _unitOfWork.TheoryLessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}