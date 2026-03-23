using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventAssos.Infrastructure.Repositories
{
    public class BaseRepository<TEntity, TKey>(EventAssosContext _context) : IBaseRepository<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {

        protected DbSet<TEntity> _entities = _context.Set<TEntity>();

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync();
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(TKey id)
        {
            return await Task.FromResult(_entities.Find(id) != null);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(_entities.Where(predicate).AsEnumerable());
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task UpdateAsync(TKey id, TEntity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
    
}

