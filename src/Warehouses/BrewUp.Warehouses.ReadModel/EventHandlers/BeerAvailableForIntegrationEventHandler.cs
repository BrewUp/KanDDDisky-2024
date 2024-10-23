using BrewUp.Shared.Messages.Sagas;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;

namespace BrewUp.Warehouses.ReadModel.EventHandlers;

public sealed class BeerAvailableForIntegrationEventHandler(ILoggerFactory loggerFactory,
    IEventBus eventBus) : DomainEventHandlerBase<BeerAvailable>(loggerFactory) 
{
    public override async Task HandleAsync(BeerAvailable @event, CancellationToken cancellationToken = default)
    {
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        BeerAvailableCommunicated integrationEvent = new(@event.BeerId, correlationId, @event.Quantity);
        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}