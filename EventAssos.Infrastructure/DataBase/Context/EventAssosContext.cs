using Microsoft.EntityFrameworkCore;
using EventAssos.Domain.Entities;

namespace EventAssos.Infrastructure.DataBase.Context
{
    public class EventAssosContext : DbContext
    {
        public EventAssosContext(DbContextOptions<EventAssosContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<EventCategory> EventCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==========================================
            // Applique toutes les configurations de l'assembly
            // ==========================================
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventAssosContext).Assembly);

            // ==========================================
            // CONFIGURATION MEMBER
            // ==========================================
            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(m => m.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(m => m.Role)
                      .HasDefaultValue(Domain.Enums.Role.User);
            });

            // ==========================================
            // CONFIGURATION EVENT
            // ==========================================
            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Status)
                      .HasDefaultValue(Domain.Enums.EventStatus.InProgress);

                // AJOUT : Configuration du RowVersion pour la concurrence optimiste (Isolation ACID)
                // Cela évite d'ajouter des attributs [Timestamp] dans ton projet Domaine (POCO)
                entity.Property(e => e.RowVersion)
                      .IsRowVersion();
            });

            // ==========================================
            // CONFIGURATION REGISTRATION
            // ==========================================
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.Property(e => e.RegisteredAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsConfirmed)
                      .HasDefaultValue(false);
            });

            // ==========================================
            // CONFIGURATION EVENTCATEGORY
            // ==========================================
            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.HasKey(c => c.Id);

                // On rend le nom obligatoire et unique pour éviter les doublons
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            // ==========================================
            // RELATION EVENT <-> EVENTCATEGORY (One-To-Many)
            // ==========================================
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(e => e.Category)      // Un événement a une catégorie
                    .WithMany(c => c.Events)        // Une catégorie a plusieurs événements
                    .HasForeignKey(e => e.CategoryId) // La clé étrangère dans la table Event
                    .OnDelete(DeleteBehavior.Restrict); // Empêche de supprimer une catégorie si elle est liée à des événements
            });
        }
    }
}