using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Saga.Consumers;

namespace BrewUp.Saga.Infrastructure.RabbitMq.Events;

public sealed class PaymentRejectedConsumer(
    IRabbitMQConnectionFactory mufloneConnectionFactory,
    ILoggerFactory loggerFactory,
    IServiceBus serviceBus,
    ISagaRepository sagaRepository)
    : SagaEventConsumerBase<PaymentRejected>(mufloneConnectionFactory, loggerFactory)
{
    protected override ISagaEventHandlerAsync<PaymentRejected> HandlerAsync { get; } =
        new SalesOrderSaga(serviceBus, sagaRepository, loggerFactory);
}