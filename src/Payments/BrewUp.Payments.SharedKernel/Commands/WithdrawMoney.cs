using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Payments.SharedKernel.Commands;

public sealed class WithdrawMoney(CustomerId aggregateId, Guid commitId,
    CustomerName customerName,
    Amount amount) : Command(aggregateId, commitId)
{
    public readonly CustomerId CustomerId = aggregateId;
    public readonly CustomerName CustomerName = customerName;
    public readonly Amount Amount = amount;
}