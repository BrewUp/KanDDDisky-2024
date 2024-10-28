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

public sealed class CreateSavingsAccountSuccessfully : CommandSpecification<CreateSavingsAccount>
{
    private CustomerId _customerId = new(Guid.NewGuid());
    private CustomerName _customerName = new("Muflone");
    
    protected override IEnumerable<DomainEvent> Given()
    {
        yield break;
    }

    protected override CreateSavingsAccount When()
    {
        return new CreateSavingsAccount(_customerId, _customerName);
    }

    protected override ICommandHandlerAsync<CreateSavingsAccount> OnHandler()
    {
        return new CreateSavingsAccountCommandHandler(Repository, new NullLoggerFactory());
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
        yield return new SavingsAccountCreated(_customerId, _customerName);
    }
}