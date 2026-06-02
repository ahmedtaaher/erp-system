using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence
{
  public class TenantProvider : ITenantProvider
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor accessor)
    {
      _httpContextAccessor = accessor;
    }

    public Guid TenantId
    {
      get
      {
        var value = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        
        return Guid.TryParse(value, out var tenantId)? tenantId: Guid.Empty;
      }
    }
  }
}