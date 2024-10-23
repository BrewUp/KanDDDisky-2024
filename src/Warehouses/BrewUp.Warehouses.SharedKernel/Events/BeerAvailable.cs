using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Warehouses.SharedKernel.Events;

public sealed class BeerAvailable(BeerId aggregateId, Guid commitId, Quantity quantity)
    : DomainEvent(aggregateId, commitId)
{
    public readonly BeerId BeerId = aggregateId;
    public readonly Quantity Quantity = quantity;
}