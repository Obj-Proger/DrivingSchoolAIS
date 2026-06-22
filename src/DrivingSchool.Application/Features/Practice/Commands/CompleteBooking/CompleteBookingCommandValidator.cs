namespace DrivingSchool.Application.Features.Practice.Commands.CompleteBooking;

internal sealed class CompleteBookingCommandValidator
    : AbstractValidator<CompleteBookingCommand>
{
    public CompleteBookingCommandValidator()
    {
        RuleFor(x => x.PracticeHoursLogged)
            .GreaterThan(0).WithMessage("Practice hours must be greater than zero.")
            .LessThanOrEqualTo(8).WithMessage("Practice hours must not exceed 8.");

        RuleForEach(x => x.SkillRatings)
            .ChildRules(rating =>
            {
                rating.RuleFor(r => r.SkillName)
                    .NotEmpty().WithMessage("Skill name is required.")
                    .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters.");

                rating.RuleFor(r => r.Score)
                    .InclusiveBetween(1, 5).WithMessage("Score must be between 1 and 5.");
            });

        RuleFor(x => x.InstructorNote)
            .MaximumLength(1000).WithMessage("Instructor note must not exceed 1000 characters.")
            .When(x => x.InstructorNote is not null);
    }
}