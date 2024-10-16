using BrewUp.Shared.CustomTypes;
using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.CommandHandlers;

public sealed class AskForBeerAvailabilityCommandHandler(IRepository repository,
    ILoggerFactory loggerFactory): CommandHandlerAsync<AskForBeerAvailability>(repository, loggerFactory)
{
    public override async Task HandleAsync(AskForBeerAvailability command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var aggregate = await Repository.GetByIdAsync<Entities.Availability>(command.BeerId, cancellationToken);
        aggregate!.AskForAvailability(command.Quantity, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}