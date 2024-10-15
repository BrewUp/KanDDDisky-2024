using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Payments.SharedKernel.Events;

public sealed class SavingsAccountCreated(CustomerId aggregateId, CustomerName customerName)
    : DomainEvent(aggregateId)
{
    public readonly CustomerId CustomerId = aggregateId;
    public readonly CustomerName CustomerName = customerName;
}