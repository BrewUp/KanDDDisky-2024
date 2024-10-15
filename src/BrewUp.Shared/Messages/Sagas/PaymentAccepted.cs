using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Shared.Messages.Sagas;

public sealed class PaymentAccepted(CustomerId aggregateId, Guid commitId, Amount amount) 
    : IntegrationEvent(aggregateId, commitId)
{
    public readonly CustomerId CustomerId = aggregateId;
}