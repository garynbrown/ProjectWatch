using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.Data;

namespace Core.Common.Contracts
{
	public interface IDataRepository
	{
	}

	public interface IDataRepository<T> : IDataRepository
		where T : ClientEntityBase, new()
	{
		Task<T> AddAsync(T entity);
		T Add(T entity);

		void RemoveAsync(T entity);
		void Remove(T entity);

		Task<T> UpdateAsync(T entity);
		T Update(T entity);

		Task<EntitySetBase<T>> GetAsync();
		EntitySetBase<T> Get();

		Task<T> GetAsync(int id);
		T Get(int id);

	}
}
