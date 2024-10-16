using BrewUp.Warehouses.Domain.Entities;
using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.CommandHandlers;

public sealed class UpdateAvailabilityDueToProductionOrderCommandHandler(
	IRepository repository,
	ILoggerFactory loggerFactory)
	: CommandHandlerAsync<UpdateAvailabilityDueToProductionOrder>(repository, loggerFactory)
{
	public override async Task HandleAsync(UpdateAvailabilityDueToProductionOrder command, CancellationToken cancellationToken = default)
	{
		var aggregate = await Repository.GetByIdAsync<Availability>(command.BeerId, cancellationToken);
		aggregate!.UpdateAvailability(command.Quantity, command.MessageId);

		await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
	}
}