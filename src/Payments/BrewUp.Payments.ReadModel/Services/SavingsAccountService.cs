using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.ReadModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BrewUp.Payments.ReadModel.Services;

public sealed class SavingsAccountService(ILoggerFactory loggerFactory, [FromKeyedServices("payments")]IPersister persister)
    : ServiceBase(loggerFactory, persister), ISavingsAccountService
{
    public async Task CreateSavingsAccountAsync(CustomerId customerId, CustomerName customerName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            var savingsAccount = Dtos.SavingsAccount.Create(customerId, customerName);
            await Persister.InsertAsync(savingsAccount, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating savingsAccount");
            throw;
        }
    }

    public async Task DepositMoneyAsync(CustomerId customerId, Amount amount, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            var savingsAccount = await Persister.GetByIdAsync<Dtos.SavingsAccount>(customerId.Value, cancellationToken);
            savingsAccount.DepositMoney(amount);
            await Persister.UpdateAsync(savingsAccount, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deposit money into savingsAccount");
            throw;
        }
    }

    public async Task WithdrawMoneyAsync(CustomerId customerId, Amount amount, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            var savingsAccount = await Persister.GetByIdAsync<Dtos.SavingsAccount>(customerId.Value, cancellationToken);
            savingsAccount.WithdrawMoney(amount);
            await Persister.UpdateAsync(savingsAccount, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error withdrawing money into savingsAccount");
            throw;
        }
    }

    public async Task<bool> CheckIfSavingsAccountExistsAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        try
        {
            var savingsAccount = await Persister.GetByIdAsync<Dtos.SavingsAccount>(customerId.Value, cancellationToken);
            return savingsAccount is not null && !string.IsNullOrEmpty(savingsAccount.CustomerId);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error withdrawing money into savingsAccount");
            throw;
        }
    }
}