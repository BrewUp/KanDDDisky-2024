using Muflone.Core;
using Muflone.Messages.Commands;

namespace BrewUp.Saga.Messages.Commands;

public sealed class StartSalesOrderSaga(IDomainId aggregateId, Guid commitId) : Command(aggregateId, commitId)
{
    
}