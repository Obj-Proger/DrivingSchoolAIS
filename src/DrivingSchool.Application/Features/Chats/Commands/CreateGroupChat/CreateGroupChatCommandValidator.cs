namespace DrivingSchool.Application.Features.Chats.Commands.CreateGroupChat;

internal sealed class CreateGroupChatCommandValidator : AbstractValidator<CreateGroupChatCommand>
{
    public CreateGroupChatCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Chat name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Type)
            .NotEqual(ChatType.Direct)
                .WithMessage("Use CreateDirectChat to start a one-on-one conversation.");
    }
}