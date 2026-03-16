namespace EventAssos.Application.Interfaces.Services
{
    public interface IBaseService<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id);

        Task<TEntity> CreateAsync(TEntity user);
        Task UpdateAsync(TKey id, TEntity user);
        Task DeleteAsync(TKey id);
    }
}
