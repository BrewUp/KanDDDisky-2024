using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Saga.Messages.Commands;

public sealed class RestoreCommittedForSale(BeerId aggregateId, Guid correlationId,
    Quantity quantity) : Command(aggregateId, correlationId)
{
    public readonly BeerId BeerId = aggregateId;
    public readonly Quantity Quantity = quantity;
}