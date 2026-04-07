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
        protected readonly DbSet<TEntity> _entities = _context.Set<TEntity>();

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
            // Utilisation de AnyAsync pour rester purement asynchrone sans charger l'entité
            return await _entities.AnyAsync(e => EF.Property<TKey>(e, "Id").Equals(id));
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            // CORRECTION : ToListAsync pour filtrer CÔTÉ BASE DE DONNÉES
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _entities.FindAsync(id);
        }

        // Surcharge pratique pour ton service (quand tu as déjà l'objet en main)
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TKey id, TEntity entity)
        {
            // On s'assure que l'entité est bien attachée
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}