using BrewUp.Saga.Messages.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Saga.Consumers;

namespace BrewUp.Saga.Infrastructure.RabbitMq.Commands;

public sealed class StartSalesOrderSagaConsumer(
    ISagaRepository repository,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory,
    IServiceBus serviceBus)
    : SagaStartedByConsumerBase<StartSalesOrderSaga>(repository, connectionFactory, loggerFactory)
{
    protected override ISagaStartedByAsync<StartSalesOrderSaga> HandlerAsync { get; } =
        new SalesOrderSaga(serviceBus, repository, loggerFactory);
}