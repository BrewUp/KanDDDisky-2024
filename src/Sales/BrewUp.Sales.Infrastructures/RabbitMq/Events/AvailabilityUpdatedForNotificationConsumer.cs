using BrewUp.Sales.Acl;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Sales.Infrastructures.RabbitMq.Events;

public sealed class AvailabilityUpdatedForNotificationConsumer(IServiceBus serviceBus,
	IRabbitMQConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
	: IntegrationEventsConsumerBase<AvailabilityUpdatedForNotification>(connectionFactory, loggerFactory)
{
	protected override IEnumerable<IIntegrationEventHandlerAsync<AvailabilityUpdatedForNotification>> HandlersAsync { get; } = new List<IIntegrationEventHandlerAsync<AvailabilityUpdatedForNotification>>
	{
		new AvailabilityUpdatedForNotificationEventHandler(loggerFactory, serviceBus)
	};
}