using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Questions.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.StartTestSession;

/// <summary>Handles <see cref="StartTestSessionCommand"/>.</summary>
internal sealed class StartTestSessionCommandHandler
    : ICommandHandler<StartTestSessionCommand, TestSessionInProgressDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public StartTestSessionCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<TestSessionInProgressDto>> Handle(
        StartTestSessionCommand command,
        CancellationToken cancellationToken)
    {
        var existingSession = await _unitOfWork.TestSessions
            .GetActiveByContractAsync(command.ContractId, cancellationToken);

        if (existingSession is not null)
            return Result.Failure<TestSessionInProgressDto>(
                DomainErrors.TestSession.ActiveSessionExists);

        var template = await _unitOfWork.TestTemplates
            .GetByIdAsync(command.TemplateId, cancellationToken);

        if (template is null)
            return Result.Failure<TestSessionInProgressDto>(DomainErrors.TestTemplate.NotFound);

        var questions = await _unitOfWork.Questions.GetRandomAsync(
            template.QuestionCount,
            template.TopicIds.Count > 0 ? template.TopicIds : null,
            template.Category,
            ct: cancellationToken);

        if (questions.Count < template.QuestionCount)
            return Result.Failure<TestSessionInProgressDto>(
                DomainErrors.TestTemplate.InsufficientQuestions);

        var session = TestSession.Start(
            command.ContractId,
            template.Id,
            questions.Select(q => q.Id),
            template.TimeLimitMinutes);

        await _unitOfWork.TestSessions.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var questionDtos = questions
            .Select(q => new TestQuestionDto(
                q.Id,
                q.Text,
                q.ImageUrl,
                q.Options.Select(o => new QuestionOptionDto(o.Id, o.Text)).ToList()))
            .ToList();

        return Result.Success(new TestSessionInProgressDto(
            session.Id,
            template.Id,
            template.Name,
            session.StartedAt,
            session.ExpiresAt,
            session.TotalQuestions,
            [],
            questionDtos));
    }
}