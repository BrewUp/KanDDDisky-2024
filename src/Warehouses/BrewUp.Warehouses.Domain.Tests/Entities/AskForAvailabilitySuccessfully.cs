using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Warehouses.Domain.CommandHandlers;
using BrewUp.Warehouses.Domain.Tests.InMemory;
using BrewUp.Warehouses.SharedKernel.Commands;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;

namespace BrewUp.Warehouses.Domain.Tests.Entities;

public sealed class AskForAvailabilitySuccessfully : CommandSpecification<AskForBeerAvailability>
{
    private readonly BeerId _beerId = new(Guid.NewGuid());
    private readonly BeerName _beerName = new("Muflone IPA");
    private readonly Quantity _quantity = new(100, "Lt");

    private readonly Quantity _quantityForSale = new(30, "Lt");
    private readonly Quantity _availability = new(70, "Lt");
    
    private readonly Guid _correlationId = Guid.NewGuid();
    
    protected override IEnumerable<DomainEvent> Given()
    {
        yield return new BeerAvailabilityCreated(_beerId, _correlationId, _beerName, _quantity);
    }

    protected override AskForBeerAvailability When()
    {
        return new AskForBeerAvailability(_beerId, _correlationId, _quantityForSale);
    }

    protected override ICommandHandlerAsync<AskForBeerAvailability> OnHandler()
    {
        return new AskForBeerAvailabilityCommandHandler(Repository, new NullLoggerFactory());
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
        yield return new BeerAvailable(_beerId, _correlationId, _availability);
    }
}