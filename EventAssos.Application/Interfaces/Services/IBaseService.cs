namespace EventAssos.Application.Interfaces.Services
{
    public interface IBaseService<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);

        Task<TEntity> CreateAsync(TEntity member);
        Task UpdateAsync(TKey id, TEntity member);
        Task DeleteAsync(TKey id);
    }
}
