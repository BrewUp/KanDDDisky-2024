using BrewUp.Sales.SharedKernel.Events;
using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone;

namespace BrewUp.Sales.ReadModel.EventHandlers;

public sealed class SalesOrderCreatedForIntegrationEventHandlerAsync(ILoggerFactory loggerFactory,
		IEventBus eventBus)
	: DomainEventHandlerBase<SalesOrderCreated>(loggerFactory)
{
	public override async Task HandleAsync(SalesOrderCreated @event, CancellationToken cancellationToken = new())
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		var correlationId =
			new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
		
		SalesOrderCreatedCommunicated integrationEvent = new(@event.SalesOrderId, correlationId, @event.SalesOrderNumber, 
			@event.OrderDate, @event.CustomerId, @event.CustomerName, @event.Rows);
		await eventBus.PublishAsync(integrationEvent, cancellationToken);
	}
}