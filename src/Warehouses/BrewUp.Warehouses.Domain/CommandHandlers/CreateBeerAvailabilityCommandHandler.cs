﻿using BrewUp.Warehouses.Domain.Entities;
using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.CommandHandlers;

public sealed class CreateBeerAvailabilityCommandHandler(
    IRepository repository,
    ILoggerFactory loggerFactory)
    : CommandHandlerBaseAsync<CreateBeerAvailability>(repository, loggerFactory)
{
    public override async Task ProcessCommand(CreateBeerAvailability command, CancellationToken cancellationToken = default)
    {
        var aggregate = Availability.CreateAvailability(command.BeerId, command.BeerName, command.Quantity, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}