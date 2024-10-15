﻿using BrewUp.Payments.Domain.CommandHandlers;
using BrewUp.Payments.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Payments.Infrastructures.RabbitMq.Commands;

public sealed class WithdrawingMoneyConsumer(IRepository repository,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) 
    : CommandConsumerBase<WithdrawingMoney>(repository, connectionFactory, loggerFactory)
{
    protected override ICommandHandlerAsync<WithdrawingMoney> HandlerAsync { get; } =
        new WithdrawingMoneyCommandHandler(repository, loggerFactory);
}