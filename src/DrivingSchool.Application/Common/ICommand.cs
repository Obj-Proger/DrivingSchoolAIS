namespace DrivingSchool.Application.Common;

/// <summary>
/// Marker interface for commands that do not return a value.
/// A command represents an intent to change system state.
/// Handled by an <see cref="ICommandHandler{TCommand}"/>.
/// </summary>
public interface ICommand : IRequest<Result>;

/// <summary>
/// Marker interface for commands that return a value of type <typeparamref name="TResponse"/>.
/// Handled by an <see cref="ICommandHandler{TCommand,TResponse}"/>.
/// </summary>
/// <typeparam name="TResponse">The type of value returned on success.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;