using BrewUp.Warehouses.ReadModel.Services;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;

namespace BrewUp.Warehouses.ReadModel.EventHandlers;

public sealed class BeerAvailabilityCreatedEventHandler(ILoggerFactory loggerFactory,
		IAvailabilityService availabilityService)
	: DomainEventHandlerBase<BeerAvailabilityCreated>(loggerFactory)
{
	public override async Task HandleAsync(BeerAvailabilityCreated @event,
		CancellationToken cancellationToken = new())
	{
		cancellationToken.ThrowIfCancellationRequested();

		await availabilityService.CreateAvailabilityAsync(@event.BeerId, @event.BeerName, @event.Quantity, cancellationToken);
	}
}