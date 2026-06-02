using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tenants.Commands.RegisterTenant
{
  public class RegisterTenantHandler : IRequestHandler<RegisterTenantCommand, Guid>
  {
    private readonly IApplicationDbContext _db;

    private readonly IPasswordHasher _passwordHasher;

    public RegisterTenantHandler(IApplicationDbContext db, IPasswordHasher hasher)
    {
      _db = db;
      _passwordHasher = hasher;
    }
    public async Task<Guid> Handle(RegisterTenantCommand request, CancellationToken cancellationToken)
    {
      var tenant = new Tenant
      {
        Id = Guid.NewGuid(),

        Name = request.CompanyName,

        CreatedAt = DateTime.UtcNow,

        UpdatedAt = DateTime.UtcNow
      };

      await _db.Tenants.AddAsync(tenant, cancellationToken);

      var adminUser = new User
      {
        Id = Guid.NewGuid(),

        TenantId = tenant.Id,

        FirstName = request.FirstName,

        LastName = request.LastName,

        Email = request.Email.Trim().ToLower(),

        PasswordHash = _passwordHasher.Hash(request.Password),

        IsActive = true,

        CreatedAt = DateTime.UtcNow,

        UpdatedAt = DateTime.UtcNow
      };

      await _db.Users.AddAsync(adminUser, cancellationToken);

      await _db.SaveChangesAsync(cancellationToken);

      return tenant.Id;
    }
  }
}