using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Shared.Messages.Sagas;

public sealed class BeerAvailableCommunicated(BeerId aggregateId, Guid correlationId,
	Quantity availability)
	: IntegrationEvent(aggregateId, correlationId)
{
	public readonly BeerId BeerId = aggregateId;
	public readonly Quantity Availability = availability;
}