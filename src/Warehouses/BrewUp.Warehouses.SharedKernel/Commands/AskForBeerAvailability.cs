using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Warehouses.SharedKernel.Commands;

public sealed class AskForBeerAvailability(BeerId aggregateId, Guid commitId)
    : Command(aggregateId, commitId)
{
    public readonly BeerId BeerId = aggregateId;
}