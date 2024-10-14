using BrewUp.Saga.Messages.Commands;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace BrewUp.Saga;

public class SalesOrderSaga(IServiceBus serviceBus, ISagaRepository repository, ILoggerFactory loggerFactory)
    : Saga<SalesOrderSaga.SalesOrderSagaState>(serviceBus, repository, loggerFactory),
        ISagaStartedByAsync<StartSalesOrderSaga>,
        ISagaEventHandlerAsync<BeerAvailabilityCommunicated>,
        ISagaEventHandlerAsync<SalesOrderCreatedCommunicated>
{
    public class SalesOrderSagaState
    {
        public string SagaId { get; set; } = string.Empty;
        
        public string SalesOrderId { get; set; } = default!;
        public SalesOrderNumber SalesOrderNumber {get; set;} = default!;
        public OrderDate OrderDate { get; set; } = default!;
        public string CustomerId { get; set; } = default!;
        public CustomerName CustomerName { get; set; } = default!;
        public IEnumerable<SalesOrderRowJson> Rows { get; set; } = default!;

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

            SalesOrderId = command.AggregateId.Value,
            SalesOrderNumber = command.SalesOrderNumber,
            OrderDate = command.OrderDate,
            CustomerId = command.CustomerId.Value,
            CustomerName = command.CustomerName,
            Rows = command.Rows,
            
            AvailabilityChecked = false,
            SalesOrderCreated = false,
            SalesOrderProcessed = false
        };
        await Repository.SaveAsync(command.MessageId, SagaState);

        foreach (var row in command.Rows)
        {
            AskForBeerAvailability rowCommand = new(new BeerId(row.BeerId), command.MessageId);
            await ServiceBus.SendAsync(rowCommand, CancellationToken.None);
        }
    }

    public async Task HandleAsync(BeerAvailabilityCommunicated @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore and Update the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        SagaState.RowsChecked++;
        if (SagaState.RowsChecked == SagaState.Rows.Count())
        {
            SagaState.AvailabilityChecked = true;
            CreateSalesOrder command = new(new SalesOrderId(new Guid(SagaState.SalesOrderId)), correlationId,
                SagaState.SalesOrderNumber, SagaState.OrderDate,
                new CustomerId(new Guid(SagaState.CustomerId)), SagaState.CustomerName,
                SagaState.Rows);
            await ServiceBus.SendAsync(command, CancellationToken.None);
        }
        await Repository.SaveAsync(correlationId, SagaState);
    }

    public async Task HandleAsync(SalesOrderCreatedCommunicated @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore and Update the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        SagaState.SalesOrderCreated = true;
        await Repository.SaveAsync(correlationId, SagaState);
    }
}