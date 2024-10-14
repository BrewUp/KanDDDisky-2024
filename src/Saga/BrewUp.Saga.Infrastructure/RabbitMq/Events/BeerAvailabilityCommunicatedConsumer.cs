using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Saga.Consumers;

namespace BrewUp.Saga.Infrastructure.RabbitMq.Events;

public sealed class BeerAvailabilityCommunicatedConsumer(
    IRabbitMQConnectionFactory mufloneConnectionFactory,
    ILoggerFactory loggerFactory,
    IServiceBus serviceBus,
    ISagaRepository sagaRepository)
    : SagaEventConsumerBase<BeerAvailabilityCommunicated>(mufloneConnectionFactory, loggerFactory)
{
    protected override ISagaEventHandlerAsync<BeerAvailabilityCommunicated> HandlerAsync { get; } =
        new SalesOrderSaga(serviceBus, sagaRepository, loggerFactory);
}