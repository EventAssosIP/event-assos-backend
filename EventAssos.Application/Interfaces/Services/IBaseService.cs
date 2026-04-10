namespace EventAssos.Application.Interfaces.Services
{
    public interface IBaseService<TEntity, TKey, TUpdateDto>
        where TEntity : class
        where TKey : struct
        where TUpdateDto : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity> CreateAsync(TEntity entity);

        // La méthode de mise à jour utilise maintenant le type générique TUpdateDto
        Task UpdateAsync(TKey id, TUpdateDto dto);

        Task DeleteAsync(TKey id);
    }
}