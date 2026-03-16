using System.Linq.Expressions;

namespace EventAssos.Application.Interfaces.Repositories
{

   

    public interface IBaseRepository<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        // Create | Read (All, ById) | Update | Delete
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(TKey id);
        Task<int> CountAsync();


        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TKey id, TEntity entity);
        Task DeleteAsync(TKey id);
    }
}
