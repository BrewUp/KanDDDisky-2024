using BrewUp.Saga.Messages.Commands;
using BrewUp.Saga.Tests.Persistence;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Microsoft.Extensions.DependencyInjection;
using Muflone;
using Muflone.Persistence;
using Muflone.Saga.Persistence;
using Muflone.Transport.InMemory;

namespace BrewUp.Saga.Tests;

public class SalesOrderSagaTest
{
    private readonly SalesOrderId _salesOrderId = new(Guid.NewGuid());
    private readonly SalesOrderNumber _salesOrderNumber = new("20241016-1200");
    private readonly OrderDate _orderDate = new(DateTime.UtcNow);
    
    private readonly CustomerId _customerId = new(Guid.NewGuid());
    private readonly CustomerName _customerName = new("Muflone");
    
    private readonly BeerId _beerId = new(Guid.NewGuid());
    private readonly BeerName _beerName = new("Muflone IPA");
    private readonly Quantity _quantity = new(30, "Lt");
    private readonly Price _price = new(3.5m, "€");

    private readonly Guid _correlationId = Guid.NewGuid();
    //private IServiceBus _serviceBus = new InMe

    public SalesOrderSagaTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<ISagaRepository, InMemorySagaRepository>();
        services.AddSingleton<IRepository, InMemoryEventRepository>();
        services.AddMufloneTransportInMemory(GetSagaConsumers());
    }
    
    private IEnumerable<IConsumer> GetSagaConsumers()
    {
        return new List<IConsumer>
        {
            // new StartSalesOrderSagaConsumer()
        };
    }
    
    [Fact]
    public void RunSagaSuccessfully()
    {
        StartSalesOrderSaga startSalesOrderSaga = new StartSalesOrderSaga(
            _salesOrderId,
            _correlationId,
            _salesOrderNumber,
            _orderDate,
            _customerId,
            _customerName,
            new List<SalesOrderRowJson>
            {
                new SalesOrderRowJson
                {
                    BeerId = new Guid(_beerId.Value),
                    BeerName = _beerName.Value,
                    Quantity = _quantity,
                    Price = _price
                }
            }
        );
    }
}