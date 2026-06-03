using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity
{
  public class CurrentUserService : ICurrentUser
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId => GetGuidClaim("userId");

    public Guid TenantId => GetGuidClaim("tenantId");

    public string Email => GetClaim("email");

    public string Role => GetClaim(ClaimTypes.Role);

    private string GetClaim(string type)
    {
      return _httpContextAccessor.HttpContext?.User?.FindFirstValue(type) ?? string.Empty;
    }

    private Guid GetGuidClaim(string type)
    {
      var value = GetClaim(type);

      return Guid.TryParse(value, out var result) ? result : Guid.Empty;
    }
  }
}