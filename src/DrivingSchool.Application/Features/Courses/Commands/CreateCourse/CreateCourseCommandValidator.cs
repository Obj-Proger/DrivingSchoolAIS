namespace DrivingSchool.Application.Features.Courses.Commands.CreateCourse;

internal sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid licence category.");

        RuleFor(x => x.TheoryHoursTotal)
            .GreaterThan(0).WithMessage("Theory hours must be greater than zero.");

        RuleFor(x => x.PracticeHoursTotal)
            .GreaterThan(0).WithMessage("Practice hours must be greater than zero.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must not be negative.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}