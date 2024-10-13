using BrewUp.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace BrewUp.Saga.Messages.Commands;

public sealed class AskForBeerAvailability(BeerId aggregateId, Guid commitId)
    : Command(aggregateId, commitId)
{
    public readonly BeerId BeerId = aggregateId;
}