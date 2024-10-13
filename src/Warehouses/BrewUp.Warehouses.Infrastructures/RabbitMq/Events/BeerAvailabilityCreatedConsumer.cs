using BrewUp.Warehouses.ReadModel.EventHandlers;
using BrewUp.Warehouses.ReadModel.Services;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq.Events;

public sealed class BeerAvailabilityCreatedConsumer(IAvailabilityService availabilityService,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) : DomainEventsConsumerBase<BeerAvailabilityCreated>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<BeerAvailabilityCreated>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<BeerAvailabilityCreated>>
        {
            new BeerAvailabilityCreatedEventHandler(loggerFactory, availabilityService)
        };
}