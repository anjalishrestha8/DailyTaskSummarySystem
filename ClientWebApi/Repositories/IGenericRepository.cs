using ClientWebApi.Models.Entities;
using System.Linq.Expressions;

namespace ClientWebApi.Repositories
{
    public interface IGenericRepository<T,TId> where T : EntityBase<TId>
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TId id);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteAsync(TId id);
    }
}
