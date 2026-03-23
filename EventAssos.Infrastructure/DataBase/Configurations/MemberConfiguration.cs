using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
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
            .HasColumnType("date");



        builder.Property(m => m.Gender)
            .HasConversion<string>() // lisible en DB
            .HasDefaultValue(Gender.Male);

        builder.Property(m => m.Role)
            .HasConversion<string>()
            .HasDefaultValue(Role.User);

        builder.OwnsOne(m => m.EmailAddress, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(255);

            email.HasIndex(e => e.Value)
                .IsUnique(); // UNIQUE EMAIL
        });

        builder.OwnsOne(m => m.Password, password =>
        {
            password.Property(p => p.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.HasIndex("Email"); // index pour login
    }
}
