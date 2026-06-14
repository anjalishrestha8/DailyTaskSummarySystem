using ClientWebApi.Data;
using ClientWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClientWebApi.Repositories
{
    public class GenericRepository<T,TId> : IGenericRepository<T,TId> where T : EntityBase<TId>
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<T> dbSet;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }
        public IQueryable<T> FindByCondition(
                        Expression<Func<T, bool>> expression) =>
                        dbSet.Where(expression).AsNoTracking();
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        public virtual async Task<T?> GetByIdAsync(TId id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            var existingEntity = await dbSet.FindAsync(entity.Id);
            if (existingEntity == null)
            {
                return null;
            }
            entity.UpdatedAt = DateTime.UtcNow;
            dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            return entity;
        }

        public virtual async Task<T?> DeleteAsync(TId id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            entity.UpdatedAt = DateTime.UtcNow;
            dbSet.Remove(entity);
            return entity;
        }

    }
}
