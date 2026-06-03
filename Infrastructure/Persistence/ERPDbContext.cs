using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
  public class ERPDbContext : DbContext, IApplicationDbContext
  {
    private readonly ITenantProvider _tenantProvider;
    public ERPDbContext(DbContextOptions<ERPDbContext> options, ITenantProvider tenantProvider) : base(options)
    {
      _tenantProvider = tenantProvider;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfigurationsFromAssembly(typeof(ERPDbContext).Assembly);

      foreach (var entityType in builder.Model.GetEntityTypes())
      {
        if (typeof(BaseTenantEntity).IsAssignableFrom(entityType.ClrType))
        {
          var method = typeof(ERPDbContext).GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)!.MakeGenericMethod(entityType.ClrType);

          method.Invoke(this, new object[] { builder });
        }
      }
    }

    private void SetTenantFilter<TEntity>(ModelBuilder builder) where TEntity : BaseTenantEntity
    {
      builder.Entity<TEntity>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
    } 

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
      var now = DateTime.UtcNow;

      foreach (var entry in ChangeTracker.Entries<BaseTenantEntity>())
      {
        if (entry.State == EntityState.Added)
        {
          entry.Entity.CreatedAt = now;
          entry.Entity.UpdatedAt = now;
          if(_tenantProvider.TenantId != Guid.Empty)
          {
            entry.Entity.TenantId = _tenantProvider.TenantId;
          }
        }

        if (entry.State == EntityState.Modified)
        {
          entry.Entity.UpdatedAt = now;
        }
      }

      return base.SaveChangesAsync(ct);
    }
  }
}