using BrewUp.Payments.SharedKernel.Events;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Core;

namespace BrewUp.Payments.Domain.Entities;

public sealed class SavingsAccount : AggregateRoot
{
    private CustomerName _customerName;
    private Balance _balance;

    protected SavingsAccount()
    {}
    
    internal static SavingsAccount Create(CustomerId customerId, CustomerName customerName)
    {
        return new SavingsAccount(customerId, customerName);
    }
    
    private SavingsAccount(CustomerId customerId, CustomerName customerName)
    {
        RaiseEvent(new SavingsAccountCreated(customerId, customerName));
    }
    
    private void Apply(SavingsAccountCreated @event)
    {
        Id = @event.CustomerId;
        _customerName = @event.CustomerName;
        
        _balance = new Balance(0, string.Empty);
    }
    
    internal void Deposit(Amount amount)
    {
        amount = amount with {Value = _balance.Value + amount.Value}; 
        RaiseEvent(new MoneyDeposited((CustomerId)Id, amount));
    }
    
    private void Apply(MoneyDeposited @event)
    {
        _balance = new Balance(@event.Amount.Value, @event.Amount.Currency);
    }
}