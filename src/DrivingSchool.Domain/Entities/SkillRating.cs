using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Records an instructor's assessment of a specific driving skill
/// observed during a practice lesson.
/// Owned by <see cref="PracticeBooking"/> and immutable after creation.
/// </summary>
public sealed class SkillRating
{
    private SkillRating() { } // Required by EF Core

    /// <summary>Gets the identifier of the booking this rating belongs to.</summary>
    public Guid BookingId { get; private set; }

    /// <summary>
    /// Gets the name of the skill being assessed
    /// (e.g. "Parallel Parking", "Lane Changing", "Emergency Braking").
    /// </summary>
    public string SkillName { get; private set; } = string.Empty;

    /// <summary>Gets the score awarded for this skill on a scale of 1 to 5.</summary>
    public int Score { get; private set; }

    /// <summary>
    /// Creates a new skill rating.
    /// </summary>
    /// <param name="bookingId">The parent booking identifier.</param>
    /// <param name="skillName">The name of the assessed skill.</param>
    /// <param name="score">The score (1–5).</param>
    /// <returns>
    /// A successful <see cref="Result{SkillRating}"/>,
    /// or a failure with <see cref="DomainErrors.PracticeBooking.InvalidRating"/>
    /// or <see cref="DomainErrors.PracticeBooking.InvalidSkillName"/>.
    /// </returns>
    public static Result<SkillRating> Create(Guid bookingId, string skillName, int score)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            return Result.Failure<SkillRating>(DomainErrors.PracticeBooking.InvalidSkillName);

        if (score is < 1 or > 5)
            return Result.Failure<SkillRating>(DomainErrors.PracticeBooking.InvalidRating);

        return Result.Success(new SkillRating
        {
            BookingId = bookingId,
            SkillName = skillName.Trim(),
            Score = score
        });
    }
}