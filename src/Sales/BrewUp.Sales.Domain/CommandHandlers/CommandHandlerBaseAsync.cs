﻿using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public abstract class CommandHandlerBaseAsync<TCommand>(IRepository repository, ILoggerFactory loggerFactory)
  : CommandHandlerAsync<TCommand>(repository, loggerFactory)
  where TCommand : class, ICommand
{
  public override async Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
  {
    cancellationToken.ThrowIfCancellationRequested();

    try
    {
      Logger.LogInformation(
        "Processing command: {Type} - Aggregate: {CommandAggregateId} - CommandId : {CommandMessageId}",
        command.GetType(), command.AggregateId, command.MessageId);
      await ProcessCommand(command, cancellationToken);
    }
    catch (Exception ex)
    {
      Logger.LogError(
        "Error processing command: {Type} - Aggregate: {CommandAggregateId} - CommandId : {CommandMessageId} - Messagge: {ExMessage} - Stack Trace {ExStackTrace}",
        command.GetType(), command.AggregateId, command.MessageId, ex.Message, ex.StackTrace);
      throw;
    }
  }

  public abstract Task ProcessCommand(TCommand command, CancellationToken cancellationToken = default);
}