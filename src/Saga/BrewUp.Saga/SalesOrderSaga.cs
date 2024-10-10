using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace BrewUp.Saga;

public class SalesOrderSaga(IServiceBus serviceBus, ISagaRepository repository, ILoggerFactory loggerFactory)
    : Saga<SalesOrderSaga.SalesOrderSagaState>(serviceBus, repository, loggerFactory)
{
    public class SalesOrderSagaState
    {
        public string SagaId { get; set; } = string.Empty;
        
        public bool AvailabilityChecked { get; set; }
        public bool SalesOrderCreated { get; set; }
        public bool SalesOrderProcessed { get; set; }
    }
}