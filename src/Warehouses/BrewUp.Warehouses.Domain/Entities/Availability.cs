using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Warehouses.SharedKernel.Events;
using Muflone.Core;

namespace BrewUp.Warehouses.Domain.Entities;

public class Availability : AggregateRoot
{
	internal BeerId _beerId;
	internal BeerName _beerName;
	internal Quantity _quantity = new(0, string.Empty);
	internal Quantity _committedForSale;

	protected Availability()
	{
	}

	internal static Availability CreateAvailability(BeerId beerId, BeerName beerName, Quantity quantity, Guid correlationId)
	{
		// Check invariant here!
		return new Availability(beerId, beerName, quantity, correlationId);
	}

	private Availability(BeerId beerId, BeerName beerName, Quantity quantity, Guid correlationId)
	{
		RaiseEvent(new BeerAvailabilityCreated(beerId, correlationId, beerName, quantity));
	}
	
	private void Apply(BeerAvailabilityCreated @event)
	{
		Id = @event.BeerId;

		_beerId = @event.BeerId;
		_beerName = @event.BeerName;
		_quantity = @event.Quantity;
		_committedForSale = @event.Quantity with {Value = 0};
	}

	internal void UpdateAvailability(Quantity quantity, Guid correlationId)
	{
		quantity = _quantity with { Value = _quantity.Value + quantity.Value };
		RaiseEvent(new AvailabilityUpdatedDueToProductionOrder(_beerId, correlationId, _beerName, quantity));
	}

	private void Apply(AvailabilityUpdatedDueToProductionOrder @event)
	{
		Id = @event.BeerId;

		_beerId = @event.BeerId;
		_beerName = @event.BeerName;
		_quantity = @event.Quantity;
	}

	internal void AskForAvailability(Quantity quantity, Guid correlationId)
	{
		// Check if there is enough availability and raise a different event if not
		var availability = _quantity with {Value = _quantity.Value - _committedForSale.Value - quantity.Value};
		if (availability.Value >= 0)
			RaiseEvent(new BeerAvailable(_beerId, correlationId, availability));
		else
			RaiseEvent(new BeerNotAvailable(_beerId, correlationId));
	}
	
	private void Apply(BeerAvailable @event)
	{
		_committedForSale = _committedForSale with {Value = _committedForSale.Value + @event.Quantity.Value};
	}

	private void Apply(BeerNotAvailable @event)
	{
		// Do nothing
	}
	
	internal void RestoreCommittedForSale(Quantity quantity, Guid correlationId)
	{
		_committedForSale = _committedForSale with {Value = _committedForSale.Value - quantity.Value};
		RaiseEvent(new CommittedForSaleRestored(_beerId, correlationId, _quantity));
	}
	
	private void Apply(CommittedForSaleRestored @event)
	{
		_committedForSale = @event.Quantity;
	}
}