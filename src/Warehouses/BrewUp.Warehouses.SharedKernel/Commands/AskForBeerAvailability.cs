using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Warehouses.SharedKernel.Commands;

public sealed class AskForBeerAvailability(BeerId aggregateId, Guid commitId, Quantity quantity)
    : Command(aggregateId, commitId)
{
    public readonly BeerId BeerId = aggregateId;
    public readonly Quantity Quantity = quantity;
}