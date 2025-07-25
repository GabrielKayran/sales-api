using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Services;

/// <summary>
/// Implementation of domain event dispatcher using MediatR
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task DispatchEventsAsync(BaseEntity entity, CancellationToken cancellationToken = default)
    {
        var domainEvents = entity.DomainEvents.ToList();
        
        if (!domainEvents.Any())
            return;

        _logger.LogInformation("Dispatching {EventCount} domain events for entity {EntityType} with ID {EntityId}", 
            domainEvents.Count, entity.GetType().Name, entity.Id);

        // Clear events before publishing to prevent re-processing
        entity.ClearDomainEvents();

        // Publish all events
        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await _mediator.Publish(domainEvent, cancellationToken);
                _logger.LogDebug("Successfully published domain event {EventType}", domainEvent.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing domain event {EventType}: {ErrorMessage}", 
                    domainEvent.GetType().Name, ex.Message);
                throw;
            }
        }
    }

    public async Task DispatchEventsAsync(IEnumerable<BaseEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DispatchEventsAsync(entity, cancellationToken);
        }
    }
}
