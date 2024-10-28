using BrewUp.Payments.SharedKernel.Commands;
using BrewUp.Saga.Messages.Commands;
using BrewUp.Saga.Models;
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
    : Saga<StartSalesOrderSaga, SalesOrderSaga.SalesOrderSagaState>(serviceBus, repository, loggerFactory),
        ISagaEventHandlerAsync<BeerAvailableCommunicated>,
        ISagaEventHandlerAsync<BeerNotAvailableCommunicated>,
        ISagaEventHandlerAsync<SalesOrderCreatedCommunicated>,
        ISagaEventHandlerAsync<PaymentAccepted>,
        ISagaEventHandlerAsync<PaymentRejected>
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
        public IEnumerable<BeerAvailabilities> Availabilities { get; set; } = [];
            

        public int RowsChecked { get; set; } = 0;
        public bool AvailabilityChecked { get; set; }
        public bool SalesOrderCreated { get; set; }
        public bool SalesOrderProcessed { get; set; }
        
        public bool PaymentAccepted { get; set; }
        public bool PaymentRejected { get; set; }
        
        public bool SagaFailed { get; set; }
    }

    public override async Task StartedByAsync(StartSalesOrderSaga command)
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
            
            Availabilities = command.Rows.Select(r => new BeerAvailabilities(r.BeerId, r.Quantity, new Quantity(0, string.Empty))),
            
            AvailabilityChecked = false,
            SalesOrderCreated = false,
            SalesOrderProcessed = false,
            
            SagaFailed = false
        };
        await Repository.SaveAsync(command.MessageId, SagaState);

        foreach (var row in command.Rows)
        {
            AskForBeerAvailability rowCommand = new(new BeerId(row.BeerId), command.MessageId, row.Quantity);
            await ServiceBus.SendAsync(rowCommand, CancellationToken.None);
        }
    }

    public async Task HandleAsync(BeerAvailableCommunicated @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore and Update the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        
        if (SagaState.AvailabilityChecked || SagaState.SagaFailed)
            return;
        
        SagaState.RowsChecked++;
        
        var row = SagaState.Availabilities.FirstOrDefault(a => a.BeerId.ToString() == @event.BeerId.Value);
        if (row != null)
        {
            // Replace row with updated availability
            SagaState.Availabilities = SagaState.Availabilities.Where(b => b.BeerId.ToString() != @event.BeerId.Value)
                .ToList();
            SagaState.Availabilities = SagaState.Availabilities.Concat(new List<BeerAvailabilities>
            {
                row with {Availability = @event.Availability}
            });
        }

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
    
    public async Task HandleAsync(BeerNotAvailableCommunicated @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore and Update the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        
        if (SagaState.AvailabilityChecked || SagaState.SagaFailed)
            return;
        
        SagaState.SagaFailed = true;
        await Repository.SaveAsync(correlationId, SagaState);
        
        // Command to NotificationBoundedContext to notify the user
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
        
        WithdrawMoney command = new(new CustomerId(new Guid(SagaState.CustomerId)), correlationId,
            SagaState.CustomerName, new Amount(SagaState.Rows.Sum(r => r.Price.Value * r.Quantity.Value), "EUR"));
        await ServiceBus.SendAsync(command, CancellationToken.None);
    }

    public async Task HandleAsync(PaymentAccepted @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore and Update the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        SagaState.PaymentAccepted = true;
        await Repository.CompleteAsync(correlationId);
    }

    public async Task HandleAsync(PaymentRejected @event)
    {
        // Read correlationId from the event
        var correlationId =
            new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
        
        // Restore and Update the saga state
        SagaState = await Repository.GetByIdAsync<SalesOrderSagaState>(correlationId);
        SagaState.PaymentRejected = true;
        await Repository.SaveAsync(correlationId, SagaState);

        foreach (var row in SagaState.Rows)
        {
            RestoreCommittedForSale command = new(new BeerId(row.BeerId), correlationId, row.Quantity);
            await ServiceBus.SendAsync(command, CancellationToken.None);
        }
    }

    #region Dispose

    public void Dispose()
    {
        loggerFactory.Dispose();
    }
    #endregion
}