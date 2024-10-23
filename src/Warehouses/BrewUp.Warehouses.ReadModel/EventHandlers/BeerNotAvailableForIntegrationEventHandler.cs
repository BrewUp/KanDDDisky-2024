using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Messages.Sagas;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;

namespace BrewUp.Warehouses.ReadModel.EventHandlers;

public sealed class BeerNotAvailableForIntegrationEventHandler(ILoggerFactory loggerFactory,
    IEventBus eventBus) : DomainEventHandlerBase<BeerNotAvailable>(loggerFactory)
{
    public override async Task HandleAsync(BeerNotAvailable @event, CancellationToken cancellationToken = default)
    {
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        BeerNotAvailableCommunicated integrationEvent = new(@event.BeerId, correlationId, new Quantity(-1, string.Empty));
        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}