using BrewUp.Shared.Messages.Sagas;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;

namespace BrewUp.Warehouses.ReadModel.EventHandlers;

public sealed class AvailabilityCheckedEventHandler(ILoggerFactory loggerFactory,
    IEventBus eventBus) : DomainEventHandlerBase<AvailabilityChecked>(loggerFactory) 
{
    public override async Task HandleAsync(AvailabilityChecked @event, CancellationToken cancellationToken = default)
    {
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        BeerAvailabilityCommunicated integrationEvent = new(@event.BeerId, correlationId, @event.Quantity);
        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}