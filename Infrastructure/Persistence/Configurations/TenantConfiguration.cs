using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
  {
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
      builder.ToTable("tenants");

      builder.HasKey(x => x.Id);

      builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

      builder.Property(x => x.CreatedAt).IsRequired();

      builder.Property(x => x.UpdatedAt).IsRequired();
    }
  }
}