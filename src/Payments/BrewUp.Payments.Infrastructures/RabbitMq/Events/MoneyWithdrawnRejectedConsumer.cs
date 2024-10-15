using BrewUp.Payments.ReadModel.EventHandlers;
using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Payments.Infrastructures.RabbitMq.Events;

public sealed class MoneyWithdrawnRejectedConsumer(IEventBus eventBus,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) : DomainEventsConsumerBase<MoneyWithdrawnRejected>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<MoneyWithdrawnRejected>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<MoneyWithdrawnRejected>>
        {
            new MoneyWithdrawnRejectedForIntegrationEventHandler(eventBus, loggerFactory)
        };
}