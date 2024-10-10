using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public sealed class UpdateAvailabilityDueToWarehousesNotificationCommandHandler : CommandHandlerBaseAsync<UpdateAvailabilityDueToWarehousesNotification>
{
	public UpdateAvailabilityDueToWarehousesNotificationCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
	{
	}

	public override async Task ProcessCommand(UpdateAvailabilityDueToWarehousesNotification command,
		CancellationToken cancellationToken = default)
	{
		try
		{
			var aggregate = await Repository.GetByIdAsync<Availability>(command.BeerId.Value);
			aggregate.UpdateAvailability(command.Quantity, command.MessageId);

			await Repository.SaveAsync(aggregate, Guid.NewGuid());
		}
		catch
		{
			// I'm lazy ... I should check the exception type
			var aggregate = Availability.CreateAvailability(command.BeerId, command.BeerName, command.Quantity, command.MessageId);

			await Repository.SaveAsync(aggregate, Guid.NewGuid());
		}
	}
}