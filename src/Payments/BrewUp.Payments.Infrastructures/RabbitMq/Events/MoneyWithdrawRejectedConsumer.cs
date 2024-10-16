using BrewUp.Payments.ReadModel.EventHandlers;
using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Payments.Infrastructures.RabbitMq.Events;

public sealed class MoneyWithdrawRejectedConsumer(IEventBus eventBus,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) : DomainEventsConsumerBase<MoneyWithdrawRejected>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<MoneyWithdrawRejected>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<MoneyWithdrawRejected>>
        {
            new MoneyWithdrawnRejectedForIntegrationEventHandler(eventBus, loggerFactory)
        };
}