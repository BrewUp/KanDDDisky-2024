﻿using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public sealed class CreateSalesOrderCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
	: CommandHandlerAsync<CreateSalesOrder>(repository, loggerFactory)
{
	public override async Task HandleAsync(CreateSalesOrder command, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		var aggregate = SalesOrder.CreateSalesOrder(command.SalesOrderId, command.MessageId, command.SalesOrderNumber,
			command.OrderDate, command.CustomerId, command.CustomerName, command.Rows);
		await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
	}
}