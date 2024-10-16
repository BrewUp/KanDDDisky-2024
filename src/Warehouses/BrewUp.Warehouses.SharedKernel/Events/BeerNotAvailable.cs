using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Warehouses.SharedKernel.Events;

public sealed class BeerNotAvailable(BeerId aggregateId, Guid correlationId) : DomainEvent(aggregateId, correlationId)
{
    public readonly BeerId BeerId = aggregateId;
}