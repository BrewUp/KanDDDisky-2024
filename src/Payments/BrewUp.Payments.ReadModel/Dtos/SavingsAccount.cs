using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Entities;

namespace BrewUp.Payments.ReadModel.Dtos;

public class SavingsAccount : DtoBase
{
    public string CustomerId { get; private set; } = default!;
    public string CustomerName { get; private set; } = default!;
    public Amount Balance { get; private set; } = new(0, string.Empty);
    
    protected SavingsAccount()
    {
    }
    
    public static SavingsAccount Create(CustomerId customerId, CustomerName customerName)
    {
        return new SavingsAccount(customerId.Value, customerName.Value);
    }
    
    private SavingsAccount(string customerId, string customerName)
    {
        Id = customerId;
        
        CustomerId = customerId;
        CustomerName = customerName;
        Balance = new(0, string.Empty);
    }
    
    internal void DepositMoney(Amount amount)
    {
        Balance = Balance with {Value = Balance.Value + amount.Value};
    }
    
    internal void WithdrawMoney(Amount amount)
    {
        Balance = Balance with {Value = Balance.Value - amount.Value};
    }
}