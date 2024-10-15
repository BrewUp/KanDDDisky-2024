using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Payments.SharedKernel.Commands;

public sealed class DepositMoney(CustomerId aggregateId,
    CustomerName customerName,
    Amount amount) : Command(aggregateId)
{
    public readonly CustomerId CustomerId = aggregateId;
    public readonly CustomerName CustomerName = customerName;
    public readonly Amount Amount = amount;
}