namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Valide tous les changements effectués dans le contexte (Atomicité ACID).
        /// </summary>
        /// <returns>Le nombre de lignes affectées en base de données.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Optionnel : Permet de gérer manuellement une transaction si nécessaire.
        /// </summary>
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}