using BrewUp.Shared.CustomTypes;

namespace BrewUp.Payments.SharedKernel.Contracts;

public record PaymentMovementJson(string CustomerId, string CustomerName, Amount Amount);