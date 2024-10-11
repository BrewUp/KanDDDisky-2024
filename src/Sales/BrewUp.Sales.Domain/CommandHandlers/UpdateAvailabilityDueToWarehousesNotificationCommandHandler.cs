using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public sealed class UpdateAvailabilityDueToWarehousesNotificationCommandHandler(
  IRepository repository,
  ILoggerFactory loggerFactory)
  : CommandHandlerBaseAsync<UpdateAvailabilityDueToWarehousesNotification>(repository, loggerFactory)
{
  public override async Task ProcessCommand(UpdateAvailabilityDueToWarehousesNotification command,
    CancellationToken cancellationToken = default)
  {
    try
    {
      var aggregate = await Repository.GetByIdAsync<Availability>(command.BeerId, cancellationToken);
      aggregate!.UpdateAvailability(command.Quantity, command.MessageId);

      await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
    catch
    {
      // I'm lazy ... I should check the exception type
      var aggregate = Availability.CreateAvailability(command.BeerId, command.BeerName, command.Quantity, command.MessageId);

      await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
  }
}