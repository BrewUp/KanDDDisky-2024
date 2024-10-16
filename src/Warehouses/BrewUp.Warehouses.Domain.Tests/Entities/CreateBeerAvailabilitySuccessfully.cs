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

public sealed class CreateBeerAvailabilitySuccessfully : CommandSpecification<CreateBeerAvailability>
{
    private readonly BeerId _beerId = new(Guid.NewGuid());
    private readonly BeerName _beerName = new("Muflone IPA");
    private readonly Quantity _quantity = new(100, "Lt");
    
    private readonly Guid _correlationId = Guid.NewGuid();
    
    protected override IEnumerable<DomainEvent> Given()
    {
        yield break;
    }

    protected override CreateBeerAvailability When()
    {
        return new CreateBeerAvailability(_beerId, _correlationId, _beerName, _quantity);
    }

    protected override ICommandHandlerAsync<CreateBeerAvailability> OnHandler()
    {
        return new CreateBeerAvailabilityCommandHandler(Repository, new NullLoggerFactory());
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
        yield return new BeerAvailabilityCreated(_beerId, _correlationId, _beerName, _quantity);
    }
}