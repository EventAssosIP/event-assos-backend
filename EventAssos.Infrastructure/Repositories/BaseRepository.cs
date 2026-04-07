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

        // --- MÉTHODES EXISTANTES (CONSERVÉES) ---

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
            return await _entities.AnyAsync(e => EF.Property<TKey>(e, "Id").Equals(id));
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
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

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TKey id, TEntity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        // --- NOUVELLES MÉTHODES POUR L'UNIT OF WORK (SANS SAVESCHANGES) ---
        // Ces méthodes permettent de préparer les changements en cascade sans les valider.

        /// <summary>
        /// Ajoute l'entité au contexte sans sauvegarder en base.
        /// Utile pour les opérations atomiques via Unit of Work.
        /// </summary>
        public async Task AddToTrackerAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        /// <summary>
        /// Marque l'entité comme modifiée dans le tracker sans sauvegarder en base.
        /// </summary>
        public void UpdateInTracker(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Marque l'entité pour suppression dans le tracker sans sauvegarder en base.
        /// </summary>
        public void DeleteFromTracker(TEntity entity)
        {
            _entities.Remove(entity);
        }
    }
}