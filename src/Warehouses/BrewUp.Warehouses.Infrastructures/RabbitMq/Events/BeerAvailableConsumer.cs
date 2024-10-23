using BrewUp.Warehouses.ReadModel.EventHandlers;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq.Events;

public sealed class BeerAvailableConsumer(IEventBus eventBus,
    IRabbitMQConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
    : DomainEventsConsumerBase<BeerAvailable>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<BeerAvailable>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<BeerAvailable>>
        {
            new BeerAvailableForIntegrationEventHandler(loggerFactory, eventBus)
        };
}