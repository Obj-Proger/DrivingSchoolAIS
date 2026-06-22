namespace DrivingSchool.Application.Features.Practice.Commands.CreateDrivingRoute;

internal sealed class CreateDrivingRouteCommandValidator
    : AbstractValidator<CreateDrivingRouteCommand>
{
    public CreateDrivingRouteCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Route name is required.")
            .MaximumLength(200).WithMessage("Route name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Route description is required.")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}