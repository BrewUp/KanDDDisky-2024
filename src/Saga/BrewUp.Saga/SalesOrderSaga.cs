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
    : Saga<StartSalesOrderSaga, SalesOrderSaga.SalesOrderSagaState>(serviceBus, repository, loggerFactory)
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

    #region Dispose

    public void Dispose()
    {
        loggerFactory.Dispose();
    }
    #endregion
}