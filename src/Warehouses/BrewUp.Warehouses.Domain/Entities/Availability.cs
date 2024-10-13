using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Warehouses.SharedKernel.Events;
using Muflone.Core;

namespace BrewUp.Warehouses.Domain.Entities;

public class Availability : AggregateRoot
{
	internal BeerId _beerId;
	internal BeerName _beerName;
	internal Quantity _quantity;
	internal Quantity _committedForSale;

	protected Availability()
	{
	}

	internal static Availability CreateAvailability(BeerId beerId, BeerName beerName, Quantity quantity, Guid correlationId)
	{
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

	public void AskForAvailability(Guid correlationId)
	{
		var availability = _quantity with {Value = _quantity.Value - _committedForSale.Value};
		RaiseEvent(new AvailabilityChecked(_beerId, correlationId, availability));
	}
	
	private void Apply(AvailabilityChecked @event)
	{
		_committedForSale = _committedForSale with {Value = _committedForSale.Value + @event.Quantity.Value};
	}
}