using Microsoft.EntityFrameworkCore;
using EventAssos.Domain.Entities;

namespace EventAssos.Infrastructure.DataBase.Context
{
    public class EventAssosContext : DbContext
    {
        // Utiliser le type générique pour EF Core
        public EventAssosContext(DbContextOptions<EventAssosContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!; // nullable désactivé, EF Core s'attend à ce que ce soit initialisé

        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applique toutes les configurations de l'assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventAssosContext).Assembly);

            // Exemple : valeur par défaut pour CreatedAt et Role sur Member
            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(m => m.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()"); // SQL Server UTC now

                entity.Property(m => m.Role)
                      .HasDefaultValue(Domain.Enums.Role.User);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()"); // SQL Server UTC now

                entity.Property(e => e.Status)
                      .HasDefaultValue(Domain.Enums.EventStatus.InProgress);
            });
        }
    }
}