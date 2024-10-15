using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Payments.SharedKernel.Events;

public sealed class MoneyWithdrawn(CustomerId aggregateId,
    Amount amount) : DomainEvent(aggregateId)
{
    public readonly CustomerId CustomerId = aggregateId;
    public readonly Amount Amount = amount;
}