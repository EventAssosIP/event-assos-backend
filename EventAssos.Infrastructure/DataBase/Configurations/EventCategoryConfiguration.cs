using EventAssos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssos.Infrastructure.DataBase.Configurations
{
    public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
    {
        public void Configure(EntityTypeBuilder<EventCategory> builder)
        {
            // Clé primaire
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            // Configuration du Nom
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            // INDEX UNIQUE : Très important pour ton "GetOrCreateByName"
            // Cela empêche techniquement d'avoir deux fois "Sport" en base
            builder.HasIndex(c => c.Name)
                .IsUnique();

            // Relation inverse (optionnelle si déjà configurée dans EventConfiguration)
            // Mais c'est une bonne pratique de la déclarer d'un côté.
            builder.HasMany(c => c.Events)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}