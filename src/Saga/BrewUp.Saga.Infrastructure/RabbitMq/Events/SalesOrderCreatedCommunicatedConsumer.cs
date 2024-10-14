using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Saga.Consumers;

namespace BrewUp.Saga.Infrastructure.RabbitMq.Events;

public class SalesOrderCreatedCommunicatedConsumer(
    IRabbitMQConnectionFactory mufloneConnectionFactory,
    ILoggerFactory loggerFactory,
    IServiceBus serviceBus,
    ISagaRepository sagaRepository)
    : SagaEventConsumerBase<SalesOrderCreatedCommunicated>(mufloneConnectionFactory, loggerFactory)
{
    protected override ISagaEventHandlerAsync<SalesOrderCreatedCommunicated> HandlerAsync { get; } =
        new SalesOrderSaga(serviceBus, sagaRepository, loggerFactory);
}