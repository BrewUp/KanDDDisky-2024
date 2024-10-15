using BrewUp.Payments.SharedKernel.Events;
using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone;

namespace BrewUp.Payments.ReadModel.EventHandlers;

public sealed class MoneyWithdrawnRejectedForIntegrationEventHandler(IEventBus eventBus,
    ILoggerFactory loggerFactory) : DomainEventHandlerBase<MoneyWithdrawnRejected>(loggerFactory)
{
    public override async Task HandleAsync(MoneyWithdrawnRejected @event, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        PaymentRejected integrationEvent = new(@event.CustomerId, correlationId);
        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}