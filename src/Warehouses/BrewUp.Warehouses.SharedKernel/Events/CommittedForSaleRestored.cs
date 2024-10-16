using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Warehouses.SharedKernel.Events;

public class CommittedForSaleRestored(BeerId aggregateId, Guid correlationId,
    Quantity quantity) : DomainEvent(aggregateId, correlationId)
{
    public readonly BeerId BeerId = aggregateId;
    public readonly Quantity Quantity = quantity;
}