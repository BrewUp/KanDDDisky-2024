using BrewUp.Payments.SharedKernel.Events;
using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone;

namespace BrewUp.Payments.ReadModel.EventHandlers;

public sealed class MoneyWithdrawnAcceptedForIntegrationEventHandler(IEventBus eventBus,
    ILoggerFactory loggerFactory) : DomainEventHandlerBase<MoneyWithdrawnAccepted>(loggerFactory)
{
    public override async Task HandleAsync(MoneyWithdrawnAccepted @event, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        PaymentAccepted integrationEvent = new(@event.CustomerId, correlationId, @event.Amount);
        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}