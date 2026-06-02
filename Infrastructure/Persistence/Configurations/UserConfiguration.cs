using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("users");

      builder.HasKey(x => x.Id);

      builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();

      builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();

      builder.Property(x => x.Email).HasMaxLength(255).IsRequired();

      builder.Property(x => x.PasswordHash).IsRequired();

      builder.Property(x => x.TenantId).IsRequired();

      builder.Property(x => x.CreatedAt).IsRequired();

      builder.Property(x => x.UpdatedAt).IsRequired();

      builder.Property(x => x.IsActive).HasDefaultValue(true);

      builder.HasOne(x => x.Tenant).WithMany(x => x.Users).HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(x => new {
        x.Email,
        x.TenantId
      }).IsUnique();

      builder.HasIndex(x => x.TenantId);
    }
  }
}