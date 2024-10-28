using BrewUp.Payments.Domain.CommandHandlers;
using BrewUp.Payments.Domain.Tests.InMemory;
using BrewUp.Payments.SharedKernel.Commands;
using BrewUp.Payments.SharedKernel.Events;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;

namespace BrewUp.Payments.Domain.Tests.Entities;

public sealed class WithdrawingMoneyFailure : CommandSpecification<WithdrawMoney>
{
    private CustomerId _customerId = new(Guid.NewGuid());
    private CustomerName _customerName = new("Muflone");
    
    private Guid _correlationId = Guid.NewGuid();
    
    private Amount _amount = new(1000, "EUR");
    private Amount _withdrawn = new(500, "EUR");
    
    protected override IEnumerable<DomainEvent> Given()
    {
        yield return new SavingsAccountCreated(_customerId, _customerName);
        yield return new MoneyDeposited(_customerId, _amount);
        yield return new MoneyWithdrawAccepted(_customerId, _correlationId, _withdrawn);
        yield return new MoneyWithdrawAccepted(_customerId, _correlationId, _withdrawn);
    }

    protected override WithdrawMoney When()
    {
        return new WithdrawMoney(_customerId, _correlationId, _customerName, _withdrawn);
    }

    protected override ICommandHandlerAsync<WithdrawMoney> OnHandler()
    {
        return new WithdrawMoneyCommandHandler(Repository, new NullLoggerFactory());
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
        yield return new MoneyWithdrawRejected(_customerId, _correlationId);
    }
}