namespace DrivingSchool.Application.Features.Chats.Commands.UpdateGroupChat;

internal sealed class UpdateGroupChatCommandValidator : AbstractValidator<UpdateGroupChatCommand>
{
    public UpdateGroupChatCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Chat name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}