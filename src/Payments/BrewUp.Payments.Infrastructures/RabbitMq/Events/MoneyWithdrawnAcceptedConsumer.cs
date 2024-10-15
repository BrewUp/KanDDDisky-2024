using BrewUp.Payments.ReadModel.EventHandlers;
using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Payments.Infrastructures.RabbitMq.Events;

public sealed class MoneyWithdrawnAcceptedConsumer(ISavingsAccountService savingsAccountService,
    IEventBus eventBus,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) : DomainEventsConsumerBase<MoneyWithdrawnAccepted>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<MoneyWithdrawnAccepted>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<MoneyWithdrawnAccepted>>
        {
            new MoneyWithdrawnAcceptedEventHandler(savingsAccountService, loggerFactory),
            new MoneyWithdrawnAcceptedForIntegrationEventHandler(eventBus, loggerFactory)
        };
}