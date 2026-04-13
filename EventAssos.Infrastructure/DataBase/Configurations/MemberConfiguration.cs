using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        // PK
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever(); // GUID géré par le domaine

        builder.Property(m => m.Pseudo)
            .IsRequired()
            .HasMaxLength(50);

        var converter = new ValueConverter<DateOnly?, DateTime?>(
        v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : null,
        v => v.HasValue ? DateOnly.FromDateTime(v.Value) : null
        );

        builder.Property(m => m.Birthdate)
            .HasConversion(converter)
            .HasColumnType("Date");

        builder.Property(m => m.Gender)
            .HasConversion<string>(); // lisible en DB

        builder.Property(m => m.Role)
            .HasConversion<string>();

        builder.Property(m => m.EmailAddress)
            .HasConversion(
                v => v.Value,                 // vers DB (string)
                v => EmailAddress.Create(v)) // vers domaine (VO)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(m => m.EmailAddress)
            .IsUnique();


        builder.OwnsOne(m => m.Password, password =>
        {
            password.Property(p => p.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.Property(m => m.CreatedAt)
            .IsRequired();
    }
}
