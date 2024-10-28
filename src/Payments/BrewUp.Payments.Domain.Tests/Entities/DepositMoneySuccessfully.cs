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

public sealed class DepositMoneySuccessfully : CommandSpecification<DepositMoney>
{
    private CustomerId _customerId = new(Guid.NewGuid());
    private CustomerName _customerName = new("Muflone");
    
    private Amount _amount = new(1000, "EUR");
    
    protected override IEnumerable<DomainEvent> Given()
    {
        yield return new SavingsAccountCreated(_customerId, _customerName);
    }

    protected override DepositMoney When()
    {
        return new DepositMoney(_customerId, _customerName, _amount);
    }

    protected override ICommandHandlerAsync<DepositMoney> OnHandler()
    {
        return new DepositMoneyCommandHandler(Repository, new NullLoggerFactory());
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
        yield return new MoneyDeposited(_customerId, _amount);
    }
}