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

    // CONSEIL : Garde les deux versions de Delete
    Task DeleteAsync(TKey id);             // Utile si on a que l'ID
    Task DeleteAsync(TEntity entity);      // AJOUT : Bien plus efficace si on a déjà l'objet (évite un Find interne dans EF)

    // --- UNIT OF WORK ---
    Task AddToTrackerAsync(TEntity entity);
    void UpdateInTracker(TEntity entity);
    void DeleteFromTracker(TEntity entity);
}