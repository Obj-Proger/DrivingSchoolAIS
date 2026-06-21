using DrivingSchool.Application.Features.Users.DTOs;
using DrivingSchool.Application.Features.Users.Queries.GetAllUsers;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Queries.GetStudentCard;

/// <summary>
/// Handles <see cref="GetStudentCardQuery"/>.
/// Aggregates training progress, skill ratings, and financial data
/// from all contracts held by the student.
/// </summary>
internal sealed class GetStudentCardQueryHandler
    : IQueryHandler<GetStudentCardQuery, StudentCardDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentCardQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<StudentCardDto>> Handle(
        GetStudentCardQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Load the student
        var user = await _unitOfWork.Users
            .GetByIdAsync(query.StudentId, cancellationToken);

        if (user is null)
            return Result.Failure<StudentCardDto>(DomainErrors.User.NotFound);

        // 2. Load all contracts
        var contracts = await _unitOfWork.Contracts
            .GetByStudentIdAsync(query.StudentId, cancellationToken);

        // 3. Aggregate training statistics
        var totalPracticeHours = contracts.Sum(c => c.PracticeHoursCompleted);
        var totalTheoryLessons = contracts.Sum(c => c.TheoryLessonsAttended);
        var totalDebt = contracts
            .Where(c => c.Status == ContractStatus.Active)
            .Sum(c => c.DebtAmount.Amount);

        var activeContract = contracts
            .FirstOrDefault(c => c.Status == ContractStatus.Active);

        // 4. Aggregate skill ratings from completed bookings
        var skillSummaries = new List<SkillSummaryDto>();

        foreach (var contract in contracts)
        {
            var bookings = await _unitOfWork.PracticeBookings
                .GetByContractIdAsync(contract.Id, BookingStatus.Completed, cancellationToken);

            var allRatings = bookings
                .SelectMany(b => b.SkillRatings)
                .GroupBy(r => r.SkillName)
                .Select(g => new SkillSummaryDto(
                    SkillName: g.Key,
                    AverageScore: Math.Round(g.Average(r => r.Score), 1),
                    AssessmentCount: g.Count()))
                .ToList();

            // Merge with existing summaries
            foreach (var skill in allRatings)
            {
                var existing = skillSummaries.FirstOrDefault(s => s.SkillName == skill.SkillName);
                if (existing is null)
                {
                    skillSummaries.Add(skill);
                }
                else
                {
                    var mergedAvg = Math.Round(
                        (existing.AverageScore * existing.AssessmentCount +
                         skill.AverageScore * skill.AssessmentCount) /
                        (existing.AssessmentCount + skill.AssessmentCount), 1);

                    skillSummaries[skillSummaries.IndexOf(existing)] = existing with
                    {
                        AverageScore = mergedAvg,
                        AssessmentCount = existing.AssessmentCount + skill.AssessmentCount
                    };
                }
            }
        }

        return Result.Success(new StudentCardDto(
            User: GetAllUsersQueryHandler.MapToDto(user),
            ActiveContractId: activeContract?.Id,
            TotalPracticeHoursCompleted: totalPracticeHours,
            TotalTheoryLessonsAttended: totalTheoryLessons,
            TotalDebtAmount: totalDebt,
            QualityIndicator: activeContract?.QualityIndicator,
            SkillSummaries: skillSummaries));
    }
}