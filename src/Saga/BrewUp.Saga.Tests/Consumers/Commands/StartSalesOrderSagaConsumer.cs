using BrewUp.Saga.Messages.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Saga.Persistence;
using Muflone.Transport.InMemory.Abstracts;
using Muflone.Transport.InMemory.Consumers;

namespace BrewUp.Saga.Tests.Consumers.Commands;

// public class StartSalesOrderSagaConsumer(
//     IServiceBus serviceBus,
//     ISagaRepository sagaRepository) : SagaStartedByConsumerBase<StartSalesOrderSaga>(new LoggerFactory())
// {
//     protected override ISagaStartedByAsync<StartSalesOrderSaga> HandlerAsync { get; } =
//         new SalesOrderSaga(serviceBus, sagaRepository, new LoggerFactory());
// }