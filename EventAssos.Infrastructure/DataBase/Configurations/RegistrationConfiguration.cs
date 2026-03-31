using EventAssos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.HasKey(r => r.Id);

        // Inscription unique à l'event
        builder.HasIndex(r => new { r.EventId, r.MemberId })
            .IsUnique();

        // Relation Event
        builder.HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relation Member
        builder.HasOne(r => r.Member)
            .WithMany(m => m.Registrations)
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.RegisteredAt)
            .IsRequired();

        builder.Property(r => r.WaitingPosition);
    }
}