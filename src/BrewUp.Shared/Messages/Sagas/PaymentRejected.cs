using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Shared.Messages.Sagas;

public sealed class PaymentRejected(CustomerId aggregateId, Guid commitId) 
    : IntegrationEvent(aggregateId, commitId)
{
    public readonly CustomerId CustomerId = aggregateId;
}