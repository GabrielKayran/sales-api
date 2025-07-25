using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Interface for dispatching domain events
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches all domain events from the entity
    /// </summary>
    /// <param name="entity">Entity containing domain events</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DispatchEventsAsync(BaseEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches all domain events from multiple entities
    /// </summary>
    /// <param name="entities">Entities containing domain events</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DispatchEventsAsync(IEnumerable<BaseEntity> entities, CancellationToken cancellationToken = default);
}
