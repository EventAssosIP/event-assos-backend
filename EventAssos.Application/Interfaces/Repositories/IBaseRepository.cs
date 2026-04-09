using System.Linq.Expressions;

public interface IBaseRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    // --- Méthodes de lecture ---
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(TKey id);
    Task<int> CountAsync();

    // --- Méthodes d'écriture immédiate ---
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);

    // --- AJOUTS POUR UNIT OF WORK (Préparation sans sauvegarde) ---
    // Ces méthodes ne retournent pas de Task (ou seulement pour l'ajout async)
    // car elles ne vont pas jusqu'à la base de données.
    Task AddToTrackerAsync(TEntity entity);
    void UpdateInTracker(TEntity entity);
    void DeleteFromTracker(TEntity entity);
}