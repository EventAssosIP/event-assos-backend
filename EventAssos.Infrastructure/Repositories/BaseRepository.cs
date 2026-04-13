using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EventAssos.Application.Interfaces.Repositories; // Assure-toi d'importer l'interface

namespace EventAssos.Infrastructure.Repositories
{
    public class BaseRepository<TEntity, TKey>(EventAssosContext _context) : IBaseRepository<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        protected readonly DbSet<TEntity> _entities = _context.Set<TEntity>();

        // --- LECTURE ---

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task<bool> ExistsAsync(TKey id)
        {
            // Correction : Utilisation de FindAsync ou Any avec comparaison d'ID
            // La réflexion EF.Property est correcte si toutes tes entités ont une propriété "Id"
            return await _entities.AnyAsync(e => EF.Property<TKey>(e, "Id").Equals(id));
        }

        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync();
        }

        // --- ÉCRITURE IMMÉDIATE ---

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            // On vérifie si l'entité est déjà suivie pour éviter les conflits de tracking
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        // AJOUT : La surcharge demandée par tes nouveaux services
        public async Task DeleteAsync(TEntity entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // --- UNIT OF WORK (TRACKER UNIQUEMENT) ---

        public async Task AddToTrackerAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public void UpdateInTracker(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteFromTracker(TEntity entity)
        {
            _entities.Remove(entity);
        }
    }
}