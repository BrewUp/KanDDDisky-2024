﻿using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Payments.SharedKernel.Events;

public sealed class MoneyWithdrawnRejected(CustomerId aggregateId, Guid commitId) 
    : DomainEvent(aggregateId, commitId)
{
    public readonly CustomerId CustomerId = aggregateId;
}