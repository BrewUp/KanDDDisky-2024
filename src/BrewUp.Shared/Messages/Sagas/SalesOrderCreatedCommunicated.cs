using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Shared.Messages.Sagas;

public sealed class SalesOrderCreatedCommunicated(SalesOrderId aggregateId,
    Guid commitId, 
    SalesOrderNumber salesOrderNumber,
    OrderDate orderDate, 
    CustomerId customerId,
    CustomerName customerName,
    IEnumerable<SalesOrderRowJson> rows) : IntegrationEvent(aggregateId, commitId)
{
    public readonly SalesOrderId SalesOrderId = aggregateId;
    public readonly SalesOrderNumber SalesOrderNumber = salesOrderNumber;
    public readonly OrderDate OrderDate = orderDate;

    public readonly CustomerId CustomerId = customerId;
    public readonly CustomerName CustomerName = customerName;

    public readonly IEnumerable<SalesOrderRowJson> Rows = rows;
}