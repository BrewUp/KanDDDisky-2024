using BrewUp.Sales.ReadModel.EventHandlers;
using BrewUp.Sales.ReadModel.Services;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Sales.Infrastructures.RabbitMq.Events;

public sealed class SalesOrderCreatedConsumer(
	ISalesOrderService salesOrderService,
	IEventBus eventBus,
	IRabbitMQConnectionFactory connectionFactory,
	ILoggerFactory loggerFactory)
	: DomainEventsConsumerBase<SalesOrderCreated>(connectionFactory, loggerFactory)
{
	protected override IEnumerable<IDomainEventHandlerAsync<SalesOrderCreated>> HandlersAsync { get; } = new List<DomainEventHandlerAsync<SalesOrderCreated>>
	{
		new SalesOrderCreatedEventHandlerAsync(loggerFactory, salesOrderService),
		new SalesOrderCreatedForIntegrationEventHandlerAsync(loggerFactory, eventBus)
	};
}