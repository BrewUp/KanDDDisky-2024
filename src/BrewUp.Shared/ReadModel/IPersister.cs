using BrewUp.Shared.Entities;

namespace BrewUp.Shared.ReadModel
{
    public interface IPersister
	{
		Task<T> GetByIdAsync<T>(string id, CancellationToken cancellationToken) where T : DtoBase;
		Task InsertAsync<T>(T entity, CancellationToken cancellationToken) where T : DtoBase;
		Task UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : DtoBase;
		Task DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : DtoBase;
	}
}