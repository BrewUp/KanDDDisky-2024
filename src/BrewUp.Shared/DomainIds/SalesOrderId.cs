using Muflone.Core;

namespace BrewUp.Shared.DomainIds;

public sealed class SalesOrderId : DomainId
{
	public SalesOrderId(Guid value) : base(value.ToString())
	{
	}
}