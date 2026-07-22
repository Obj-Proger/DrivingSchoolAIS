namespace DrivingSchool.Application.Features.Practice.Commands.UpdateDrivingRoute;

internal sealed class UpdateDrivingRouteCommandValidator
    : AbstractValidator<UpdateDrivingRouteCommand>
{
    public UpdateDrivingRouteCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Route name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}