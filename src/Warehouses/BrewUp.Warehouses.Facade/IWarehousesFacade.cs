using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;
using BrewUp.Warehouses.SharedKernel.Contracts;

namespace BrewUp.Warehouses.Facade;

public interface IWarehousesFacade
{
	Task SetAvailabilityAsync(SetAvailabilityJson availability, CancellationToken cancellationToken);
	Task<PagedResult<BeerAvailabilityJson>> GetAvailabilitiesAsync(CancellationToken cancellationToken);
}