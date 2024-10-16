using BrewUp.Saga.Messages.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Saga.Persistence;
using Muflone.Transport.InMemory.Consumers;

namespace BrewUp.Saga.Tests.Consumers.Commands;

public class StartSalesOrderSagaConsumer(
    IServiceBus serviceBus,
    ISagaRepository sagaRepository) : CommandConsumerBase<StartSalesOrderSaga>(new LoggerFactory())
{
    //protected override ICommandHandlerAsync<StartSalesOrderSaga> HandlerAsync { get; } = new SalesOrderSaga(serviceBus, sagaRepository, new NullLoggerFactory());
    protected override ICommandHandlerAsync<StartSalesOrderSaga> HandlerAsync { get; }
}