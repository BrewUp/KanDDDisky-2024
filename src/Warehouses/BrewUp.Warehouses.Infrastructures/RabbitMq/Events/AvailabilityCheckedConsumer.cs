using BrewUp.Warehouses.ReadModel.EventHandlers;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq.Events;

public sealed class AvailabilityCheckedConsumer(IEventBus eventBus,
    IRabbitMQConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
    : DomainEventsConsumerBase<AvailabilityChecked>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<AvailabilityChecked>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<AvailabilityChecked>>
        {
            new AvailabilityCheckedEventHandler(loggerFactory, eventBus)
        };
}