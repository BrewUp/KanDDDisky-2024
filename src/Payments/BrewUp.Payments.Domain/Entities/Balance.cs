using Muflone.Core;

namespace BrewUp.Payments.Domain.Entities;

public class Balance : ValueObject
{
    public readonly decimal Value;
    public readonly string Currency;

    public Balance(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield break;
    }
}