using BrewUp.Warehouses.ReadModel.EventHandlers;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq.Events;

public sealed class BeerNotAvailableConsumer(IEventBus eventBus,
    IRabbitMQConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
    : DomainEventsConsumerBase<BeerNotAvailable>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<BeerNotAvailable>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<BeerNotAvailable>>
        {
            new BeerNotAvailableForIntegrationEventHandler(loggerFactory, eventBus)
        };
}