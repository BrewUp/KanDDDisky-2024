using BrewUp.Payments.ReadModel.EventHandlers;
using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Payments.Infrastructures.RabbitMq.Events;

public sealed class MoneyDepositedConsumer(ISavingsAccountService savingsAccountService,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) : DomainEventsConsumerBase<MoneyDeposited>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<MoneyDeposited>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<MoneyDeposited>>
        {
            new MoneyDepositedEventHandler(savingsAccountService, loggerFactory)
        };
}