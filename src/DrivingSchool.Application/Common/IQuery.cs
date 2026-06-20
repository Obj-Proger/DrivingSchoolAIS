namespace DrivingSchool.Application.Common;

/// <summary>
/// Marker interface for queries that return a value of type <typeparamref name="TResponse"/>.
/// A query represents a request for data that does not alter system state.
/// Handled by an <see cref="IQueryHandler{TQuery,TResponse}"/>.
/// </summary>
/// <typeparam name="TResponse">The type of data returned.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;