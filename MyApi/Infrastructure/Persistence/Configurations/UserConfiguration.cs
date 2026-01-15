using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Domain.Users;

namespace MyApi.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.Email)
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt);
    }
}