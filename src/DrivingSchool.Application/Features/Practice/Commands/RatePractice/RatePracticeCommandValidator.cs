namespace DrivingSchool.Application.Features.Practice.Commands.RatePractice;

internal sealed class RatePracticeCommandValidator : AbstractValidator<RatePracticeCommand>
{
    public RatePracticeCommandValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Review)
            .MaximumLength(500).WithMessage("Review must not exceed 500 characters.")
            .When(x => x.Review is not null);
    }
}