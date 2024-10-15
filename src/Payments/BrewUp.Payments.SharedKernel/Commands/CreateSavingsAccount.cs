using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Payments.SharedKernel.Commands;

public sealed class CreateSavingsAccount(CustomerId aggregateId, CustomerName customerName)
    : Command(aggregateId)
{
    public readonly CustomerId CustomerId = aggregateId;
    public readonly CustomerName CustomerName = customerName;
}