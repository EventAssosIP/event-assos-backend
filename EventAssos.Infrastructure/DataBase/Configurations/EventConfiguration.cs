using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Description)
            .HasMaxLength(255);

        builder.Property(e => e.Location)
            .HasMaxLength(255);

        builder.Property(e => e.StartDate)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(e => e.EndDate)
            .HasColumnType("datetime2");

        builder.Property(e => e.Category)
               .HasConversion<string>()
               .HasDefaultValue(Category.Other);

        builder.HasIndex(e => new { e.Category, e.StartDate });

        builder.Property(e => e.MinParticipants)
            .IsRequired();

        builder.Property(e => e.MaxParticipants)
            .IsRequired();      

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasDefaultValue(EventStatus.Pending);

        builder.HasIndex(e => new { e.Status, e.StartDate });

        builder.Property(e => e.IsWaitingListActive)
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2");
    }
}
