using BrewUp.Shared.CustomTypes;

namespace BrewUp.Saga.Models;

public record BeerAvailabilities(Guid BeerId, Quantity Quantity, Quantity Availability);