using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Payments.SharedKernel.Events;

public sealed class MoneyWithdrawAccepted(CustomerId aggregateId, Guid commitId,
    Amount amount) : DomainEvent(aggregateId, commitId)
{
    public readonly CustomerId CustomerId = aggregateId;
    public readonly Amount Amount = amount;
}