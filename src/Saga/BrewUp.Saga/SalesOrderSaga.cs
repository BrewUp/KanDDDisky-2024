using BrewUp.Saga.Messages.Commands;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace BrewUp.Saga;

public class SalesOrderSaga(IServiceBus serviceBus, ISagaRepository repository, ILoggerFactory loggerFactory)
    : Saga<SalesOrderSaga.SalesOrderSagaState>(serviceBus, repository, loggerFactory),
        ISagaStartedByAsync<StartSalesOrderSaga>,
        ISagaEventHandlerAsync<BeerAvailabilityCommunicated>
{
    public class SalesOrderSagaState
    {
        public string SagaId { get; set; } = string.Empty;
        
        public StartSalesOrderSaga StartSalesOrderSaga { get; set; } = default!;

        public int RowsChecked { get; set; } = 0;
        public bool AvailabilityChecked { get; set; }
        public bool SalesOrderCreated { get; set; }
        public bool SalesOrderProcessed { get; set; }
    }


    public async Task StartedByAsync(StartSalesOrderSaga command)
    {
        SagaState = new SalesOrderSagaState
        {
            SagaId = command.MessageId.ToString(),
            RowsChecked = 0,
            StartSalesOrderSaga = command,
            AvailabilityChecked = false,
            SalesOrderCreated = false,
            SalesOrderProcessed = false
        };
        await Repository.SaveAsync(command.MessageId, SagaState);

        foreach (var row in command.Rows)
        {
            AskForBeerAvailability rowCommand = new(new BeerId(row.BeerId), Guid.NewGuid());
            await ServiceBus.SendAsync(rowCommand, CancellationToken.None);
        }
    }

    public async Task HandleAsync(BeerAvailabilityCommunicated @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        SagaState.RowsChecked++;
        if (SagaState.RowsChecked == SagaState.StartSalesOrderSaga.Rows.Count())
        {
            SagaState.AvailabilityChecked = true;
        }
        await Repository.SaveAsync(correlationId, SagaState);
    }
}