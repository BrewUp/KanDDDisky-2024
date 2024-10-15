using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;

namespace BrewUp.Payments.ReadModel.Services;

public interface ISavingsAccountService
{
    Task CreateSavingsAccountAsync(CustomerId customerId, CustomerName customerName,
        CancellationToken cancellationToken = default);
    Task DepositMoneyAsync(CustomerId customerId, Amount amount, CancellationToken cancellationToken = default);
    Task WithdrawMoneyAsync(CustomerId customerId, Amount amount, CancellationToken cancellationToken = default);
    
    Task<bool> CheckIfSavingsAccountExistsAsync(CustomerId customerId, CancellationToken cancellationToken = default);
}