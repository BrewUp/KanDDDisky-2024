using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Entities;
using BrewUp.Shared.ReadModel;
using BrewUp.Warehouses.SharedKernel.Commands;
using BrewUp.Warehouses.SharedKernel.Contracts;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Facade;

public sealed class WarehousesFacade(IServiceBus serviceBus,
	IQueries<ReadModel.Dtos.Availability> queries) : IWarehousesFacade
{

	public async Task SetAvailabilityAsync(SetAvailabilityJson availability, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		var availabilityDto = await queries.GetByIdAsync(availability.BeerId, cancellationToken);
		if (availabilityDto is not null)
		{
			UpdateAvailabilityDueToProductionOrder command =
				new(new BeerId(new Guid(availability.BeerId)), Guid.NewGuid(), new BeerName(availability.BeerName),
					availability.Quantity);

			await serviceBus.SendAsync(command, cancellationToken);			
		}
		else
		{
			CreateBeerAvailability command =
				new(new BeerId(new Guid(availability.BeerId)), Guid.NewGuid(), new BeerName(availability.BeerName),
					availability.Quantity);
			await serviceBus.SendAsync(command, cancellationToken);
		}
	}

	public async Task<PagedResult<BeerAvailabilityJson>> GetAvailabilitiesAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var availabilities = await queries.GetByFilterAsync(null, 0, 100, cancellationToken);
		
		return new PagedResult<BeerAvailabilityJson>(availabilities.Results.Select(x => new BeerAvailabilityJson
				( x.BeerId, x.BeerName, new Availability(0, x.Quantity.Value, x.Quantity.UnitOfMeasure))), 
			availabilities.Page, availabilities.PageSize, availabilities.Results.Count());
	}
}