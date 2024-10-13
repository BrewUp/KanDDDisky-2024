using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Sales.SharedKernel.Commands;

public class CloseSalesOrder(SalesOrderId aggregateId, 
    Guid commitId,
    SalesOrderNumber salesOrderNumber) : Command(aggregateId,commitId)
{
    public readonly SalesOrderId SalesOrderId = aggregateId;
    public readonly SalesOrderNumber SalesOrderNumber = salesOrderNumber;
}